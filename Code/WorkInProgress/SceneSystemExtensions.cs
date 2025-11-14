using Sandbox;

namespace Legacy;

/// <summary>
/// Legacy compatability extensions for scene system.
/// </summary>
public static class SceneSystemExtensions
{
	extension( Input )
	{
		public static bool StopProcessing { get => Input.Suppressed; set => Input.Suppressed = value; }
	}

	extension( Time )
	{
		public static int Tick => (Time.Now / Sandbox.Game.ActiveScene.FixedDelta).CeilToInt();
	}
}
