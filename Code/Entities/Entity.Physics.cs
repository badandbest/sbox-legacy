namespace Sandbox;

public partial class Entity
{
	string IEntity.TagList
	{
		get => string.Join( " ", Tags );
		set => GameObject.Tags.SetFrom( value );
	}

	/// <summary>
	/// What this entity is "standing" on.
	/// </summary>
	[Hide]
	public Entity GroundEntity { get; set; }

	#region Unimplemented

	public BBox WorldSpaceBounds { get; }
	public Vector3 Velocity { get; set; }

	#endregion
}
