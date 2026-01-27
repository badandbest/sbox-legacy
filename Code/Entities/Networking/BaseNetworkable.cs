using System;

namespace Sandbox;

/// <summary>
/// A network capable object.<br />
/// Properties marked with <see cref="NetAttribute">[Net]</see> attribute will be automagically networked to clients when possible.
/// </summary>
public abstract class BaseNetworkable : Component
{
	public BaseNetworkable()
	{
		ClassName = TypeLibrary.GetType( GetType() ).ClassName;
	}

	/// <summary>
	/// The "class name" used internally for identifying networkables.
	/// </summary>
	[Hide]
	public string ClassName { get; protected set; }

	/// <summary>
	/// Should return an ID of this networkable that is common across the network.
	/// </summary>
	[Hide]
	public virtual int NetworkIdent => 0;

	/// <summary>
	/// Serialize this class to the network. You shouldn't need to call this manually unless you're
	/// implementing INetworkSerializer and want to force a write. In other situations we can detect
	/// when things change and update them manually.
	/// </summary>
	public void WriteNetworkData() => throw new NotSupportedException();
}
