using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sandbox;

/// <summary>
/// A wrapper to define which clients to send network things to. This aims to make code more readable by having the target argument in generated functions be more obvious and visible.
/// </summary>
public struct To : IEnumerable<IClient>
{
	internal IEnumerable<IClient> Targets;

	/// <summary>
	/// Send to a single client (the client owner of this pawn.)
	/// </summary>
	public static To Single( Entity pawn ) => Single( pawn.Client );

	/// <summary>
	/// Send to single client.
	/// </summary>
	public static To Single( IClient client ) => new() { Targets = [client] };

	/// <summary>
	/// Send to multiple clients.
	/// </summary>
	public static To Multiple( IEnumerable<IClient> clients ) => new() { Targets = clients };

	/// <summary>
	/// The same as <c>To.Multiple( Client.All )</c>.
	/// </summary>
	public static To Everyone => Multiple( Game.Clients );

	/// <summary>
	/// Is this client a target recipient?
	/// </summary>
	public bool IsRecipient( IClient client ) => Targets.Contains( client );

	public IEnumerator<IClient> GetEnumerator() => Targets.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	/// Send console command to clients referenced by this class.
	/// </summary>
	public void SendCommand( string command ) => throw new NotImplementedException();
}
