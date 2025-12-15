namespace Sandbox;

public partial class Entity
{
	/// <summary>
	/// The entity which attacked this one last. Server only.
	/// </summary>
	[Hide]
	public Entity LastAttacker { get; set; }

	/// <summary>
	/// The weapon <see cref="LastAttacker" /> was carrying, if any. Server only.
	/// </summary>
	[Hide]
	public Entity LastAttackerWeapon { get; set; }

	/// <summary>
	/// Entity's health.
	/// </summary>
	[Category( "Life" )]
	public float Health { get; set; }

	/// <summary>
	/// Entity's life state. Can be used to determine if certain events such as <see cref="OnKilled" /> need firing.
	/// </summary>
	[Category( "Life" )]
	public LifeState LifeState { get; set; }

	/// <summary>
	/// Called when the entity receives a damage event. This is where you want to subtract health, etc.
	/// </summary>
	/// <param name="info"></param>
	public virtual void TakeDamage( DamageInfo info )
	{
		LastAttacker = info.Attacker;
		LastAttackerWeapon = info.Weapon;

		if ( Game.IsServer && Health > 0f && LifeState == LifeState.Alive )
		{
			Health -= info.Damage;
			if ( Health <= 0f )
			{
				Health = 0f;
				OnKilled();
			}
		}
	}

	/// <summary>
	/// Called (from <see cref="M:Sandbox.Entity.TakeDamage(Sandbox.DamageInfo)" /> by default unless overridden) when there's no health left.
	/// </summary>
	public virtual void OnKilled()
	{
		if ( LifeState == LifeState.Alive )
		{
			LifeState = LifeState.Dead;
			Delete();
		}
	}
}

public enum LifeState
{
	/// <summary>
	/// Alive as normal
	/// </summary>
	Alive,

	/// <summary>
	/// Playing a death animation
	/// </summary>
	Dying,

	/// <summary>
	/// Dead, lying still
	/// </summary>
	Dead,

	/// <summary>
	/// Can respawn, usually waiting for some client action to respawn
	/// </summary>
	Respawnable,

	/// <summary>
	/// Is in the process of respawning
	/// </summary>
	Respawning
}
