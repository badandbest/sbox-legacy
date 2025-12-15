using System;

namespace Sandbox;

/// <summary>
/// A struct to help you set up your citizen based animations
/// </summary>
public struct CitizenAnimationHelper( AnimatedEntity owner )
{
	/// <summary>
	/// Have the player look at this point in the world
	/// </summary>
	public void WithLookAt( Vector3 look, float eyesWeight = 1.0f, float headWeight = 1.0f, float bodyWeight = 1.0f )
	{
		var aimRay = owner.AimRay;

		owner.SetAnimLookAt( "aim_eyes", aimRay.Position, look );
		owner.SetAnimLookAt( "aim_head", aimRay.Position, look );
		owner.SetAnimLookAt( "aim_body", aimRay.Position, look );

		owner.SetAnimParameter( "aim_eyes_weight", eyesWeight );
		owner.SetAnimParameter( "aim_head_weight", headWeight );
		owner.SetAnimParameter( "aim_body_weight", bodyWeight );
	}

	public void WithVelocity( Vector3 Velocity )
	{
		var dir = Velocity;
		var forward = owner.Rotation.Forward.Dot( dir );
		var sideward = owner.Rotation.Right.Dot( dir );

		var angle = MathF.Atan2( sideward, forward ).RadianToDegree().NormalizeDegrees();

		owner.SetAnimParameter( "move_direction", angle );
		owner.SetAnimParameter( "move_speed", Velocity.Length );
		owner.SetAnimParameter( "move_groundspeed", Velocity.WithZ( 0 ).Length );
		owner.SetAnimParameter( "move_y", sideward );
		owner.SetAnimParameter( "move_x", forward );
		owner.SetAnimParameter( "move_z", Velocity.z );
	}

	public void WithWishVelocity( Vector3 Velocity )
	{
		var dir = Velocity;
		var forward = owner.Rotation.Forward.Dot( dir );
		var sideward = owner.Rotation.Right.Dot( dir );

		var angle = MathF.Atan2( sideward, forward ).RadianToDegree().NormalizeDegrees();

		owner.SetAnimParameter( "wish_direction", angle );
		owner.SetAnimParameter( "wish_speed", Velocity.Length );
		owner.SetAnimParameter( "wish_groundspeed", Velocity.WithZ( 0 ).Length );
		owner.SetAnimParameter( "wish_y", sideward );
		owner.SetAnimParameter( "wish_x", forward );
		owner.SetAnimParameter( "wish_z", Velocity.z );
	}

	public Rotation AimAngle
	{
		set
		{
			value = owner.Rotation.Inverse * value;
			var ang = value.Angles();

			owner.SetAnimParameter( "aim_body_pitch", ang.pitch );
			owner.SetAnimParameter( "aim_body_yaw", ang.yaw );
		}
	}

	public float AimEyesWeight
	{
		get => owner.GetAnimParameterFloat( "aim_eyes_weight" );
		set => owner.SetAnimParameter( "aim_eyes_weight", value );
	}

	public float AimHeadWeight
	{
		get => owner.GetAnimParameterFloat( "aim_head_weight" );
		set => owner.SetAnimParameter( "aim_head_weight", value );
	}

	public float AimBodyWeight
	{
		get => owner.GetAnimParameterFloat( "aim_body_weight" );
		set => owner.SetAnimParameter( "aim_headaim_body_weight_weight", value );
	}


	public float FootShuffle
	{
		get => owner.GetAnimParameterFloat( "move_shuffle" );
		set => owner.SetAnimParameter( "move_shuffle", value );
	}

	public float DuckLevel
	{
		get => owner.GetAnimParameterFloat( "duck" );
		set => owner.SetAnimParameter( "duck", value );
	}

	public float VoiceLevel
	{
		get => owner.GetAnimParameterFloat( "voice" );
		set => owner.SetAnimParameter( "voice", value );
	}

	public bool IsSitting
	{
		get => owner.GetAnimParameterBool( "b_sit" );
		set => owner.SetAnimParameter( "b_sit", value );
	}

	public bool IsGrounded
	{
		get => owner.GetAnimParameterBool( "b_grounded" );
		set => owner.SetAnimParameter( "b_grounded", value );
	}

	public bool IsSwimming
	{
		get => owner.GetAnimParameterBool( "b_swim" );
		set => owner.SetAnimParameter( "b_swim", value );
	}

	public bool IsClimbing
	{
		get => owner.GetAnimParameterBool( "b_climbing" );
		set => owner.SetAnimParameter( "b_climbing", value );
	}

	public bool IsNoclipping
	{
		get => owner.GetAnimParameterBool( "b_noclip" );
		set => owner.SetAnimParameter( "b_noclip", value );
	}

	public bool IsWeaponLowered
	{
		get => owner.GetAnimParameterBool( "b_weapon_lower" );
		set => owner.SetAnimParameter( "b_weapon_lower", value );
	}

	public enum HoldTypes
	{
		None,
		Pistol,
		Rifle,
		Shotgun,
		HoldItem,
		Punch,
		Swing,
		RPG
	}

	public HoldTypes HoldType
	{
		get => (HoldTypes)owner.GetAnimParameterInt( "holdtype" );
		set => owner.SetAnimParameter( "holdtype", (int)value );
	}

	public enum Hand
	{
		Both,
		Right,
		Left
	}

	public Hand Handedness
	{
		get => (Hand)owner.GetAnimParameterInt( "holdtype_handedness" );
		set => owner.SetAnimParameter( "holdtype_handedness", (int)value );
	}

	public void TriggerJump()
	{
		owner.SetAnimParameter( "b_jump", true );
	}

	public void TriggerDeploy()
	{
		owner.SetAnimParameter( "b_deploy", true );
	}

	public enum MoveStyles
	{
		Auto,
		Walk,
		Run
	}

	/// <summary>
	/// We can force the model to walk or run, or let it decide based on the speed.
	/// </summary>
	public MoveStyles MoveStyle
	{
		get => (MoveStyles)owner.GetAnimParameterInt( "move_style" );
		set => owner.SetAnimParameter( "move_style", (int)value );
	}
}
