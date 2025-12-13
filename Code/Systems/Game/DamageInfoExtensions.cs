using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Legacy;

/// <summary>
/// Extensions for <see cref="DamageInfo"/> that adds members from the old entity system.
/// </summary>
public static class DamageInfoExtensions
{
	extension( DamageInfo source )
	{
		/// <summary>
		/// Creates a new DamageInfo with the "bullet" tag
		/// </summary>
		public static DamageInfo FromBullet( Vector3 hitPosition, Vector3 hitForce, float damage ) => new()
		{
			Position = hitPosition,
			Damage = damage,
			//Force = hitForce,
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
			//Force = force,
			Damage = damage,
			Tags = ["explosion", "blast"]
		};

		/// <summary>
		/// The force of the damage - for moving physics etc. This would be the trajectory
		/// of the bullet multiplied by the speed and mass.
		/// </summary>
		public Vector3 Force
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		/// <summary>
		/// The physics body that was hit
		/// </summary>
		public PhysicsBody Body
		{
			get => source.Shape.Body;
			set => source.Shape = value.Shapes.First();
		}

		/// <summary>
		/// The bone index that the hitbox was attached to
		/// </summary>
		public int BoneIndex
		{
			get => source.Hitbox.Bone.Index;
			set => throw new NotImplementedException();
		}

		/// <summary>
		/// Set the attacker
		/// </summary>
		public DamageInfo WithAttacker( Entity attacker )
		{
			source.Attacker = attacker;

			return source;
		}

		/// <summary>
		/// Set the attacker
		/// </summary>
		public DamageInfo WithWeapon( Entity weapon )
		{
			source.Weapon = weapon;

			return source;
		}

		/// <summary>
		/// Sets the hit physics body
		/// </summary>
		public DamageInfo WithHitBody( PhysicsBody body )
		{
			source.Body = body;

			return source;
		}

		/// <summary>
		/// Sets the bone index
		/// </summary>
		public DamageInfo WithBone( int bone )
		{
			source.BoneIndex = bone;

			return source;
		}

		/// <summary>
		/// Sets the amount of damage
		/// </summary>
		/// <returns></returns>
		public DamageInfo WithDamage( float damage )
		{
			source.Damage = damage;

			return source;
		}

		/// <summary>
		/// Sets the position
		/// </summary>
		public DamageInfo WithPosition( Vector3 position )
		{
			source.Position = position;

			return source;
		}

		/// <summary>
		/// Sets the force
		/// </summary>
		public DamageInfo WithForce( Vector3 force )
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Fills in the PhysicsBody and Hitbox from the trace result
		/// </summary>
		public DamageInfo UsingTraceResult( TraceResult tr )
		{
			source.Shape = tr.Shape;
			source.Hitbox = tr.Hitbox;

			return source;
		}

		/// <summary>
		/// Includes one tag to this damage info.
		/// Can be accessed via <see cref="M:Sandbox.DamageInfo.HasTag(System.String)" /> and <see cref="P:Sandbox.DamageInfo.Tags" /> for a full list.
		/// </summary>
		public DamageInfo WithTag( string tag )
		{
			source.Tags.Add( tag );

			return source;
		}

		/// <summary>
		/// Includes any number of tags to this damage info.
		/// Can be accessed via <see cref="M:Sandbox.DamageInfo.HasTag(System.String)" /> and <see cref="P:Sandbox.DamageInfo.Tags" /> for a full list.
		/// </summary>
		public DamageInfo WithTags( params IEnumerable<string> tags )
		{
			foreach ( var tag in tags )
			{
				source.Tags.Add( tag );
			}

			return source;
		}

		/// <summary>
		/// Do we have a tag that matches this string?
		/// </summary>
		public bool HasTag( string tag ) => source.Tags.Has( tag );

		/// <summary>
		/// returns true if this damageinfo has any of the following tags
		/// </summary>
		public bool HasAnyTag( params IEnumerable<string> tags ) => source.Tags.HasAny( tags );

		/// <summary>
		/// Returns true if this DamageInfo has ALL of the following tags
		/// </summary>
		public bool HasAllTags( params IEnumerable<string> tags ) => source.Tags.HasAll( tags );
	}
}
