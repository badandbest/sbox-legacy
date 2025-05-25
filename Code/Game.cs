using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Sandbox.Utility;

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
}
