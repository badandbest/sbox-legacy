using System.Collections.Generic;
using System.Linq;

namespace Sandbox;

public partial class Entity
{
	IEntity IEntity.Parent => Parent;
	IEntity IEntity.Owner => Owner;
	string IEntity.TagList { get => string.Join( " ", Tags ); set => GameObject.Tags.SetFrom( value ); }

	/// <summary>
	/// What this entity is parented to, if anything. Will make this entity move with it's parent, but loses collisions.
	/// </summary>
	[Hide]
	public virtual Entity Parent
	{
		get => Components.Get<Entity>( FindMode.InAncestors );
		set => SetParent( value );
	}

	/// <summary>
	/// Gets the top most parent entity. If we don't have a parent, it might be us.
	/// </summary>
	[Hide]
	public virtual Entity Root => Components.GetAll<Entity>( FindMode.EverythingInSelfAndAncestors ).Last();

	/// <inheritdoc cref="IEntity.Owner" />
	[Hide]
	public virtual Entity Owner { get; set; }

	/// <summary>
	/// All entities that are parented to this entity.
	/// </summary>
	[Hide]
	public List<Entity> Children => [.. Components.GetAll<Entity>( FindMode.EverythingInChildren )];

	/// <summary>
	/// Become a child of this entity and follow this attachment or bone if provided.
	/// </summary>
	public virtual void SetParent( Entity entity, string attachmentOrBoneName = null, Transform? transform = null ) => GameObject.SetParent( entity );

	/// <summary>
	/// Set the parent to the passed entity
	/// </summary>
	/// <param name="entity"></param>
	/// <param name="boneMerge"></param>
	public void SetParent( Entity entity, bool boneMerge )
	{
		SetParent( entity, boneMerge ? "!bonemerge" : null, Transform.Zero );
	}

	#region World Transform

	[Hide]
	public new Transform Transform
	{
		get => WorldTransform;
		set => WorldTransform = value;
	}

	[Hide]
	public Vector3 Position
	{
		get => WorldPosition;
		set => WorldPosition = value;
	}

	[Hide]
	public Rotation Rotation
	{
		get => WorldRotation;
		set => WorldRotation = value;
	}

	/// <summary>
	/// The scale of the entity. 1 is normal.
	/// </summary>
	[Hide]
	public float Scale
	{
		get => WorldScale.x;
		set => WorldScale = value;
	}

	/// <summary>
	/// The entity's scale relative to its parent's scale. 1 is normal.
	/// </summary>
	[Hide]
	public new float LocalScale
	{
		get => GameObject.LocalScale.x;
		set => GameObject.LocalScale = value;
	}

	#endregion

	/// <summary>
	/// Get a ray representing where this entity is aiming. This can differ from the entity's
	/// position and rotation, for example in a first-person shooter the player's body is usually
	/// aiming in a different direction to the player's eyes.
	/// </summary>
	[Hide]
	public virtual Ray AimRay => Transform.ForwardRay;
}
