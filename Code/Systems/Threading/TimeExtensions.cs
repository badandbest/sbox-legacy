namespace Sandbox;

/// <summary>
/// Extensions for <see cref="Time"/> that adds members from the old entity system.
/// </summary>
public static class TimeExtensions
{
	extension( Time )
	{
		public static int Tick => (Time.Now / Game.ActiveScene.FixedDelta).CeilToInt();
	}
}
