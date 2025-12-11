using System.Collections.Generic;
using System.Linq;
using Sandbox;

namespace Legacy;

/// <summary>
/// Loads entities with <see cref="HammerEntityAttribute"/>.
/// </summary>
public class LegacyMapLoader : SceneMapLoader
{
	public Dictionary<string, TypeDescription> HammerEntities { get; private set; }

	public LegacyMapLoader( SceneWorld world, PhysicsWorld physics ) : base( world, physics )
	{
		var hammerEntities = TypeLibrary.GetTypesWithAttribute<HammerEntityAttribute>().Select( t => t.Type );
		HammerEntities = hammerEntities.ToDictionary( t => t.GetAttribute<LibraryAttribute>().Name );
	}

	protected override void CreateObject( ObjectEntry kv )
	{
		if ( HammerEntities.TryGetValue( kv.TypeName, out var type ) )
		{
			type.Create<Entity>();
		}
		else
		{
			CreateModel( kv );
		}
	}
}
