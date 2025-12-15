namespace Sandbox;

/// <summary>
/// Extensions for <see cref="Input"/> that adds members from the old entity system.
/// </summary>
public static class InputExtensions
{
	extension( Input )
	{
		public static bool StopProcessing { get => Input.Suppressed; set => Input.Suppressed = value; }
	}
}
