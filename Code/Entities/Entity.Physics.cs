namespace Sandbox;

public partial class Entity : Component.ICollisionListener
{
	/// <summary>
	/// What this entity is "standing" on.
	/// </summary>
	[Hide]
	public Entity GroundEntity { get; set; }

	[Hide]
	public BBox WorldSpaceBounds { get; }

	[Hide]
	public virtual Vector3 Velocity { get; set; }

	/// <summary>
	/// Velocity in local coordinates.
	/// </summary>
	[Hide]
	public virtual Vector3 LocalVelocity
	{
		get => Rotation.Inverse * Velocity;
		set => Velocity = Rotation * value;
	}

	/// <summary>
	/// The angular velocity in local coordinates.
	/// </summary>
	[Hide]
	public virtual Angles AngularVelocity { get; set; }

	/// <summary>
	/// Generally describes the velocity of this object that is caused by its parent moving.
	/// Examples would be conveyor belts and elevators.
	/// </summary>
	[Hide]
	public Vector3 BaseVelocity { get; set; }

	/// <summary>
	/// If this entity has multiple physics objects, a physics group lets you control them all as one.
	/// </summary>
	[Hide]
	public PhysicsGroup PhysicsGroup => null;

	/// <summary>
	/// Calls <see cref="PhysicsBody.ApplyImpulse" /> on all bodies of this entity with world-space input.
	/// </summary>
	public void ApplyAbsoluteImpulse( Vector3 impulse ) => Velocity += impulse;

	/// <summary>
	/// Calls <see cref="PhysicsBody.ApplyAngularImpulse" /> on all bodies of this entity.
	/// </summary>
	public void ApplyLocalAngularImpulse( Vector3 impulse ) => AngularVelocity += impulse;

	/// <summary>
	/// Calls <see cref="PhysicsBody.ApplyImpulse" /> on all bodies of this entity local-space input. (Relative to the entity)
	/// </summary>
	public void ApplyLocalImpulse( Vector3 impulse ) => LocalVelocity += impulse;

	/// <summary>
	/// Internal only: update all of the collision rules, push those rules onto physics shapes.
	/// </summary>
	protected void CollisionRulesChanged() { }

	/// <summary>
	/// An entity has stopped touching this trigger entity. Requires <see cref="ModelEntity.EnableTouch">EnableTouch</see> enabled on this entity.
	/// </summary>
	public virtual void StartTouch( Entity other ) { }

	/// <summary>
	/// An entity has touched this trigger entity. Requires <see cref="ModelEntity.EnableTouch">EnableTouch</see> enabled on this entity.
	///
	/// See also <see cref="ModelEntity.EnableTouchPersists" />.
	/// </summary>
	public virtual void Touch( Entity other ) { }

	/// <summary>
	/// An entity has stopped touching this trigger entity. Requires <see cref="ModelEntity.EnableTouch">EnableTouch</see> enabled on this entity.
	/// </summary>
	public virtual void EndTouch( Entity other ) { }

	/// <summary>
	/// Called when this entity collides with anything else via Physics.
	/// </summary>
	protected virtual void OnPhysicsCollision( CollisionEventData eventData ) { }
	void ICollisionListener.OnCollisionStart( Collision collision ) => OnPhysicsCollision( new CollisionEventData( collision ) );
}
