using System.Collections.Generic;
using Sandbox;

namespace Legacy;

/// <summary>
/// A base entity all other entities derive from.
/// </summary>
[Library( "entity" )]
public partial class Entity : IEntity
{
	/// <summary>
	/// A list of all active entities.
	/// </summary>
	public static List<Entity> All { get; } = [];

	/// <summary>
	/// The game object this entity belongs to.
	/// </summary>
	[Hide]
	public GameObject GameObject { get; }

	/// <summary>
	/// Create the entity.
	/// </summary>
	public Entity()
	{
		GameObject = new GameObject( true, GetType().Name );
		GameObject.Flags |= GameObjectFlags.NotSaved;

		var wrapper = GameObject.AddComponent<EntityWrapper>();
		wrapper.Entity = this;

		Components = new EntityComponentSystem( this );
		All.Add( this );
	}

	public bool IsValid => GameObject.IsValid();

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
	/// Called when the entity was destroyed. This is not the same as the class destructor.
	/// </summary>
	protected virtual void OnDestroy() { }

	/// <summary>
	/// Delete this entity. You shouldn't access it anymore.
	/// </summary>
	public void Delete()
	{
		OnDestroy();

		All.Remove( this );
		GameObject.Destroy();
	}

	#region Unimplemented

	public BBox WorldSpaceBounds { get; }
	public string TagList { get; set; }

	public Vector3 Velocity { get; set; }

	#endregion
}
