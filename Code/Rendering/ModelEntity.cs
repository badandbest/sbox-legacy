using Sandbox;

namespace Legacy;

public class ModelEntity : Entity
{
	internal virtual ModelRenderer Renderer => GameObject.GetOrAddComponent<ModelRenderer>();

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
