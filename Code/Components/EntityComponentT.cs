namespace Legacy;

/// <summary>
/// Component that can be only added to given entity type.
/// </summary>
/// <typeparam name="T">Entity type this component can be added to.</typeparam>
public class EntityComponent<T> : EntityComponent where T : Entity
{
	/// <inheritdoc cref="P:Sandbox.EntityComponent.Entity" />
	public new T Entity => base.Entity as T;

	/// <summary>
	/// Return false if target entity is not of type T
	/// </summary>
	public override bool CanAddToEntity( Entity entity ) => entity is T;
}
