namespace Sandbox;

public sealed class LegacyMapInstance : MapInstance, ISceneStartup
{
	protected override void OnCreateObject( GameObject go, MapLoader.ObjectEntry kv )
	{
		using ( go.Push() )
		{
			TypeLibrary.Create<Entity>( kv.TypeName, false );
		}
	}

	protected override void OnStart()
	{
		using ( GetComponentInChildren<MapCollider>().GameObject.Push() )
		{
			new Entity();
		}
	}
}
