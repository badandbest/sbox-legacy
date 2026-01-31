namespace Sandbox;

internal static class PhysicsLockExtensions
{
	static readonly PhysicsLock _rotation = new() { Pitch = true, Yaw = true, Roll = true };

	extension( PhysicsLock )
	{
		/// <summary>
		/// Lock rotation axes.
		/// </summary>
		internal static PhysicsLock Rotation => _rotation;
	}
}
