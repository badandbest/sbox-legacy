using System;
using Sandbox;

namespace Legacy;

public class EntityComponent : IComponent
{
	/// <summary>
	/// The entity this component is attached to.
	/// </summary>
	[Hide]
	public Entity Entity { get; internal set; }

	public bool Enabled { get; set; }

	public bool IsClientOnly => throw new NotImplementedException();
	public bool IsServerOnly => throw new NotImplementedException();
	public string Name { get; set; }

	/// <summary>
	/// Called when this component is enabled (or added to the entity)
	/// </summary>
	protected virtual void OnActivate()
	{
	}

	/// <summary>
	/// Called when this component is disabled (or removed from the entity)
	/// </summary>
	protected virtual void OnDeactivate()
	{
	}

	/// <summary>
	/// Return false if can't be added to this entity for some reason
	/// </summary>
	public virtual bool CanAddToEntity( Entity entity )
	{
		return true;
	}

	/// <summary>
	/// Remove this component from the parent entity. Entity will become null immediately,
	/// so don't try to access it after calling this!
	/// </summary>
	public void Remove()
	{
		if ( Entity.IsValid() )
		{
			Entity.Components.Remove( this );
		}
	}

	public override string ToString() => Name ?? $"{DisplayInfo.For( this ).Name} {GetHashCode()}";
}
