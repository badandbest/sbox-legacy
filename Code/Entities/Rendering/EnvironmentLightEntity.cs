namespace Sandbox;

/// <summary>
/// An environment light entity. This acts as the sun.
/// </summary>
[Library( "light_environment" ), HammerEntity]
[EditorModel( "models/editor/sun", "yellow", "rgb(255, 64, 64)" )]
[Title( "Environment Light" ), Category( "Lighting" ), Icon( "wb_sunny" )]
public class EnvironmentLightEntity : Entity
{
	public EnvironmentLightEntity()
	{
		GameObject.AddComponent<DirectionalLight>();
	}
}
