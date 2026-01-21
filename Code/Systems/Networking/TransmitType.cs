namespace Sandbox;

/// <summary>
/// Entity transmit mode, dictates when entity will be networked to clients.
/// </summary>
public enum TransmitType
{
	/// <summary>
	/// Engine will decide when to transmit the entity.
	/// </summary>
	Default,
	/// <summary>
	/// Transmit always, no matter what.
	/// </summary>
	Always,
	/// <summary>
	/// Do not transmit, ever.
	/// </summary>
	Never,
	/// <summary>
	/// Entity will be networked if it's in a client (or its Pawn's) PVS
	/// </summary>
	Pvs,
	/// <summary>
	/// Inherit the transmit type from parent or owner.
	/// </summary>
	Owner
}
