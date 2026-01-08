using System;

namespace Sandbox;

[Library( "player" ), Category( "Clients" ), Title( "Client" ), Icon( "account_circle" )]
internal sealed class ClientEntity() : Entity, IClient
{
	public IEntity Pawn { get; set; }

	public long SteamId => Network.Owner.SteamId;

	public string Name => Network.Owner.DisplayName;

	public bool IsFriend => new Friend( SteamId ).IsFriend;

	public int Ping => (int)Network.Owner.Ping;

	public bool IsListenServerHost => Network.Owner.IsHost;

	public void Kick() => Network.Owner.Kick( string.Empty );

	public override void Spawn()
	{
		GameManager.Current.ClientJoined( this );
	}

	protected override void OnDestroy()
	{
		//GameManager.Current.ClientDisconnect( this );
	}

	#region Unimplemented

	public bool IsBot => throw new NotImplementedException();
	public bool IsUsingVr => throw new NotImplementedException();
	public int PacketLoss => throw new NotImplementedException();
	public IClient.IVoice Voice => throw new NotImplementedException();
	public string GetClientData( string key, string defaultValue = null ) => Network.Owner.GetUserData( key );
	public T GetClientData<T>( string key, T defaultValue = default ) => throw new NotImplementedException();
	public void SendCommandToClient( string command ) => throw new NotImplementedException();
	public void SetValue( string key, object value ) => throw new NotImplementedException();
	public T GetValue<T>( string key, T defaultValue = default ) => throw new NotImplementedException();

	#endregion

	public override string ToString() => $"{SteamId}/{Name}";
}
