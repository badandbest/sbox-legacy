using System.Collections.Generic;

namespace Sandbox;

public partial class Entity
{
	internal Entity parent = null!;

	IEntity IEntity.Parent => Parent;

	public virtual Entity Parent
	{
		get => parent;
		set => SetParent( value );
	}

	/// <summary>
	/// All entities that are parented to this entity.
	/// </summary>
	public List<Entity> Children { get; } = [];

	/// <summary>
	/// Become a child of this entity.
	/// </summary>
	public virtual void SetParent( Entity entity )
	{
		parent?.Children.Remove( this );

		parent = entity;

		parent?.Children.Add( this );
		GameObject.SetParent( entity?.GameObject );
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
