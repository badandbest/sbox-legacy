using System.Collections.Generic;

namespace Sandbox;

/// <summary>
/// A base entity all other entities derive from.
/// </summary>
[Library( "entity" )]
[Icon( "people" ), Tint( EditorTint.White )]
public partial class Entity : Component, IEntity
{
	/// <summary>
	/// A list of all active entities.
	/// </summary>
	public static IEnumerable<Entity> All => Game.ActiveScene.GetAll<Entity>();

	/// <summary>
	/// Create the entity.
	/// </summary>
	public Entity()
	{
		//
		// For entities, GameObjects need to be set immediately.
		//

		if ( GameObject.Deserializing is GameObject parent )
		{
			TypeLibrary.SetProperty( this, nameof( GameObject ), parent );
			return;
		}

		var go = new GameObject( GetType().Name );
		go.AddComponent( this );
	}

	/// <summary>
	/// Delete this entity. You shouldn't access it anymore.
	/// </summary>
	public void Delete() => DestroyGameObject();

	/// <summary>
	/// Called when the entity is spawned in. It should have all properties set by this point.
	/// This is the place to set up your entity.
	/// </summary>
	public virtual void Spawn() { }

	/// <summary>
	/// Called when simulating as part of a player's tick. Like if it's a pawn.
	/// </summary>
	public virtual void Simulate( IClient cl ) { }

	/// <summary>
	/// Called each frame clientside only on Pawn (and anything the pawn decides to call it on).
	/// </summary>
	public virtual void FrameSimulate( IClient cl ) { }

	/// <summary>
	/// Pawns get a chance to mess with the input. This is called on the client.
	/// </summary>
	public virtual void BuildInput() { }

	/// <summary>
	/// A component has been added to the entity.
	/// </summary>
	protected internal virtual void OnComponentAdded( EntityComponent component ) { }

	/// <summary>
	/// A component has been removed from the entity.
	/// </summary>
	protected internal virtual void OnComponentRemoved( EntityComponent component ) { }

	public static implicit operator GameObject( Entity entity ) => entity.GameObject;

	public static implicit operator Entity( GameObject gameObject ) => gameObject?.GetComponent<Entity>();

	#region Forwarded actions

	protected override void OnStart() => Spawn();

	#endregion
}
