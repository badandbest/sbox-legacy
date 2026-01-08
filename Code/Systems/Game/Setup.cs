using System.Linq;

namespace Sandbox;

internal sealed class Setup : MapInstance, ISceneStartup
{
	protected override void OnStart()
	{
		var collider = GetComponentInChildren<MapCollider>();

		OnCreateObject( collider.GameObject, new MapLoader.ObjectEntry() { TypeName = "entity" } );
	}

	protected override void OnCreateObject( GameObject go, MapLoader.ObjectEntry kv )
	{
		if ( !Game.InGame )
		{
			return;
		}

		var type = TypeLibrary.GetType( kv.TypeName );

		if ( type is null )
		{
			return;
		}

		go.Flags |= GameObjectFlags.Deserializing;
		go.Components.Create( type );
		go.Flags &= ~GameObjectFlags.Deserializing;
	}

	public void OnHostInitialize()
	{
		TypeLibrary.GetTypes<GameManager>().Single( x => !x.IsAbstract ).Create<GameManager>();
	}

	public void OnClientInitialize()
	{
		var cl = new ClientEntity();
		cl.GameObject.NetworkSpawn();
	}
}
