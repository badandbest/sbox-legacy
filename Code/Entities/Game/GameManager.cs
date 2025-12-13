using System.Linq;
using Sandbox;
using System.Collections.Generic;

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
	public static GameManager Current => All.OfType<GameManager>().Single();

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

/// <summary>
/// Loads entities with <see cref="HammerEntityAttribute"/>.
/// </summary>
public class LegacyMapLoader : SceneMapLoader
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
			type.Create<Entity>();
		}
		else
		{
			CreateModel( kv );
		}
	}
}
