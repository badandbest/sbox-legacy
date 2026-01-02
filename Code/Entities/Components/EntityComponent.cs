using System;

namespace Sandbox;

[Library, Icon( "extension" ), Tint( EditorTint.White )]
public abstract class EntityComponent : Component
{
	/// <summary>
	/// The entity this component is attached to.
	/// </summary>
	public Entity Entity => GameObject;

	/// <summary>
	/// Return false if can't be added to this entity for some reason.
	/// </summary>
	public virtual bool CanAddToEntity( Entity entity ) => throw new NotImplementedException();

	/// <summary>
	/// Called when this component is enabled (or added to the entity).
	/// </summary>
	protected virtual void OnActivate() { }

	/// <summary>
	/// Called when this component is disabled (or removed from the entity).
	/// </summary>
	protected virtual void OnDeactivate() { }

	/// <summary>
	/// Remove this component from the parent entity. Entity will become null immediately,
	/// so don't try to access it after calling this!
	/// </summary>
	public void Remove() => Destroy();

	protected sealed override void OnEnabled() => OnActivate();
	protected sealed override void OnDisabled() => OnDeactivate();
}

/// <summary>
/// Component that can be only added to given entity type.
/// </summary>
/// <typeparam name="T">Entity type this component can be added to.</typeparam>
public abstract class EntityComponent<T> : EntityComponent where T : Entity
{
	/// <inheritdoc cref="P:Sandbox.EntityComponent.Entity" />
	public new T Entity => base.Entity as T;
}
