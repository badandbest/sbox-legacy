using System.Linq;
using Sandbox;
using Sandbox.Network;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Legacy.GameManager;

namespace Legacy;

internal sealed class NativeGameManager( Scene scene ) : GameObjectSystem( scene ), ISceneLoadingEvents, Component.INetworkListener
{
	/// <summary>
	/// Load map.
	/// </summary>
	public async Task OnLoad( Scene scene, SceneLoadOptions options )
	{
		var map = LaunchArguments.Map;
		map ??= "facepunch.flatgrass";

		if ( Package.TryParseIdent( map, out var parsed ) )
		{
			var package = await Package.FetchAsync( map, false );
			var files = await package.MountAsync();

			map = package.PrimaryAsset;
		}

		var loader = new LegacyMapLoader( scene.SceneWorld, scene.PhysicsWorld );
		_ = new Map( map, loader );

		loader.AddCollision();
	}

	/// <summary>
	/// Initialize GameManager.
	/// </summary>
	/// <param name="scene"></param>
	public void AfterLoad( Scene scene )
	{
		if ( !Game.InGame ) return;

		var manager = TypeLibrary.GetTypes<GameManager>().Single( x => !x.IsAbstract ).Create<GameManager>();

		Listen( Stage.StartFixedUpdate, 0, () => manager.Simulate( Game.LocalClient ), nameof( manager.Simulate ) );
		Listen( Stage.StartUpdate, 0, () => manager.FrameSimulate( Game.LocalClient ), nameof( manager.FrameSimulate ) );
		Listen( Stage.StartUpdate, 1, () => manager.BuildInput(), nameof( manager.BuildInput ) );
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

/// <summary>
/// Loads entities with <see cref="HammerEntityAttribute"/>.
/// </summary>
file class LegacyMapLoader : SceneMapLoader
{
	public Dictionary<string, TypeDescription> HammerEntities { get; private set; }

	public LegacyMapLoader( SceneWorld world, PhysicsWorld physics ) : base( world, physics )
	{
		var hammerEntities = TypeLibrary.GetTypesWithAttribute<HammerEntityAttribute>().Select( t => t.Type );
		HammerEntities = hammerEntities.ToDictionary( t => t.GetAttribute<LibraryAttribute>().Name );
	}

	protected override void CreateObject( ObjectEntry kv )
	{
		if ( HammerEntities.TryGetValue( kv.TypeName, out var type ) )
		{
			var entity = type.Create<Entity>();
			entity.Rotation = kv.Rotation;
			entity.Position = kv.Position;
			entity.Scale = kv.Scales.x;
			entity.Tags.Add( kv.Tags );
			entity.Tags.Add( "world" );
		}
		else
		{
			CreateModel( kv );
		}
	}

	public void AddCollision()
	{
		var physics = new Entity();
		physics.GameObject.Name = "worldphysics";
		physics.Tags.Add( "world" );

		foreach ( var body in PhysicsWorld.Bodies )
		{
			body.GameObject = physics;
		}
	}
}
