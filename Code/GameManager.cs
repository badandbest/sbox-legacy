using System.Linq;
using Sandbox;
using Sandbox.Network;
using System.Threading.Tasks;
using static Legacy.GameManager;

namespace Legacy;

/// <summary>
/// This is the main base game.
/// </summary>
[Category( "Setup" ), Title( "Game" ), Icon( "sports_esports" )]
public abstract class GameManager : Entity
{
	/// <summary>
	/// Currently active game entity.
	/// </summary>
	public static GameManager Current { get; protected internal set; }

	/// <summary>
	/// Client has joined the server. Create their puppets.
	/// </summary>
	public virtual void ClientJoined( IClient client )
	{
		Log.Info( $"\"{client}\" has joined the game." );
	}

	/// <summary>
	/// Client has disconnected from the server. Remove their entities etc.
	/// </summary>
	public virtual void ClientDisconnect( IClient cl )
	{
		Log.Info( $"\"{cl.Name}\" has left the game." );

		if ( cl.Pawn.IsValid() )
		{
			cl.Pawn.Delete();
			cl.Pawn = null;
		}
	}

	/// <summary>
	/// Called each tick.
	/// Serverside: Called for each client every tick
	/// Clientside: Called for each tick for local client. Can be called multiple times per tick.
	/// </summary>
	public override void Simulate( IClient cl )
	{
		if ( cl.Pawn is Entity entity && entity.IsValid() /*&& entity.IsAuthority*/ )
		{
			entity.Simulate( cl );
		}
	}

	/// <summary>
	/// Called each frame on the client only to simulate things that need to be updated every frame. An example
	/// of this would be updating their local pawn's look rotation so it updates smoothly instead of at tick rate.
	/// </summary>
	public override void FrameSimulate( IClient cl )
	{
		if ( cl.Pawn is Entity entity && entity.IsValid() /*&& entity.IsAuthority*/ )
		{
			entity.FrameSimulate( cl );
		}
	}

	/// <summary>
	/// Clientside only. Called every frame to process the input.
	/// The results of this input are encoded into a user command and passed to the PlayerController both clientside and serverside.
	/// This routine is mainly responsible for taking input from mouse/controller and building look angles and move direction.
	/// </summary>
	public override void BuildInput()
	{
		Game.LocalPawn?.BuildInput();
	}
}

internal sealed class InternalGameManager( Scene scene )
	: GameObjectSystem( scene ), ISceneLoadingEvents, Component.INetworkListener
{
	/// <summary>
	/// Load map.
	/// </summary>
	public async Task OnLoad( Scene scene, SceneLoadOptions options )
	{
		var map = LaunchArguments.Map;

		if ( Package.TryParseIdent( map, out var parsed ) )
		{
			var package = await Package.FetchAsync( map, false );
			var files = await package.MountAsync();

			map = package.PrimaryAsset;
		}

		_ = new Map( map, new LegacyMapLoader( scene.SceneWorld, scene.PhysicsWorld ) );
	}

	/// <summary>
	/// Initialize GameManager.
	/// </summary>
	/// <param name="scene"></param>
	public void AfterLoad( Scene scene )
	{
		if ( Sandbox.Game.InGame ) return;

		Current = TypeLibrary.GetTypes<GameManager>().Single( x => !x.IsAbstract ).Create<GameManager>();

		Listen( Stage.StartFixedUpdate, 0, () => Current.Simulate( Game.LocalClient ), nameof( Current.Simulate ) );
		Listen( Stage.StartUpdate, 0, () => Current.FrameSimulate( Game.LocalClient ), nameof( Current.FrameSimulate ) );
		Listen( Stage.StartUpdate, 1, () => Current.BuildInput(), nameof( Current.BuildInput ) );
		Listen( Stage.FinishUpdate, 0, () => GameEvent.Run( "camera.post" ), "camera.post" );

		Networking.CreateLobby( new LobbyConfig() );
	}

	public void OnActive( Connection channel )
	{
		var cl = new ClientEntity( channel );

		Current.ClientJoined( cl );
	}

	public void OnDisconnected( Connection channel )
	{
		var cl = Game.Clients.Single( cl => cl.SteamId == channel.SteamId );

		Current.ClientDisconnect( cl );
		cl.Delete();
	}
}
