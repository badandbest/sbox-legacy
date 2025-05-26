using System.Collections.Generic;

namespace Legacy;

public interface IComponentSystem
{
	/// <summary>
	/// The amount of components, including disabled
	/// </summary>
	int Count { get; }

	/// <summary>
	/// Remove given component from this system
	/// </summary>
	bool Remove( EntityComponent component );

	/// <summary>
	/// Add component to this system
	/// </summary>
	bool Add( EntityComponent component );

	/// <summary>
	/// Get a component by type, if it exists
	/// </summary>
	T Get<T>( bool includeDisabled = false ) where T : EntityComponent;

	/// <summary>
	/// Returns true if component was found, else false
	/// </summary>
	bool TryGet<T>( out T component, bool includeDisabled = false ) where T : EntityComponent;

	/// <summary>
	/// Create the component
	/// </summary>
	T Create<T>( bool startEnabled = true ) where T : EntityComponent, new();

	/// <summary>
	/// Get the component, create if it doesn't exist. Will include disabled components in search.
	/// </summary>
	T GetOrCreate<T>( bool startEnabled = true ) where T : EntityComponent, new();

	/// <summary>
	/// Remove all components of given type
	/// </summary>
	bool RemoveAny<T>() where T : EntityComponent;

	/// <summary>
	/// Get all components by type, if any exist
	/// </summary>
	IEnumerable<T> GetAll<T>( bool includeDisabled = false ) where T : EntityComponent;

	/// <summary>
	/// Remove all components to this entity
	/// </summary>
	void RemoveAll();
}
