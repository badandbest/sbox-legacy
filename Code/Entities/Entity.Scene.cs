using System.Collections.Generic;
using System.Linq;

namespace Sandbox;

public partial class Entity
{
	/// <summary>
	/// A list of all active entities.
	/// </summary>
	public static IReadOnlyList<Entity> All => [.. Game.ActiveScene.GetAll<EntityBinder>().Select( b => b.Entity )];

	/// <summary>
	/// The game object this entity is bindined to.
	/// </summary>
	[Hide]
	internal GameObject GameObject { get; }

	/// <summary>
	/// Create the entity.
	/// </summary>
	public Entity()
	{
		Components = new EntityComponentSystem( this );

		GameObject = GameObject.Scoped ?? new( GetType().Name );
		GameObject.GetOrAddComponent<EntityBinder>().Entity = this;
	}

	public bool IsValid => GameObject.IsValid();

	/// <summary>
	/// Delete this entity. You shouldn't access it anymore.
	/// </summary>
	public void Delete() => GameObject.Destroy();

	public static implicit operator GameObject( Entity entity ) => entity.GameObject;

	public static implicit operator Entity( GameObject gameObject ) => gameObject?.GetComponent<EntityBinder>()?.Entity;
}

[Title( "Entity" ), Tint( EditorTint.White )]
file sealed class EntityBinder : Component
{
	[Property, InlineEditor]
	public Entity Entity { get; set; }

	protected override void OnStart() => Entity.Spawn();
	protected override void OnDestroy() => Entity.OnDestroy();
}
