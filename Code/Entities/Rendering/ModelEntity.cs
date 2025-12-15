using System;

namespace Sandbox;

public class ModelEntity : Entity
{
	internal virtual ModelRenderer Renderer => GameObject.GetOrAddComponent<ModelRenderer>();

	/// <summary>
	/// The <see cref="P:Sandbox.ModelEntity.SceneObject" /> that represents this entity.
	/// </summary>
	[Hide]
	public virtual SceneObject SceneObject => Renderer.SceneObject;

	public ModelEntity()
	{
	}

	public ModelEntity( string modelName )
	{
		SetModel( modelName );
	}

	public ModelEntity( string modelName, Entity parent )
	{
		SetModel( modelName );
		SetParent( parent, boneMerge: true );
	}

	/// <summary>
	/// Access to this entity's model.
	/// </summary>
	// [Prefab]
	public Model Model
	{
		get => Renderer.Model;
		set
		{
			Renderer.Model = value;
			OnNewModel( value );
		}
	}

	/// <summary>
	/// The collision bounds.
	/// </summary>
	[Hide]
	public BBox CollisionBounds
	{
		get => Model.Bounds;
		set => throw new NotImplementedException();
	}

	/// <summary>
	/// Set the <see cref="P:Sandbox.ModelEntity.Model" /> of this entity by name/path.
	/// </summary>
	public void SetModel( string name )
	{
		// AssertNotPreSpawn( "SetModel" );

		Model = Model.Load( name );
	}

	/// <summary>
	/// Called when the model of this entity has changed.
	/// </summary>
	public virtual void OnNewModel( Model model ) { }
}
