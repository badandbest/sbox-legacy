using Sandbox;
using Sandbox.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Legacy;

public static class Game
{
	/// <summary>
	/// Returns a list of all clients on the server.
	/// </summary>
	public static IEnumerable<IClient> Clients => Entity.All.OfType<ClientEntity>();

	/// <summary>
	/// The local client. This is you if you're connecting to the server.
	/// </summary>
	public static IClient LocalClient => Clients.FirstOrDefault( cl => cl.SteamId == Steam.SteamId );

	/// <summary>
	/// The local client's pawn. This is probably a player, or a vehicle, or a melon.
	/// </summary>
	public static Entity LocalPawn => LocalClient?.Pawn as Entity;

	/// <summary>
	/// Returns true only when current code is running on the server.
	/// </summary>
	public static bool IsServer => Networking.IsHost;

	/// <summary>
	/// Holds information about the current user's preferences.
	/// </summary>
	public static class Preferences
	{
		/// <summary>
		/// The user's preferred depth of field, as set in the options, clamped between 60 and 120
		/// </summary>
		public static float FieldOfView => Sandbox.Preferences.FieldOfView;

		/// <summary>
		/// The user's preferred VOIP volume, as set in the options, clamped between 0 and 1
		/// </summary>
		public static float VoipVolume => Sandbox.Preferences.VoipVolume;
	}
}
