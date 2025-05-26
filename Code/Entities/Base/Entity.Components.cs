namespace Legacy;

public partial class Entity
{
	/// <summary>
	/// Components of this entity.
	/// </summary>
	public IComponentSystem Components { get; }

	/// <summary>
	/// A component has been added to the entity.
	/// </summary>
	protected internal virtual void OnComponentAdded( EntityComponent component ) { }

	/// <summary>
	/// A component has been removed from the entity.
	/// </summary>
	protected internal virtual void OnComponentRemoved( EntityComponent component ) { }
}
