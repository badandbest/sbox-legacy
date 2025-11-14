using Sandbox;

namespace Legacy;

public partial class Entity
{
	/// <summary>
	/// The game object this entity is bindined to.
	/// </summary>
	[Hide]
	internal GameObject GameObject { get; }

	/// <summary>
	/// Binds an entity to a game object.
	/// </summary>
	internal Entity( GameObject go )
	{
		GameObject = go;
		GameObject.Name = GetType().Name;
		GameObject.Flags |= GameObjectFlags.NotSaved;

		var wrapper = GameObject.AddComponent<EntityBinder>();
		wrapper.Entity = this;

		Components = new EntityComponentSystem( this );
		All.Add( this );
	}

	public static implicit operator GameObject( Entity entity ) => entity.GameObject;

	public static implicit operator Entity( GameObject gameObject ) => gameObject?.GetComponent<EntityBinder>()?.Entity;
}

[Title( "Entity" )]
file sealed class EntityBinder : Component
{
	[Property, InlineEditor]
	public Entity Entity { get; set; }

	protected override void OnStart() => Entity.Spawn();
	protected override void OnDestroy() => Entity.Delete();
}
