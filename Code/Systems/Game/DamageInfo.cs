using Sandbox;
using System.Collections.Generic;

namespace Legacy;

/// <summary>
/// Describes the damage that should be done to something.
/// </summary>
public struct DamageInfo
{
	/// <summary>
	/// The player or NPC or exploding barrel (etc)1 that is attacking
	/// </summary>
	public Entity Attacker { get; set; }

	/// <summary>
	/// The weapon that the attacker is using
	/// </summary>
	public Entity Weapon { get; set; }

	/// <summary>
	/// The position the damage is being inflicted (the bullet entry point)
	/// </summary>
	public Vector3 Position { get; set; }

	/// <summary>
	/// The force of the damage - for moving physics etc. This would be the trajectory
	/// of the bullet multiplied by the speed and mass.
	/// </summary>
	public Vector3 Force { get; set; }

	/// <summary>
	/// The actual amount of damage this attack causes
	/// </summary>
	public float Damage { get; set; }

	/// <summary>
	/// Damage tags, extra information about this attack
	/// </summary>
	public TagSet Tags { get; set; }

	/// <summary>
	/// The physics body that was hit
	/// </summary>
	public PhysicsBody Body { get; set; }

	/// <summary>
	/// The bone index that the hitbox was attached to
	/// </summary>
	public int BoneIndex { get; set; }

	/// <summary>
	/// The hitbox (if any) that was hit
	/// </summary>
	public Hitbox Hitbox { get; set; }

	/// <summary>
	/// Creates a new DamageInfo with the "bullet" tag
	/// </summary>
	public static DamageInfo FromBullet( Vector3 hitPosition, Vector3 hitForce, float damage ) => new()
	{
		Position = hitPosition,
		Damage = damage,
		Force = hitForce,
		Tags = ["bullet"]
	};

	/// <summary>
	/// Creates a new DamageInfo with no tags
	/// </summary>
	public static DamageInfo Generic( float damage ) => new()
	{
		Damage = damage,
		Tags = []
	};

	/// <summary>
	/// Creates a new DamageInfo with the "explosion" tag
	/// </summary>
	public static DamageInfo FromExplosion( Vector3 sourcePosition, Vector3 force, float damage ) => new()
	{
		Position = sourcePosition,
		Force = force,
		Damage = damage,
		Tags = ["explosion", "blast"]
	};

	/// <summary>
	/// Set the attacker
	/// </summary>
	public DamageInfo WithAttacker( Entity attacker ) => this with { Attacker = attacker };

	/// <summary>
	/// Set the attacker
	/// </summary>
	public DamageInfo WithAttacker( Entity attacker, Entity weapon ) => this with { Attacker = Attacker, Weapon = weapon };

	/// <summary>
	/// Set the attacker
	/// </summary>
	public DamageInfo WithWeapon( Entity weapon ) => this with { Weapon = weapon };

	/// <summary>
	/// Sets the hit physics body
	/// </summary>
	public DamageInfo WithHitBody( PhysicsBody body ) => this with { Body = body };

	/// <summary>
	/// Sets the bone index
	/// </summary>
	public DamageInfo WithBone( int bone ) => this with { BoneIndex = bone };

	/// <summary>
	/// Sets the amount of damage
	/// </summary>
	/// <returns></returns>
	public DamageInfo WithDamage( float damage ) => this with { Damage = damage };

	/// <summary>
	/// Sets the position
	/// </summary>
	public DamageInfo WithPosition( Vector3 position ) => this with { Position = position };

	/// <summary>
	/// Sets the force
	/// </summary>
	public DamageInfo WithForce( Vector3 force ) => this with { Force = force };

	/// <summary>
	/// Fills in the PhysicsBody and Hitbox from the trace result
	/// </summary>
	public DamageInfo UsingTraceResult( TraceResult tr ) => this with { Body = tr.Body, BoneIndex = tr.Bone, Hitbox = tr.Hitbox };

	/// <summary>
	/// Includes one tag to this damage info.
	/// Can be accessed via <see cref="M:Sandbox.DamageInfo.HasTag(System.String)" /> and <see cref="P:Sandbox.DamageInfo.Tags" /> for a full list.
	/// </summary>
	public DamageInfo WithTag( string tag ) => this with { Tags = [.. Tags, tag] };

	/// <summary>
	/// Includes any number of tags to this damage info.
	/// Can be accessed via <see cref="M:Sandbox.DamageInfo.HasTag(System.String)" /> and <see cref="P:Sandbox.DamageInfo.Tags" /> for a full list.
	/// </summary>
	public DamageInfo WithTags( params IEnumerable<string> tags ) => this with { Tags = [.. Tags, .. tags] };

	/// <summary>
	/// Do we have a tag that matches this string?
	/// </summary>
	public bool HasTag( string tag ) => Tags.Has( tag );

	/// <summary>
	/// Returns true if this DamageInfo has ANY of the following tags
	/// </summary>
	public bool HasAnyTag( params IEnumerable<string> tags ) => Tags.HasAny( tags );

	/// <summary>
	/// Returns true if this DamageInfo has ALL of the following tags
	/// </summary>
	public bool HasAllTags( params IEnumerable<string> tags ) => Tags.HasAll( tags );
}
