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
	internal readonly GameObject GameObject = GameObject.Scoped;

	/// <summary>
	/// Components of this entity.
	/// </summary>
	public ComponentList Components => GameObject.Components;

	/// <summary>
	/// Create the entity.
	/// </summary>
	public Entity()
	{
		if ( GameObject is null )
		{
			return;
		}

		GameObject = new( GetType().Name );
		GameObject.AddComponent<EntityBinder>().Entity = this;

		GameObject.NetworkSpawn();
	}

	public bool IsValid => GameObject.IsValid();

	/// <summary>
	/// Delete this entity. You shouldn't access it anymore.
	/// </summary>
	public void Delete() => GameObject.Destroy();

	public static implicit operator GameObject( Entity entity ) => entity.GameObject;

	public static implicit operator Entity( GameObject gameObject ) => gameObject?.GetComponent<EntityBinder>()?.Entity;
}

/// <summary>
/// A component that forwards actions to an entity.
/// </summary>
[Title( "Entity" ), Tint( EditorTint.White )]
internal sealed class EntityBinder : Component, Component.INetworkSnapshot
{
	[Property, InlineEditor]
	public Entity Entity { get; set; }

	public void ReadSnapshot( ref ByteStream reader )
	{
		using ( GameObject.Push() )
		{
			Entity = TypeLibrary.FromBytes<Entity>( ref reader );
		}
	}

	public void WriteSnapshot( ref ByteStream writer )
	{
		TypeLibrary.ToBytes( Entity, ref writer );
	}

	protected override void OnStart()
	{
		if ( IsProxy ) return;
		Entity.Spawn();
	}

	protected override void OnDestroy()
	{
		if ( IsProxy ) return;
		Entity.OnDestroy();
	}
}
