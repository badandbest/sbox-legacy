namespace Legacy;

public partial class Entity
{
	/// <summary>
	/// Play a sound from this entity, updating sounds' position as the entity moves.
	/// </summary>
	/// <param name="soundName">The sound to play.</param>
	/// <returns>The sound controller.</returns>
	public Sound PlaySound( string soundName )
	{
		return Sound.FromEntity( soundName, this );
	}

	/// <summary>
	/// Play a sound from this entity at given attachment point, updating sounds' position as the entity and its attachments move.
	/// </summary>
	/// <param name="soundName">The sound to play.</param>
	/// <param name="attachment">Name of the attachment to play the sound at.</param>
	/// <returns>The sound controller.</returns>
	public virtual Sound PlaySound( string soundName, string attachment )
	{
		return Sound.FromEntity( soundName, this, attachment );
	}
}
