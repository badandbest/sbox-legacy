using System;
using System.ComponentModel;
using System.Linq;

namespace Sandbox;

public partial class Entity
{
	int IEntity.Id => NetworkIdent;
	bool IEntity.IsOwnedByLocalClient => GameObject.Network.IsOwner;

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
	public bool IsAuthority => !GameObject.IsProxy;

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

	protected T __sync_GetValue<T>( in WrappedPropertyGet<T> p )
	{
		return p.Value;
	}

	protected void __sync_SetValue<T>( in WrappedPropertySet<T> p )
	{
		if ( !GameObject.IsValid || GameObject.IsDeserializing )
		{
			p.Setter( p.Value );
			return;
		}

		// Rpc.Calling doesn't seem to work at the moment.
		var calling = Rpc.Caller != Connection.Local && Rpc.Caller != null;

		//
		// Send over network
		//
		if ( !calling && Networking.IsActive )
		{
			object value = p.Value;

			if ( value is Entity entity )
			{
				// s&box doesn't support custom network serialization
				// so I need to pass the component instead because otherwise
				// it will deserialize weirdly.
				value = entity.Components.Get<Handle>();
			}

			Broadcast( this, p.MemberIdent, value );
		}

		Rpc.PreCall();

		p.Setter( p.Value );
	}

	protected void __rpc_Wrapper( in WrappedMethod m, params object[] argumentList )
	{
		if ( !GameObject.IsValid || GameObject.IsDeserializing )
		{
			m.Resume();
			return;
		}

		for ( int i = 0; i < argumentList.Length; i++ )
		{
			if ( argumentList[i] is Entity entity )
			{
				argumentList[i] = entity.Components.Get<Handle>();
			}
		}

		// Rpc.Calling doesn't seem to work at the moment.
		var calling = Rpc.Caller != Connection.Local && Rpc.Caller != null;

		//
		// Send over network
		//
		if ( !calling && Networking.IsActive )
		{
			Broadcast( this, m.MethodIdentity, argumentList );
		}

		Rpc.PreCall();

		m.Resume();
	}

	[Rpc.Broadcast]
	private static void Broadcast( Handle handle, int memberIdentity, params object[] argumentList )
	{
		if ( !handle.IsValid() ) return;

		var calling = Rpc.Caller != Connection.Local && Rpc.Caller != null;

		if ( !calling )
		{
			return;
		}

		for ( int i = 0; i < argumentList.Length; i++ )
		{
			if ( argumentList[i] is Handle entity )
			{
				argumentList[i] = entity.Entity;
			}
		}

		switch ( TypeLibrary.GetMemberByIdent( memberIdentity ) )
		{
			case MethodDescription method: method.Invoke( handle.Entity, argumentList ); break;
			case PropertyDescription property: property.SetValue( handle.Entity, argumentList.Single() ); break;
		}
	}
}
