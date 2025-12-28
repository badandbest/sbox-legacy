using System;
using System.Text.Json.Serialization;

namespace Sandbox;

[Library( "player" ), Category( "Clients" ), Title( "Client" ), Icon( "account_circle" )]
internal sealed class ClientEntity : Entity, IClient, IEquatable<Connection>
{
	private Connection connection => GameObject.Network.Owner ?? Connection.Local;

	public IEntity Pawn { get; set; }

	public long SteamId => connection.SteamId;

	public string Name => connection.DisplayName;

	public bool IsFriend => new Friend( SteamId ).IsFriend;

	public int Ping => (int)connection.Ping;

	public bool IsListenServerHost => connection.IsHost;

	public void Kick() => connection.Kick( string.Empty );

	#region Unimplemented

	public bool IsBot => throw new NotImplementedException();
	public bool IsUsingVr => throw new NotImplementedException();
	public int PacketLoss => throw new NotImplementedException();
	public IClient.IVoice Voice => throw new NotImplementedException();
	public string GetClientData( string key, string defaultValue = null ) => connection.GetUserData( key );
	public T GetClientData<T>( string key, T defaultValue = default ) => throw new NotImplementedException();
	public void SendCommandToClient( string command ) => throw new NotImplementedException();
	public void SetValue( string key, object value ) => throw new NotImplementedException();
	public T GetValue<T>( string key, T defaultValue = default ) => throw new NotImplementedException();

	#endregion

	public override void Spawn()
	{
		GameManager.Current.ClientJoined( this );
	}

	protected internal override void OnDestroy()
	{
		GameManager.Current.ClientDisconnect( this );
	}

	public bool Equals( Connection other ) => connection == other;

	public override string ToString() => $"{SteamId}/{Name}";
}
