using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sandbox;

namespace Legacy;

/// <summary>
/// Loads entities with <see cref="HammerEntityAttribute"/>.
/// </summary>
[Title( "Map Instance (Legacy)" )]
public class LegacyMapInstance : MapInstance
{
	public Dictionary<string, TypeDescription> HammerEntities { get; private set; }

	protected override Task OnLoad()
	{
		var hammerEntities = TypeLibrary.GetTypesWithAttribute<HammerEntityAttribute>().Select( t => t.Type );
		HammerEntities = hammerEntities.ToDictionary( t => t.GetAttribute<LibraryAttribute>().Name );

		return base.OnLoad();
	}

	protected override void OnCreateObject( GameObject go, MapLoader.ObjectEntry kv )
	{
		if ( !Sandbox.Game.InGame ) return;
		if ( !HammerEntities.TryGetValue( kv.TypeName, out var type ) ) return;

		// Game object is already created by the entity.
		go.Destroy();

		var entity = type.Create<Entity>();
		entity.Transform = kv.Transform;
		entity.GameObject.SetParent( GameObject );
	}

	protected override void OnStart()
	{
		base.OnStart();

		var collider = GetComponentInChildren<MapCollider>();
		new Entity( collider.GameObject );
	}
}
