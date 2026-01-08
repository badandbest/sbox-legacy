using Sandbox.Diagnostics;
using Sandbox.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Sandbox;

/// <summary>
/// Extensions for <see cref="Game"/> that adds members from the old entity system.
/// </summary>
public static partial class GameExtensions
{
	private static bool IsServer = true;

	extension( Game )
	{
		/// <summary>
		/// Returns a list of all clients on the server.
		/// </summary>
		public static IEnumerable<IClient> Clients => Entity.All.OfType<IClient>();

		/// <summary>
		/// The local client. This is you if you're connecting to the server.
		/// </summary>
		public static IClient LocalClient => Game.Clients.Single( cl => cl.IsOwnedByLocalClient );

		/// <summary>
		/// The local client's pawn. This is probably a player, or a vehicle, or a melon.
		/// </summary>
		public static Entity LocalPawn => Game.LocalClient.Pawn as Entity;

		/// <summary>
		/// Returns true only when current code is running on the server.
		/// </summary>
		public static bool IsServer => IsServer;

		/// <summary>
		/// Returns true only when current code is running on the client.
		/// </summary>
		public static bool IsClient => !Application.IsHeadless;

		/// <summary>
		/// Holds information about the current user's preferences.
		/// </summary>
		public static Preferences Preferences => new();

		/// <summary>
		/// Don't NetworkSpawn entities during this scope.
		/// </summary>
		internal static IDisposable ClientScope()
		{
			var old = IsServer;
			IsServer = false;

			return DisposeAction.Create( () => IsServer = old );
		}

		/// <summary>
		/// Throws an exception when called from server or menu.
		/// </summary>
		public static void AssertClient( [CallerMemberName] string memberName = default ) => Assert.True( Game.IsClient, memberName );

		/// <summary>
		/// Throws an exception when called from client or menu.
		/// </summary>
		public static void AssertServer( [CallerMemberName] string memberName = default ) => Assert.True( Game.IsServer, memberName );

		/// <summary>
		/// Throws an exception when called from server.
		/// </summary>
		public static void AssertNotServer( [CallerMemberName] string memberName = default ) => Assert.False( Game.IsServer, memberName );

		/// <summary>
		/// Throws an exception when called from client or server.
		/// </summary>
		public static void AssertMenu( [CallerMemberName] string memberName = default ) => Assert.True( Game.IsClient, memberName );

		/// <summary>
		/// Throws an exception when called from server.
		/// </summary>
		public static void AssertClientOrMenu( [CallerMemberName] string memberName = default ) => Assert.True( Game.IsClient, memberName );
	}

	// Hack to preserve Game.Preferences path.
	public readonly struct Preferences
	{
		/// <summary>
		/// The user's preferred depth of field, as set in the options, clamped between 60 and 120
		/// </summary>
		public float FieldOfView => Sandbox.Preferences.FieldOfView;

		/// <summary>
		/// The user's preferred VOIP volume, as set in the options, clamped between 0 and 1
		/// </summary>
		public float VoipVolume => Sandbox.Preferences.VoipVolume;
	}
}
