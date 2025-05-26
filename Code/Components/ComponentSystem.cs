using System;
using System.Collections.Generic;
using System.Linq;

namespace Legacy;

internal class EntityComponentSystem( Entity entity ) : IComponentSystem
{
	private List<EntityComponent> list = [];

	/// <summary>
	/// The amount of components - including inactive
	/// </summary>
	public int Count => list.Count;

	/// <summary>
	/// Create the component
	/// </summary>
	public T Create<T>( bool startEnabled = true ) where T : EntityComponent, new()
	{
		var component = new T { Enabled = startEnabled };

		Add( component );
		return component;
	}

	/// <summary>
	/// Add a component to this entity
	/// </summary>
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

	/// <summary>
	/// Remove given component from this entity
	/// </summary>
	public bool Remove( EntityComponent c )
	{
		if ( !list.Contains( c ) ) return false;

		entity.OnComponentRemoved( c );
		c.Entity = null;

		return true;
	}

	/// <summary>
	/// Remove all components of given type
	/// </summary>
	public bool RemoveAny<T>() where T : EntityComponent
	{
		return list.RemoveAll( c => c is T ) > 0;
	}

	/// <summary>
	/// Remove all components to this entity
	/// </summary>
	public void RemoveAll()
	{
		list.ForEach( c => Remove( c ) );
	}

	/// <summary>
	/// Get a component by type, if it exists
	/// </summary>
	public T Get<T>( bool includeDisabled = false ) where T : EntityComponent
	{
		return list.OfType<T>().FirstOrDefault( c => c.Enabled );
	}

	/// <summary>
	/// Returns true if component was found, else false
	/// </summary>
	public bool TryGet<T>( out T component, bool includeDisabled = false ) where T : EntityComponent
	{
		component = Get<T>( includeDisabled );
		return component != null;
	}

	/// <summary>
	/// Get all components by type, if any exist
	/// </summary>
	public IEnumerable<T> GetAll<T>( bool includeDisabled = false ) where T : EntityComponent
	{
		return list.OfType<T>().Where( c => c.Enabled ).ToList();
	}

	/// <summary>
	/// Get the component, create if it doesn't exist. Will include disabled components in search.
	/// </summary>
	public T GetOrCreate<T>( bool startEnabled = true ) where T : EntityComponent, new()
	{
		return TryGet<T>( out var component ) ? component : Create<T>( startEnabled );
	}

	private void RemoveSingletons( Type type )
	{
		if ( !type.IsAssignableTo( typeof(ISingletonComponent) ) ) return;

		while ( type.BaseType!.IsAssignableTo( typeof(ISingletonComponent) ) )
		{
			type = type.BaseType;
		}

		list.RemoveAll( c => c.GetType().IsAssignableTo( type ) );
	}
}
