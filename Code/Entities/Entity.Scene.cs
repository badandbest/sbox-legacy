using System.Collections.Generic;

namespace Sandbox;

public partial class Entity
{
	/// <summary>
	/// A list of all active entities.
	/// </summary>
	public static IReadOnlyList<Entity> All => [.. Game.ActiveScene.GetAll<Handle>()];

	/// <summary>
	/// The game object this entity is bindined to.
	/// </summary>
	private readonly GameObject GameObject = GameObject.Scoped;
	public bool IsValid => GameObject.IsValid;

	/// <summary>
	/// Components of this entity.
	/// </summary>
	public ComponentList Components => GameObject?.Components;

	/// <summary>
	/// Create the entity.
	/// </summary>
	public Entity()
	{
		if ( GameObject is null )
		{
			GameObject = new( GetType().Name );
			GameObject.NetworkSpawn();
		}

		Components.GetOrCreate<Handle>().Entity = this;
	}

	/// <summary>
	/// Delete this entity. You shouldn't access it anymore.
	/// </summary>
	public void Delete() => GameObject.Destroy();

	public static implicit operator GameObject( Entity entity ) => entity.GameObject;

	public static implicit operator Entity( GameObject go ) => go?.GetComponent<Handle>();

	/// <summary>
	/// A component that forwards actions to an entity.
	/// </summary>
	[Title( "Entity Handle" ), Icon( "people" ), Tint( EditorTint.White )]
	private sealed class Handle : Component, Component.INetworkSnapshot
	{
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

		public static implicit operator Entity( Handle handle ) => handle?.Entity;

		#region Forwarded actions

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

		#endregion
	}
}
