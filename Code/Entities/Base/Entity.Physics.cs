using Sandbox;

namespace Legacy;

public partial class Entity
{
	/// <summary>
	/// What this entity is "standing" on.
	/// </summary>
	[Hide]
	public Entity GroundEntity { get; set; }
}
