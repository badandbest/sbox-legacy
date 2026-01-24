using System;

namespace Sandbox;

public class ModelEntity : Entity
{
	/// <summary>
	/// The <see cref="Sandbox.SceneObject" /> that represents this entity.
	/// </summary>
	[Hide]
	public virtual SceneObject SceneObject { get; } = new SceneObject( Game.ActiveScene.SceneWorld, Model.Cube, Transform.Zero );

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
	[Property]
	public Model Model
	{
		get => SceneObject.Model;
		set
		{
			SceneObject.Model = value;
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
		Model = Model.Load( name );
	}

	/// <summary>
	/// Called when the model of this entity has changed.
	/// </summary>
	public virtual void OnNewModel( Model model ) { }

	protected override void OnPreRender()
	{
		SceneObject.Transform = Transform;
	}

	public override void SetParent( Entity entity, bool boneMerge )
	{
		base.SetParent( entity, boneMerge );

		if ( entity is ModelEntity { SceneObject: var so } )
		{
			so.AddChild( string.Empty, SceneObject );
		}
	}
}
