using System;
using System.Linq;
using Sandbox;

namespace Legacy;

/// <summary>
/// Wrapper for scene trace to work like it used to in the entity system.
/// </summary>
public struct Trace( SceneTrace trace )
{
	private static SceneTrace SceneTrace => Sandbox.Game.SceneTrace;

	/// <summary>
	/// Casts a sphere from point A to point B.
	/// </summary>
	public static Trace Sphere( float radius, in Vector3 from, in Vector3 to )
	{
		return new( SceneTrace.Sphere( radius, from, to ) );
	}

	/// <summary>
	/// Casts a sphere from a given position and direction, up to a given distance.
	/// </summary>
	public static Trace Sphere( float radius, in Ray ray, in float distance )
	{
		return new( SceneTrace.Sphere( radius, ray, distance ) );
	}

	/// <summary>
	/// Casts a box from point A to point B.
	/// </summary>
	public static Trace Box( Vector3 extents, in Vector3 from, in Vector3 to )
	{
		return new( SceneTrace.Box( extents, from, to ) );
	}

	/// <summary>
	/// Casts a box from a given position and direction, up to a given distance.
	/// </summary>
	public static Trace Box( Vector3 extents, in Ray ray, in float distance )
	{
		return new( SceneTrace.Box( extents, ray, distance ) );
	}

	/// <summary>
	/// Casts a box from point A to point B.
	/// </summary>
	public static Trace Box( BBox bbox, in Vector3 from, in Vector3 to )
	{
		return new( SceneTrace.Box( bbox, from, to ) );
	}

	/// <summary>
	/// Casts a box from a given position and direction, up to a given distance.
	/// </summary>
	public static Trace Box( BBox bbox, in Ray ray, in float distance )
	{
		return new( SceneTrace.Box( bbox, ray, distance ) );
	}

	/// <summary>
	/// Casts a capsule from point A to point B.
	/// </summary>
	public static Trace Capsule( Capsule capsule, in Vector3 from, in Vector3 to )
	{
		return new( SceneTrace.Capsule( capsule, from, to ) );
	}

	/// <summary>
	/// Casts a capsule from a given position and direction, up to a given distance.
	/// </summary>
	public static Trace Capsule( Capsule capsule, in Ray ray, in float distance )
	{
		return new( SceneTrace.Capsule( capsule, ray, distance ) );
	}

	/// <summary>
	/// Casts a ray from point A to point B.
	/// </summary>
	public static Trace Ray( in Vector3 from, in Vector3 to )
	{
		return new( SceneTrace.Ray( from, to ) );
	}

	/// <summary>
	/// Casts a ray from a given position and direction, up to a given distance.
	/// </summary>
	public static Trace Ray( in Ray ray, in float distance )
	{
		return new( SceneTrace.Ray( ray, distance ) );
	}

	/// <summary>
	/// Casts a PhysicsBody from its current position and rotation to desired end point.
	/// </summary>
	public static Trace Body( PhysicsBody body, in Vector3 to )
	{
		return new( SceneTrace.Body( body, to ) );
	}

	/// <summary>
	/// Casts a PhysicsBody from a position and rotation to desired end point.
	/// </summary>
	public static Trace Body( PhysicsBody body, in Transform from, in Vector3 to )
	{
		return new( SceneTrace.Body( body, from, to ) );
	}

	/// <summary>
	/// Sweeps each <see cref="T:Sandbox.PhysicsShape">PhysicsShape</see> of given PhysicsBody and returns the closest collision. Does not support Mesh PhysicsShapes.
	/// Basically 'hull traces' but with physics shapes.
	/// Same as tracing a body but allows rotation to change during the sweep.
	/// </summary>
	public static Trace Sweep( in PhysicsBody body, in Transform from, in Transform to )
	{
		return new( SceneTrace.Sweep( body, from, to ) );
	}

	/// <summary>
	/// Creates a Trace.Sweep using the <see cref="T:Sandbox.PhysicsBody">PhysicsBody</see>'s position as the starting position.
	/// </summary>
	public static Trace Sweep( in PhysicsBody body, in Transform to )
	{
		return new( SceneTrace.Sweep( body, to ) );
	}

	/// <summary>
	/// Sets the start and end positions of the trace request
	/// </summary>
	public readonly Trace FromTo( in Vector3 from, in Vector3 to ) => new( trace.FromTo( from, to ) );

	/// <summary>
	/// Marks given entity to be ignored by the trace (the trace will go through it). Has a limit of 2 entities at this time.
	/// </summary>
	public readonly Trace Ignore( in IEntity ent, in bool hierarchy = true )
	{
		if ( ent is not Entity entity ) return this;

		return new( hierarchy
			? trace.IgnoreGameObjectHierarchy( entity.GameObject )
			: trace.IgnoreGameObject( entity.GameObject ) );
	}

	/// <summary>
	/// This trace should only hit static objects.
	/// </summary>
	public Trace StaticOnly() => new( trace.IgnoreKeyframed().IgnoreDynamic() );

	/// <summary>
	/// This trace should only hit dynamic objects.
	/// </summary>
	public Trace DynamicOnly() => new( trace.IgnoreStatic().IgnoreKeyframed() );

	/// <summary>
	/// Makes this trace an axis aligned box of given size. Extracts mins and maxs from the Bounding Box.
	/// </summary>
	public readonly Trace Size( in BBox hull ) => new( trace.Size( hull ) );

	/// <summary>
	/// Makes this trace an axis aligned box of given size. Calculates mins and maxs by assuming given size is (maxs-mins) and the center is in the middle.
	/// </summary>
	public readonly Trace Size( in Vector3 size ) => new( trace.Size( size ) );

	/// <summary>
	/// Makes this trace an axis aligned box of given size.
	/// </summary>
	public readonly Trace Size( in Vector3 mins, in Vector3 maxs ) => new( trace.Size( mins, maxs ) );

	/// <summary>
	/// Makes this trace a sphere of given radius.
	/// </summary>
	public readonly Trace Radius( float radius ) => new( trace.Radius( radius ) );

	/// <summary>
	/// Should we hit hitboxes
	/// </summary>
	public readonly Trace UseHitboxes( bool hit = true ) => new( trace.UseHitboxes( hit ) );

	/// <summary>
	/// By default traces only hit entities and world that also exist on the server. This is the most common scenario as
	/// if you hit client only entities during prediction you will get prediction errors.
	/// You might however want to opt into including client entities in specific traces.
	/// </summary>
	public readonly Trace IncludeClientside( bool include = true ) => throw new NotImplementedException();

	/// <summary>
	/// Only return entities with this tag. Subsequent calls to this will add multiple requirements
	/// and they'll all have to be met (ie, the entity will need all tags).
	/// </summary>
	public Trace WithTag( string tag ) => new( trace.WithTag( tag ) );

	/// <summary>
	/// Only return entities with all of these tags
	/// </summary>
	public Trace WithAllTags( params string[] tags ) => new( trace.WithAllTags( tags ) );

	/// <summary>
	/// Only return entities with any of these tags
	/// </summary>
	public Trace WithAnyTags( params string[] tags ) => new( trace.WithAnyTags( tags ) );

	/// <summary>
	/// Only return entities without any of these tags
	/// </summary>
	public Trace WithoutTags( params string[] tags ) => new( trace.WithoutTags( tags ) );

	/// <summary>
	/// Returns true if point has an object
	/// </summary>
	public static bool TestPoint( Vector3 point, string tag = "solid", float radius = 0.5f )
	{
		var tr = Sphere( radius, point, point )
			.WithTag( tag )
			.Run();

		return tr.Hit;
	}

	/// <summary>
	/// Run the trace and return the result. The result will return the first hit.
	/// If the trace didn't hit then <see cref="F:Sandbox.TraceResult.Hit">TraceResult.Hit</see> will be false.
	/// </summary>
	public readonly TraceResult Run() => new( trace.Run() );

	/// <summary>
	/// Run the trace and return the results. This will return every entity hit in the
	/// order that they were hit. It will only hit each entity once.
	/// </summary>
	public TraceResult[] RunAll() => [.. trace.RunAll().Select( x => new TraceResult( x ) )];
}
