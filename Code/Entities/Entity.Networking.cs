using System;
using System.ComponentModel;

namespace Sandbox;

public partial class Entity
{
	int IEntity.Id => NetworkIdent;
	bool IEntity.IsOwnedByLocalClient => !IsProxy;

	/// <summary>
	/// Defaults to <see langword="true" />, this allows you to turn off prediction for this entity. If you set this
	/// to <see langword="false" /> then the entity won't be predicted even if it's eligible (has local client owner).
	/// </summary>
	[Category( "Networking" )]
	public bool Predictable { get; set; } = true;

	/// <summary>
	/// When should this entity and its properties be networked to all clients?
	/// </summary>
	[Category( "Networking" )]
	public TransmitType Transmit { get; set; } = TransmitType.Default;

	/// <summary>
	/// The client that owns this entity. Usually as a result of being the client's Pawn.
	/// Also could be because the client's pawn owns this entity,
	/// </summary>
	[Browsable( false )]
	public IClient Client => this as ClientEntity ?? Owner?.Client ?? Parent?.Client;

	[Property, Hide, Category( "Meta" )] public int NetworkIdent => GameObject.Id.GetHashCode();

	/// <summary>
	/// Returns true if we have authority over this entity. This means we're either serverside,
	/// or we're a clientside entity, or we're a serverside entity being predicted on the client.
	/// </summary>
	[Hide]
	public bool IsAuthority => Game.IsServer || IsClientOnly;

	[Hide] public bool IsClientOnly => true;

	[Hide] public bool IsDormant => throw new NotImplementedException();

	public bool IsFromMap => GameObject.Tags.Has( "world" );

	/// <summary>
	/// Enable lag compensation. This will rewind all eligible entities to the positions they
	/// were when the client sent the command that is being simulated. When used in a `using` block,
	/// lag compensation will be automatically finished when it is disposed.
	/// </summary>
	public static IDisposable LagCompensation()
	{
		return null;
	}
}
