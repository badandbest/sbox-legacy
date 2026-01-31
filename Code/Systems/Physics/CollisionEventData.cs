namespace Sandbox;

/// <summary>
/// The collision data involving two entities, this is used in <see cref="Entity.OnPhysicsCollision">Entity.OnPhysicsCollision</see>.
/// Each Entity receives this callback with the <see cref="This">This</see> and <see cref="Other">Other</see> swapped.
/// </summary>
public struct CollisionEventData( Collision data )
{
	/// <summary>
	/// Our entity data from the collision.
	/// </summary>
	public CollisionEntityData This = new( data.Self );

	/// <summary>
	/// The other entity data that was involved in this collision.
	/// </summary>
	public CollisionEntityData Other = new( data.Other );

	/// <summary>
	/// The point at which the collision contact happened.
	/// </summary>
	public Vector3 Position = data.Contact.Point;

	/// <summary>
	/// Speed of the contact. speed of surface 1 relative to surface 0
	/// </summary>
	public Vector3 Velocity = data.Contact.Speed;

	/// <summary>
	/// Direction perpendicular to our hit surface.
	/// </summary>
	public Vector3 Normal = data.Contact.Normal;

	/// <summary>
	/// Collision speed.
	/// </summary>
	public float Speed = data.Contact.Impulse;
}

/// <summary>
/// Data on one of the two entities involved in a collision, part of <see cref="CollisionEventData" />.
/// </summary>
public struct CollisionEntityData( CollisionSource source )
{
	/// <summary>
	/// Which entity we came in contact with.
	/// </summary>
	public Entity Entity = source.GameObject;

	/// <summary>
	/// Our physics shape that caused the collision.
	/// </summary>
	public PhysicsShape PhysicsShape = source.Shape;

	/// <summary>
	/// The surface the collision happened on.
	/// </summary>
	public Surface Surface = source.Surface;

	/// <summary>
	/// Our <see cref="Entity.Velocity">velocity</see> before the collision happened.
	/// </summary>
	public Vector3 PreVelocity;

	/// <summary>
	/// Our <see cref="Entity.Velocity">velocity</see> after the collision happened.
	/// </summary>
	public Vector3 PostVelocity;

	/// <summary>
	/// Our angular impulse before the collision happened.<br />
	/// AngularImpulse are exponential maps (an axis scaled by a "twist" angle in radians)
	/// </summary>
	public Vector3 PreAngularVelocity;

	/// <summary>
	/// Our angular impulse after the collision happened.<br />
	/// AngularImpulse are exponential maps (an axis scaled by a "twist" angle in radians)
	/// </summary>
	public Vector3 PostAngularVelocity;
}
