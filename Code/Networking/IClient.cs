using System;
using Legacy.Entities.Components.Interfaces;
using Sandbox;

namespace Legacy;

public interface IClient : IEntity
{
	/// <summary>
	/// The entity this client is controlling, i.e. their physical representation in the game world.
	/// </summary>
	IEntity Pawn { get; set; }

	/// <summary>
	/// The SteamId of this client. 
	/// </summary>
	long SteamId { get; }

	/// <summary>
	/// The Steam name of this client.
	/// </summary>
	string Name { get; }

	/// <summary>
	/// Whether the client is your friend or not.
	/// </summary>
	bool IsFriend { get; }

	/// <summary>
	/// If this client is a bot.
	/// </summary>
	bool IsBot { get; }

	/// <summary>
	/// Whether the client is using Virtual Reality or not.
	/// </summary>
	bool IsUsingVr { get; }

	/// <summary>
	/// Returns true if this client is the listen server host.
	/// </summary>
	bool IsListenServerHost { get; }

	/// <summary>
	/// The time it takes for a network message to get to this client.
	/// </summary>
	int Ping { get; }

	/// <summary>
	/// The client's packet loss as a percentage (0-100.)
	/// </summary>
	int PacketLoss { get; }

	/// <summary>
	/// Information regarding the client's voice activity.
	/// </summary>
	IVoice Voice { get; }


	/// <summary>
	/// Kick this client from the server.
	/// </summary>
	void Kick();

	#region Console

	/// <summary>
	/// Gets the convar value from a ClientData convar.
	/// </summary>
	string GetClientData( string key, string defaultValue = null );

	/// <summary>
	/// Gets the convar value from a ClientData convar.
	/// </summary>
	T GetClientData<T>( string key, T defaultValue = default );

	/// <summary>
	/// Send a console command to this client to be run.
	/// </summary>
	void SendCommandToClient( string command );

	/// <summary>
	/// Sets an arbitrary client info value at given key. The value will be networked to clients, so it should be a network-able type.
	/// </summary>
	/// <param name="key">The key for our value to look it up later by.</param>
	/// <param name="value">The value. It should be a network-able type, typically a number.</param>
	void SetValue( string key, object value );

	/// <summary>
	/// Retrieves the arbitrary client info value at given key, set previously by <see cref="SetValue"/>.
	/// </summary>
	/// <param name="key">The key to look up the info at.</param>
	/// <param name="defaultValue">Value to fall back to in case there is no value stored at given key.</param>
	T GetValue<T>( string key, T defaultValue = default );

	/// <summary>
	/// Convenience function, sets an integer info at given key via <see cref="SetValue"/>
	/// </summary>
	/// <param name="key">Key for the value.</param>
	/// <param name="value">Value to set.</param>
	void SetInt( string key, int value ) => SetValue( key, value );

	/// <summary>
	/// Convenience function, returns previously set (via <see cref="SetInt"/>) integer value at given key via <see cref="GetValue{T}"/>.
	/// </summary>
	/// <param name="key"></param>
	/// <param name="defaultValue"></param>
	/// <returns></returns>
	int GetInt( string key, int defaultValue = 0 ) => GetValue( key, defaultValue );

	/// <summary>
	/// Convenience function, adds given number at given key via <see cref="SetInt"/>
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	void AddInt( string key, int value = 1 ) => SetInt( key, GetInt( key ) + value );

	#endregion

	/// <summary>
	/// Information regarding a client's voice.
	/// </summary>
	interface IVoice
	{
		/// <summary>
		/// Last voice level heard.
		/// </summary>
		float CurrentLevel { get; set; }

		/// <summary>
		/// Whether the client's voice chat is 2 channel (stereo) or 1 channel (mono).
		/// </summary>
		bool WantsStereo { get; set; }

		/// <summary>
		/// When was the last time client's voice was heard.
		/// </summary>
		RealTimeSince LastHeard { get; set; }
	}

	/// <summary>
	/// Allows manipulation of a client's Potentially Visible Set, i.e what entities are potentially visible and therefore networked to this specific client.
	/// </summary>
	class PvsConfig
	{
		/// <summary>
		/// Add an entity to the player's PVS. The player will be able to see everything that this entity can see.
		/// </summary>
		/// <param name="ent"></param>
		public void Add( IEntity ent ) => throw new NotImplementedException();

		/// <summary>
		/// Remove the entity from this player's PVS that was previously added via <see cref="Add"/>
		/// </summary>
		/// <param name="ent"></param>
		public void Remove( IEntity ent ) => throw new NotImplementedException();

		/// <summary>
		/// Remove all specialization.
		/// </summary>
		public void Clear() => throw new NotImplementedException();
	}
}
