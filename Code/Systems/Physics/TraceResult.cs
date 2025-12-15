namespace Sandbox;

public struct TraceResult( SceneTraceResult result )
{
	/// <summary>
	/// Whether the trace hit something or not
	/// </summary>
	public bool Hit = result.Hit;

	/// <summary>
	/// Whether the trace started in a solid
	/// </summary>
	public bool StartedSolid = result.StartedSolid;

	/// <summary>
	/// The start position of the trace
	/// </summary>
	public Vector3 StartPosition = result.StartPosition;

	/// <summary>
	/// The end or hit position of the trace
	/// </summary>
	public Vector3 EndPosition = result.EndPosition;

	/// <summary>
	/// The hit position of the trace
	/// </summary>
	public Vector3 HitPosition = result.HitPosition;

	/// <summary>
	/// The hit surface normal (direction vector)
	/// </summary>
	public Vector3 Normal = result.Normal;

	/// <summary>
	/// A fraction [0..1] of where the trace hit between the start and the original end positions
	/// </summary>
	public float Fraction = result.Fraction;

	/// <summary>
	/// The entity that was hit, if any
	/// </summary>
	public Entity Entity = result.GameObject;

	/// <summary>
	/// The physics object that was hit, if any
	/// </summary>
	public PhysicsBody Body = result.Body;

	/// <summary>
	/// The physics shape that was hit, if any
	/// </summary>
	public PhysicsShape Shape = result.Shape;

	/// <summary>
	/// The physical properties of the hit surface
	/// </summary>
	public Surface Surface = result.Surface;

	/// <summary>
	/// The id of the hit bone (either from hitbox or physics shape)
	/// </summary>
	public int Bone = result.Bone;

	/// <summary>
	/// The direction of the trace ray
	/// </summary>
	public Vector3 Direction = result.Direction;

	/// <summary>
	/// The triangle index hit, if we hit a mesh <see cref="T:Sandbox.PhysicsShape">physics shape</see>
	/// </summary>
	public int Triangle = result.Triangle;

	/// <summary>
	/// The tags that the hit shape had
	/// </summary>
	public string[] Tags = result.Tags;

	/// <summary>
	/// The hitbox hit, if at all. Requires <see cref="M:Sandbox.Trace.UseHitboxes(System.Boolean)">Trace.UseHitboxes</see> to work.
	/// </summary>
	public Hitbox Hitbox = result.Hitbox;

	/// <summary>
	/// The distance between start and end positions.
	/// </summary>
	public float Distance => Vector3.DistanceBetween( StartPosition, EndPosition );
}
