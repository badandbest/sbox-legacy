using System.Linq;

namespace Sandbox;

public sealed class Setup( Scene scene ) : GameObjectSystem( scene ), ISceneStartup
{
	public void OnHostInitialize()
	{
		var manager = TypeLibrary.GetTypes<GameManager>()
			.Single( t => !t.IsAbstract )
			.Create<GameManager>();

		manager.GameObject.NetworkSpawn( null );
	}

	public void OnClientInitialize()
	{
		var client = new ClientEntity();
		var manager = GameManager.Current;

		Listen( Stage.StartFixedUpdate, 0, () => manager.Simulate( client ), "Simulate" );
		Listen( Stage.StartUpdate, 0, () => manager.FrameSimulate( client ), "FrameSimulate" );
		Listen( Stage.StartUpdate, 1, manager.BuildInput, "BuildInput" );
		Listen( Stage.FinishUpdate, 0, () => GameEvent.Run( "camera.post" ), "camera.post" );
	}
}
