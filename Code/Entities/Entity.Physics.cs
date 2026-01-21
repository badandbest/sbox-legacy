namespace Sandbox;

public partial class Entity
{
	/// <summary>
	/// What this entity is "standing" on.
	/// </summary>
	[Hide]
	public Entity GroundEntity { get; set; }

	#region Unimplemented

	[Hide]
	public BBox WorldSpaceBounds { get; }
	[Hide]
	public Vector3 Velocity { get; set; }

	#endregion
}
