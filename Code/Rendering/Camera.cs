using Sandbox;

namespace Legacy;

/// <summary>
/// Global accessor for the current game's camera.
/// </summary>
public static class Camera
{
	/// <summary>
	/// The current game's camera, which is used to render the main view
	/// </summary>
	public static CameraComponent Main => Sandbox.Game.ActiveScene.Camera;

	/// <summary>
	/// The current camera's position. If current is null, falls back to main camera.
	/// </summary>
	public static Vector3 Position { get => Main.WorldPosition; set => Main.WorldPosition = value; }

	/// <summary>
	/// The camera's rotation. If current is null, falls back to main camera.
	/// </summary>
	public static Rotation Rotation { get => Main.WorldRotation; set => Main.WorldRotation = value; }

	/// <summary>
	/// The current camera's horizontal field of view in degrees. If the current is null, falls back to main camera.
	/// </summary>
	public static float FieldOfView { get => Main.FieldOfView; set => Main.FieldOfView = value; }

	/// <summary>
	/// The current camera's far plane. If the current is null, falls back to main camera.
	/// </summary>
	public static float ZFar { get => Main.ZFar; set => Main.ZFar = value; }

	/// <summary>
	/// The current camera's far plane. If current is null, falls back to main camera.
	/// </summary>
	public static float ZNear { get => Main.ZNear; set => Main.ZNear = value; }

	/// <summary>
	/// The current camera's first person viewer. If set then the entity that is viewing
	/// will be invisible, and entities marked as viewmodels will be visible.
	/// </summary>
	public static IEntity FirstPersonViewer { get; set; }

	/// <summary>
	/// The current camera's size (in pixels)
	/// </summary>
	public static Vector2 Size => Screen.Size;

	/// <summary>
	/// The eye that the current camera is targeting
	/// </summary>
	public static StereoTargetEye TargetEye { get => Main.TargetEye; set => Main.TargetEye = value; }

	/// <summary>
	/// Set attributes for rendering the viewmodel's camera.
	/// </summary>
	public static void SetViewModelCamera( this CameraComponent camera, float viewModelFieldOfView,
		float viewModelZNear = 1f, float viewModelZFar = 500f )
	{
	}
}
