using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sandbox;

public partial class Entity
{
	[Hide]
	IEntity IEntity.Parent => Parent;
	[Hide]
	IEntity IEntity.Owner => Owner;

	/// <summary>
	/// What this entity is parented to, if anything. Will make this entity move with it's parent, but loses collisions.
	/// </summary>
	[Hide, JsonIgnore]
	public virtual Entity Parent
	{
		get => GameObject.Parent;
		set => GameObject.Parent = value;
	}

	/// <summary>
	/// Gets the top most parent entity. If we don't have a parent, it might be us.
	/// </summary>
	[Hide]
	public virtual Entity Root => Parent?.Root ?? this;

	/// <inheritdoc cref="P:Legacy.IEntity.Owner" />
	[Hide, JsonIgnore]
	public virtual Entity Owner { get; set; }

	/// <summary>
	/// All entities that are parented to this entity.
	/// </summary>
	[Hide]
	public List<Entity> Children => [.. GameObject.Children];

	/// <summary>
	/// Become a child of this entity.
	/// </summary>
	public virtual void SetParent( Entity entity ) => GameObject.SetParent( entity );

	/// <summary>
	/// Set the parent to the passed entity
	/// </summary>
	/// <param name="entity"></param>
	/// <param name="boneMerge"></param>
	public void SetParent( Entity entity, bool boneMerge )
	{
		SetParent( entity );

		if ( boneMerge )
		{
			if ( this is not AnimatedEntity { Renderer: var self } )
			{
				return;
			}

			if ( entity is not AnimatedEntity { Renderer: var target } )
			{
				return;
			}

			self.BoneMergeTarget = target;
		}
	}

	#region World Transform

	[Hide, JsonIgnore]
	public virtual Transform Transform
	{
		get => GameObject.WorldTransform;
		set => GameObject.WorldTransform = value;
	}

	[Hide, JsonIgnore]
	public virtual Vector3 Position
	{
		get => GameObject.WorldPosition;
		set => GameObject.WorldPosition = value;
	}

	[Hide, JsonIgnore]
	public virtual Rotation Rotation
	{
		get => GameObject.WorldRotation;
		set => GameObject.WorldRotation = value;
	}

	/// <summary>
	/// The scale of the entity. 1 is normal.
	/// </summary>
	[Hide, JsonIgnore]
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
	[Hide, JsonIgnore]
	public virtual Vector3 LocalPosition
	{
		get => GameObject.LocalPosition;
		set => GameObject.LocalPosition = value;
	}

	/// <summary>
	/// The entity's local rotation.
	/// </summary>
	[Hide, JsonIgnore]
	public virtual Rotation LocalRotation
	{
		get => GameObject.LocalRotation;
		set => GameObject.LocalRotation = value;
	}

	/// <summary>
	/// The entity's scale relative to its parent's scale. 1 is normal.
	/// </summary>
	[Hide, JsonIgnore]
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
