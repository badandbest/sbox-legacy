using System;
using System.Collections.Generic;
using System.Linq;

namespace Sandbox;

internal class EntityComponentSystem( Entity entity ) : IComponentSystem
{
	private List<EntityComponent> list = [];

	/// <inheritdoc cref="IComponentSystem.Count"/>
	public int Count => list.Count;

	/// <inheritdoc cref="IComponentSystem.Create"/>
	public T Create<T>( bool startEnabled = true ) where T : EntityComponent, new()
	{
		var component = new T { Enabled = startEnabled };

		Add( component );
		return component;
	}

	/// <inheritdoc cref="IComponentSystem.Add"/>
	public bool Add( EntityComponent component )
	{
		if ( !component.CanAddToEntity( entity ) )
		{
			throw new InvalidOperationException( $"CanAddToEntity: Component {component} cannot be added to {entity}" );
		}

		// Remove from old entity.
		component.Entity?.Components.Remove( component );
		component.Entity = entity;

		RemoveSingletons( component.GetType() );

		list.Add( component );
		entity.OnComponentAdded( component );

		return true;
	}

	/// <inheritdoc cref="IComponentSystem.Remove"/>
	public bool Remove( EntityComponent c )
	{
		if ( !list.Contains( c ) ) return false;

		entity.OnComponentRemoved( c );
		c.Entity = null;

		return true;
	}

	/// <inheritdoc cref="IComponentSystem.RemoveAny"/>
	public bool RemoveAny<T>() where T : EntityComponent
	{
		return list.RemoveAll( c => c is T ) > 0;
	}

	/// <inheritdoc cref="IComponentSystem.RemoveAll"/>
	public void RemoveAll()
	{
		list.ForEach( c => Remove( c ) );
	}

	/// <inheritdoc cref="IComponentSystem.Get"/>
	public T Get<T>( bool includeDisabled = false ) where T : EntityComponent
	{
		return list.OfType<T>().FirstOrDefault( c => c.Enabled );
	}

	/// <inheritdoc cref="IComponentSystem.TryGet"/>
	public bool TryGet<T>( out T component, bool includeDisabled = false ) where T : EntityComponent
	{
		component = Get<T>( includeDisabled );
		return component != null;
	}

	/// <inheritdoc cref="IComponentSystem.GetAll"/>
	public IEnumerable<T> GetAll<T>( bool includeDisabled = false ) where T : EntityComponent
	{
		return list.OfType<T>().Where( c => c.Enabled ).ToList();
	}

	/// <inheritdoc cref="IComponentSystem.GetOrCreate"/>
	public T GetOrCreate<T>( bool startEnabled = true ) where T : EntityComponent, new()
	{
		return TryGet<T>( out var component ) ? component : Create<T>( startEnabled );
	}

	private void RemoveSingletons( Type type )
	{
		if ( !type.IsAssignableTo( typeof( ISingletonComponent ) ) ) return;

		while ( type.BaseType!.IsAssignableTo( typeof( ISingletonComponent ) ) )
		{
			type = type.BaseType;
		}

		list.RemoveAll( c => c.GetType().IsAssignableTo( type ) );
	}
}
