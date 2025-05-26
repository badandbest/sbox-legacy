using System.Collections.Generic;
using Sandbox;

namespace Legacy;

public partial class Entity
{
	private Entity _parent = null!;
	private Entity _owner = null!;

	IEntity IEntity.Parent => Parent;
	IEntity IEntity.Owner => _owner;

	/// <summary>
	/// What this entity is parented to, if anything. Will make this entity move with it's parent, but loses collisions.
	/// </summary>
	[Hide]
	public virtual Entity Parent { get => _parent; set => SetParent( value ); }

	/// <summary>
	/// Gets the top most parent entity. If we don't have a parent, it might be us.
	/// </summary>
	[Hide]
	public virtual Entity Root => Parent?.Root ?? this;

	/// <inheritdoc cref="P:Legacy.IEntity.Owner" />
	[Hide]
	public virtual Entity Owner { get => _owner; set => _owner = value; }

	/// <summary>
	/// All entities that are parented to this entity.
	/// </summary>
	public List<Entity> Children { get; } = [];

	/// <summary>
	/// Become a child of this entity.
	/// </summary>
	public virtual void SetParent( Entity entity )
	{
		_parent?.Children.Remove( this );

		_parent = entity;

		_parent?.Children.Add( this );
		GameObject.SetParent( entity?.GameObject );
	}

	public void SetParent( Entity entity, bool boneMerge )
	{
		if ( boneMerge )
		{
			var target = GameObject.GetComponentInParent<SkinnedModelRenderer>( includeSelf: false );
			GameObject.GetComponent<SkinnedModelRenderer>().BoneMergeTarget = target;
		}

		SetParent( entity );
	}

	#region World Transform

	[Hide]
	public virtual Transform Transform
	{
		get => GameObject.WorldTransform;
		set => GameObject.WorldTransform = value;
	}

	[Hide]
	public virtual Vector3 Position
	{
		get => GameObject.WorldPosition;
		set => GameObject.WorldPosition = value;
	}

	[Hide]
	public virtual Rotation Rotation
	{
		get => GameObject.WorldRotation;
		set => GameObject.WorldRotation = value;
	}

	/// <summary>
	/// The scale of the entity. 1 is normal.
	/// </summary>
	[Hide]
	public virtual float Scale
	{
		get => GameObject.WorldScale.x;
		set => GameObject.WorldScale = value;
	}

	#endregion

	#region Local Transform

	/// <summary>
	/// The entity's position relative to its parent (or the world if no parent)
	/// </summary>
	[Hide]
	public virtual Vector3 LocalPosition
	{
		get => GameObject.LocalPosition;
		set => GameObject.LocalPosition = value;
	}

	/// <summary>
	/// The entity's local rotation.
	/// </summary>
	[Hide]
	public virtual Rotation LocalRotation
	{
		get => GameObject.LocalRotation;
		set => GameObject.LocalRotation = value;
	}

	/// <summary>
	/// The entity's scale relative to its parent's scale. 1 is normal.
	/// </summary>
	[Hide]
	public virtual float LocalScale
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
	public virtual Ray AimRay => Transform.ForwardRay;
}
