namespace Sandbox.Systems.Scene;

public static class ComponentExtensions
{
	extension( ComponentList list )
	{
		/// <summary>
		/// Remove given component from this system
		/// </summary>
		public void Remove( EntityComponent c ) => c.Destroy();

		/// <summary>
		/// Remove all components to this entity
		/// </summary>
		public void RemoveAll() => list.RemoveAny<EntityComponent>();

		/// <summary>
		/// Remove all components of given type
		/// </summary>
		public void RemoveAny<T>() where T : Component => list.ForEach<T>( nameof( RemoveAny ), true, x => x.Destroy() );
	}
}
