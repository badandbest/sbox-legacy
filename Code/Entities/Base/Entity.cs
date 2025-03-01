namespace Sandbox;

/// <summary>
/// A base entity all other entities derive from.
/// </summary>
[Library( "entity" )]
public class Entity : IEntity
{
	/// <summary>
	/// Create the entity.
	/// </summary>
	public Entity()
	{
	}

	#region Unimplemented

	public bool IsValid { get; }
	public IEntity Parent { get; }
	public IEntity Owner { get; }
	public IClient Client { get; }
	public int Id { get; }
	public bool IsDormant { get; }
	public bool IsOwnedByLocalClient { get; }
	public Transform Transform { get; }
	public bool IsFromMap { get; }
	public BBox WorldSpaceBounds { get; }
	public string TagList { get; set; }
	public bool IsClientOnly { get; }
	public int NetworkIdent { get; }
	public Vector3 Velocity { get; set; }
	public Vector3 Position { get; set; }
	public Rotation Rotation { get; set; }
	public bool IsAuthority { get; }
	public Ray AimRay { get; }
	public IComponentSystem Components { get; }
	public void Delete() { }

	#endregion
}
