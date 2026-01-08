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
	public virtual bool CanAddToEntity( Entity entity ) => true;

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

	#region Forwarded actions

	protected override void OnValidate()
	{
		if ( CanAddToEntity( Entity ) )
		{
			return;
		}

		Log.Error( $"CanAddToEntity: Component {this} cannot be added to {Entity}" );

		Destroy();
	}

	protected override void OnStart()
	{
		if ( IsProxy ) return;

		Entity.OnComponentAdded( this );
		Network.Refresh( this );
	}

	protected override void OnDestroy()
	{
		if ( IsProxy ) return;

		Entity?.OnComponentRemoved( this );
		Network.Refresh( this );
	}

	protected sealed override void OnEnabled()
	{
		if ( IsProxy ) return;

		OnActivate();
	}

	protected sealed override void OnDisabled()
	{
		if ( IsProxy ) return;

		OnDeactivate();
	}

	#endregion
}

/// <summary>
/// Component that can be only added to given entity type.
/// </summary>
/// <typeparam name="T">Entity type this component can be added to.</typeparam>
public abstract class EntityComponent<T> : EntityComponent where T : Entity
{
	/// <inheritdoc cref="EntityComponent.Entity" />
	public new T Entity => base.Entity as T;

	/// <summary>
	/// Return false if target entity is not of type T.
	/// </summary>
	public override bool CanAddToEntity( Entity entity ) => entity is T or null;
}
