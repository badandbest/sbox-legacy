using System;

namespace Sandbox;

[Icon( "extension" ), Tint( EditorTint.White )]
public class EntityComponent : Component, IComponent
{
	/// <inheritdoc cref="IComponent.IsClientOnly"/>
	public bool IsClientOnly => throw new NotImplementedException();

	/// <inheritdoc cref="IComponent.IsServerOnly"/>
	public bool IsServerOnly => throw new NotImplementedException();

	/// <inheritdoc cref="IComponent.Name"/>
	public string Name { get; set; }

	/// <summary>
	/// The entity this component is attached to.
	/// </summary>
	public Entity Entity => GameObject;

	/// <summary>
	/// Called when this component is enabled (or added to the entity).
	/// </summary>
	protected virtual void OnActivate() { }

	/// <summary>
	/// Called when this component is disabled (or removed from the entity).
	/// </summary>
	protected virtual void OnDeactivate() { }

	/// <summary>
	/// Return false if can't be added to this entity for some reason.
	/// </summary>
	public virtual bool CanAddToEntity( Entity entity ) => true;

	/// <summary>
	/// Remove this component from the parent entity. Entity will become null immediately,
	/// so don't try to access it after calling this!
	/// </summary>
	public void Remove() => Destroy();

	/// <inheritdoc cref="object.ToString"/>
	public override string ToString() => Name ?? $"{DisplayInfo.For( this ).Name} {GetHashCode()}";

	#region Forwarded actions

	protected override void OnEnabled() => OnActivate();
	protected override void OnDisabled() => OnDeactivate();

	#endregion
}

/// <summary>
/// Component that can be only added to given entity type.
/// </summary>
/// <typeparam name="T">Entity type this component can be added to.</typeparam>
public class EntityComponent<T> : EntityComponent where T : Entity
{
	/// <inheritdoc cref="P:Sandbox.EntityComponent.Entity" />
	public new T Entity => base.Entity as T;

	/// <summary>
	/// Return false if target entity is not of type T.
	/// </summary>
	public override bool CanAddToEntity( Entity entity ) => entity is T;
}
