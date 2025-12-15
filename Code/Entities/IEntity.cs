namespace Sandbox;

/// <summary>
/// A generic entity.
/// </summary>
public interface IEntity : IValid
{
	/// <summary>
	/// The parent entity.
	/// </summary>
	IEntity Parent { get; }

	/// <summary>
	/// The entity that owns this entity. For example, a pawn's weapon will be owned by its pawn.
	/// </summary>
	IEntity Owner { get; }

	/// <summary>
	/// The client that owns this entity, if any. Usually as a result of the entity being client's pawn, or being attached to the client's pawn.
	/// </summary>
	IClient Client { get; }

	/// <summary>
	/// The entity's unique identifier, usually it's <see cref="NetworkIdent"/>.
	/// </summary>
	int Id { get; }

	/// <summary>
	/// Returns true if this entity is dormant (the client cannot see it, it isn't being networked)
	/// </summary>
	bool IsDormant { get; }

	/// <summary>
	/// Clientside, whether the <see cref="Client"/> is the local client.
	/// </summary>
	bool IsOwnedByLocalClient { get; }

	/// <summary>
	/// Entity's transform from world origin.
	/// </summary>
	Transform Transform { get; }

	/// <summary>
	/// Returns true if this specific entity instance was spawned from the map.
	/// </summary>
	bool IsFromMap { get; }

	/// <summary>
	/// Entity's axis-aligned bounding box (AABB) in world space.
	/// </summary>
	BBox WorldSpaceBounds { get; }

	/// <summary>
	/// List of tags this entity has, separated by spaces.
	/// </summary>
	string TagList { get; set; }

	/// <summary>
	/// True if this entity is a purely clientside entity, with no serverside components.
	/// </summary>
	bool IsClientOnly { get; }

	/// <summary>
	/// The entity's network id. Client only entities have a network id too!
	/// </summary>
	int NetworkIdent { get; }

	/// <summary>
	/// The velocity in world coordinates.
	/// </summary>
	Vector3 Velocity { get; set; }

	/// <summary>
	/// The entity's world position.
	/// </summary>
	Vector3 Position { get; set; }

	/// <summary>
	/// The entity's world rotation.
	/// </summary>
	Rotation Rotation { get; set; }

	/// <summary>
	/// Returns true if we have authority over this entity. This means we're either serverside,
	/// or we're a clientside entity, or we're a serverside entity being predicted on the client.
	/// </summary>
	bool IsAuthority { get; }

	/// <summary>
	/// Get a ray representing where this entity is aiming. You can override this in your entity
	/// to place the aim position at the entity's eye and the direction where it's looking.
	/// </summary>
	Ray AimRay { get; }

	/// <summary>
	/// Access to get, add and remove components.
	/// </summary>
	IComponentSystem Components { get; }

	/// <summary>
	/// Delete this entity.
	/// </summary>
	void Delete();
}
