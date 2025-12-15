namespace Sandbox;

public partial class Entity
{
	string IEntity.TagList
	{
		get => string.Join( " ", Tags );
		set => Tags.SetFrom( value );
	}

	/// <summary>
	/// Accessor to add, remove and check entity tags
	/// </summary>
	public GameTags Tags
	{
		get => GameObject.Tags;
		set => GameObject.Tags.SetFrom( value );
	}

	/// <summary>
	/// What this entity is "standing" on.
	/// </summary>
	[Hide]
	public Entity GroundEntity { get; set; }
}
