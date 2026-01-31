using Sandbox.Diagnostics;
using System;

namespace Sandbox;

public partial class ModelEntity : Component.ITriggerListener
{
	Collider _collider;
	Rigidbody _rigidbody;

	/// <summary>
	/// The first PhysicsBody of this entity, if there is one, for convenience purposes.
	/// To access other <see cref="Sandbox.PhysicsBody">PhysicsBodies</see>, use <see cref="Entity.PhysicsGroup">PhysicsGroup</see>.
	/// </summary>
	[Hide]
#pragma warning disable CS0618 // Collider.KeyframeBody is required for backward compatibility.
	public PhysicsBody PhysicsBody => _rigidbody?.PhysicsBody ?? _collider?.KeyframeBody;
#pragma warning restore CS0618

	public override Vector3 Velocity
	{
		get => PhysicsBody.IsValid() ? PhysicsBody.Velocity : field;
		set
		{
			field = value;

			if ( PhysicsBody.IsValid() )
			{
				PhysicsBody.Velocity = value;
			}
		}
	}

	public override Angles AngularVelocity
	{
		get => PhysicsBody.IsValid() ? new( PhysicsBody.AngularVelocity ) : field;
		set
		{
			field = value;

			if ( PhysicsBody.IsValid() )
			{
				PhysicsBody.AngularVelocity = value.AsVector3();
			}
		}
	}

	/// <summary>
	/// The collision bounds.
	/// </summary>
	[Hide]
	public BBox CollisionBounds
	{
		get => Model.Bounds;
		set => throw new NotImplementedException();
	}

	/// <summary>
	/// Enable or disable physics motion.
	/// </summary>
	public bool PhysicsEnabled { get; set; }

	/// <summary>
	/// Enable physics collisions for this entity. Input value is ignored. This is set automatically when setting up physics from a model.
	/// </summary>
	public bool UsePhysicsCollision { set { } }

	/// <summary>
	/// Position of the collision model. Will be same as <see cref="Entity.Position" /> in most cases.
	/// </summary>
	/// <remarks>
	/// TODO PAINDAY: This (and the others below) probably needs to be removed, or properly documented why it is needed.
	/// From what I can see, their implementation is just returning the entity's abs origin/angles.
	/// </remarks>
	[Category( "Collision" )]
	public Vector3 CollisionPosition => PhysicsBody?.Position ?? Position;

	/// <summary>
	/// Rotation of the collision model. Will be same as <see cref="Rotation" /> in most cases.
	/// </summary>
	[Category( "Collision" )]
	public Rotation CollisionRotation => PhysicsBody?.Rotation ?? Rotation;

	/// <summary>
	/// Center of the collision model (OBB) in world-space coordinates. Will be same as <see cref="Entity.WorldSpaceBounds" />.Center in most cases.
	/// </summary>
	[Category( "Collision" )]
	public Vector3 CollisionWorldSpaceCenter => PhysicsBody?.GetBounds().Center ?? WorldSpaceBounds.Center;

	/// <summary>
	/// Enable or disable <see cref="EnableSolidCollisions" />, <see cref="EnableTraceAndQueries" /> and <see cref="EnableTouch" /> at once.
	/// </summary>
	[Category( "Collision" )]
	public bool EnableAllCollisions { set => EnableSolidCollisions = EnableTraceAndQueries = EnableTouch = value; }

	/// <summary>
	/// Use hitboxes for traces etc.
	/// </summary>
	[Category( "Collision" )]
	public bool EnableHitboxes { get; set; }

	/// <summary>
	/// Allow ragdoll parts to collide with each other.
	/// </summary>
	[Category( "Collision" )]
	public bool EnableSelfCollisions { get; set; }

	/// <summary>
	/// Enables or disables solid collisions (as in, affects only physics collisions, not trace and touch events) between this and other entities.<br />
	/// This value will propagate to all child <see cref="PhysicsBody">PhysicsBodies</see>.
	/// </summary>
	[Category( "Collision" )]
	public bool EnableSolidCollisions { get; set; }

	/// <summary>
	/// Decides whether <see cref="Entity.StartTouch" />, <see cref="Entity.Touch" /> and <see cref="Entity.EndTouch" /> callbacks are allowed to be called.<br />
	/// This value will propagate to all child <see cref="PhysicsBody">PhysicsBodies</see>.<br />
	/// Touch events are propagated to parent entities on the server!<br />
	/// </summary>
	[Category( "Collision" )]
	public bool EnableTouch { get; set; }
	void ITriggerListener.OnTriggerEnter( GameObject other ) { if ( EnableTouch ) StartTouch( other ); }
	void ITriggerListener.OnTriggerExit( GameObject other ) { if ( EnableTouch ) EndTouch( other ); }

	/// <summary>
	/// Continuously call <see cref="Entity.Touch" /> for each entity that is touching us, rather than once on touch start.
	/// This value will propagate to all child <see cref="PhysicsBody">PhysicsBodies</see>.
	/// </summary>
	[Category( "Collision" )]
	public bool EnableTouchPersists { get; set; }

	/// <summary>
	/// Allow or disallow traces to hit this entity, and for entity queries (e.g. Entity.FindInBox) to detect it.
	/// This value will propagate to all child <see cref="PhysicsBody">PhysicsBodies</see>.
	/// </summary>
	[Category( "Collision" )]
	public bool EnableTraceAndQueries { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

	/// <summary>
	/// Set the method used to work out the surrounding bounds. The bounds are important for
	/// traces and collision checks, because they're used the quickly eliminate collisions.
	///
	/// <para>Affects <see cref="Entity.WorldSpaceBounds" />.</para>
	/// </summary>
	[Category( "Collision" )]
	public SurroundingBoundsType SurroundingBoundsMode { get; set; } = SurroundingBoundsType.ObjectBoundingBox;

	/// <summary>
	/// Destroy all physics objects.
	/// </summary>
	public virtual void PhysicsClear()
	{
		_collider?.Destroy();
		_rigidbody?.Destroy();

		_collider = null;
		_rigidbody = null;
	}

	T SetupCollider<T>( PhysicsMotionType motionType, PhysicsLock locks = default ) where T : Collider, new()
	{
		PhysicsClear();

		const ComponentFlags procedural = ComponentFlags.NotNetworked | ComponentFlags.NotSaved | ComponentFlags.NotCloned;

		_collider = AddComponent<T>();
		_collider.Flags |= procedural;

		if ( motionType == PhysicsMotionType.Static )
		{
			_collider.Static = true;
		}
		if ( motionType == PhysicsMotionType.Dynamic )
		{
			_rigidbody = AddComponent<Rigidbody>();
			_rigidbody.Flags |= procedural;
		}

		return (T)_collider;
	}

	/// <summary>
	/// Clears all physics bodies and sets up physics from given <a href="https://en.wikipedia.org/wiki/Minimum_bounding_box">AABB</a> that is always upright.
	/// Requires non dynamic motion type.
	/// </summary>
	/// <param name="motionType">Are we static or dynamic?</param>
	/// <param name="mins">The vector with minimum corner of the AABB relative to entity's origin.</param>
	/// <param name="maxs">The vector with maximum corner of the AABB relative to entity's origin.</param>
	/// <returns>The newly created physics group.</returns>
	/// <exception cref="ArgumentException">Thrown if invalid motion type is given.</exception>
	public PhysicsGroup SetupPhysicsFromAABB( PhysicsMotionType motionType, Vector3 mins, Vector3 maxs )
	{
		Assert.AreNotEqual( motionType, PhysicsMotionType.Invalid, "Invalid motion type given" );
		Assert.AreNotEqual( motionType, PhysicsMotionType.Dynamic, "Does not support PhysicsMotionType.Dynamic" );

		var collider = SetupCollider<BoxCollider>( motionType );

		collider.Scale = maxs - mins;
		collider.Center = (mins + maxs) * 0.5f;

		PhysicsBody.Locking = PhysicsLock.Rotation;

		return default;
	}

	/// <summary>
	/// Clears all physics bodies and sets up physics from given <a href="https://en.wikipedia.org/wiki/Minimum_bounding_box">OBB</a>.
	/// </summary>
	/// <param name="motionType">Are we static or dynamic?</param>
	/// <param name="mins">The vector with minimum corner of the OBB relative to entity's origin.</param>
	/// <param name="maxs">The vector with maximum corner of the OBB relative to entity's origin.</param>
	/// <returns>The newly created physics group.</returns>
	/// <exception cref="ArgumentException">Thrown if invalid motion type is given.</exception>
	public PhysicsGroup SetupPhysicsFromOBB( PhysicsMotionType motionType, Vector3 mins, Vector3 maxs )
	{
		Assert.AreNotEqual( motionType, PhysicsMotionType.Invalid, "Invalid motion type given" );

		var collider = SetupCollider<BoxCollider>( motionType );

		collider.Scale = maxs - mins;
		collider.Center = (mins + maxs) * 0.5f;

		return default;
	}

	/// <summary>
	/// Clears all physics bodies and sets up physics as a capsule that is always upright. A capsule is a cylinder with round ends.
	/// Requires non dynamic motion type.
	/// </summary>
	/// <param name="motionType">Are we static or dynamic?</param>
	/// <param name="capsule">The capsule data.</param>
	/// <returns>The newly created physics group.</returns>
	/// <exception cref="ArgumentException">Thrown if invalid motion type is given.</exception>
	public PhysicsGroup SetupPhysicsFromCapsule( PhysicsMotionType motionType, Capsule capsule )
	{
		Assert.AreNotEqual( motionType, PhysicsMotionType.Invalid, "Invalid motion type given" );
		Assert.AreNotEqual( motionType, PhysicsMotionType.Dynamic, "Does not support PhysicsMotionType.Dynamic" );

		var collider = SetupCollider<CapsuleCollider>( motionType );

		collider.Start = capsule.CenterA;
		collider.End = capsule.CenterB;
		collider.Radius = capsule.Radius;

		PhysicsBody.Locking = PhysicsLock.Rotation;
		return default;
	}

	/// <summary>
	/// Clears all physics bodies and sets up physics as a capsule. A capsule is a cylinder with round ends.
	/// </summary>
	/// <param name="motionType">Are we static or dynamic?</param>
	/// <param name="capsule">The capsule data.</param>
	/// <returns>The newly created physics group.</returns>
	/// <exception cref="ArgumentException">Thrown if invalid motion type is given.</exception>
	public PhysicsGroup SetupPhysicsFromOrientedCapsule( PhysicsMotionType motionType, Capsule capsule )
	{
		Assert.AreNotEqual( motionType, PhysicsMotionType.Invalid, "Invalid motion type given" );

		var collider = SetupCollider<CapsuleCollider>( motionType );

		collider.Start = capsule.CenterA;
		collider.End = capsule.CenterB;
		collider.Radius = capsule.Radius;

		return default;
	}

	/// <summary>
	/// Clears all physics bodies and sets up physics as a cylinder that is always upright.
	/// Requires non dynamic motion type.
	/// </summary>
	/// <param name="motionType">Are we static or dynamic?</param>
	/// <param name="capsule">The cylinder data.</param>
	/// <returns>The newly created physics group.</returns>
	/// <exception cref="ArgumentException">Thrown if invalid motion type is given.</exception>
	public PhysicsGroup SetupPhysicsFromCylinder( PhysicsMotionType motionType, Capsule capsule )
	{
		Assert.AreNotEqual( motionType, PhysicsMotionType.Invalid, "Invalid motion type given" );
		Assert.AreNotEqual( motionType, PhysicsMotionType.Dynamic, "Does not support PhysicsMotionType.Dynamic" );

		var collider = SetupCollider<HullCollider>( motionType );

		collider.Type = HullCollider.PrimitiveType.Cylinder;
		collider.Center = (capsule.CenterA + capsule.CenterB) * 0.5f;
		collider.Height = capsule.CenterA.Distance( capsule.CenterB );
		collider.Radius = capsule.Radius;

		PhysicsBody.Locking = PhysicsLock.Rotation;
		return default;
	}

	/// <summary>
	/// Clears all physics bodies and sets up physics as a single sphere.
	/// </summary>
	/// <param name="motionType">Are we static or dynamic?</param>
	/// <param name="center">Center of the sphere, relative to entity's origin.</param>
	/// <param name="radius">Radius of the sphere.</param>
	/// <returns>The newly created physics group.</returns>
	/// <exception cref="ArgumentException">Thrown if invalid motion type is given.</exception>
	public PhysicsGroup SetupPhysicsFromSphere( PhysicsMotionType motionType, Vector3 center, float radius )
	{
		Assert.AreNotEqual( motionType, PhysicsMotionType.Invalid, "Invalid motion type given" );

		var collider = SetupCollider<SphereCollider>( motionType );

		collider.Center = center;
		collider.Radius = radius;

		return default;
	}

	/// <summary>
	/// Clears all physics bodies and sets up physics from the model's data as set up in ModelDoc.
	/// </summary>
	/// <param name="motionType">Are we static or dynamic?</param>
	/// <param name="startasleep">Whether all physics bodies should start asleep. See <see cref="PhysicsBody.Sleeping" />.</param>
	/// <returns>The newly created physics group.</returns>
	/// <exception cref="ArgumentException">Thrown if invalid motion type is given.</exception>
	public PhysicsGroup SetupPhysicsFromModel( PhysicsMotionType motionType, bool startasleep = false )
	{
		Assert.AreNotEqual( motionType, PhysicsMotionType.Invalid, "Invalid motion type given" );

		var collider = SetupCollider<ModelCollider>( motionType );

		collider.Model = Model;

		return default;
	}
}

/// <summary>
/// Dictates how surrounding bounds (Entity.WorldSpaceBounds) of an entity as calculated.
/// </summary>
public enum SurroundingBoundsType : byte
{
	/// <summary>
	/// To contain the object's OBB.
	/// </summary>
	ObjectBoundingBox,

	/// <summary>
	/// The most expensive option. Work it out using the physics objects.
	/// If it's a ragdoll it'll contain each physics object.
	/// </summary>
	Physics,

	/// <summary>
	/// Expand the surrounding bounds to encompass all hitboxes of the model.
	/// </summary>
	Hitboxes
}
