using System;

namespace Sandbox;

public partial class ModelEntity : Entity
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
	/// Set the <see cref="Model" /> of this entity by name/path.
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
		SceneObject.Model = Model;

		var firstPerson = IsFirstPersonMode;
		var isOverlay = EnableViewmodelRendering;

		var opaque = firstPerson ? !EnableHideInFirstPerson : EnableDrawing;
		var shadow = firstPerson ? EnableShadowInFirstPerson : EnableShadowCasting;

		if ( isOverlay )
		{
			opaque = opaque && Camera.FirstPersonViewer != null;
		}

		SceneObject.Flags.IsOpaque = opaque;
		SceneObject.Flags.CastShadows = shadow;
		SceneObject.Flags.OverlayLayer = isOverlay;
	}

	public override void SetParent( Entity entity, string attachmentOrBoneName = null, Transform? transform = null )
	{
		base.SetParent( entity, attachmentOrBoneName, transform );

		if ( entity is ModelEntity { SceneObject: var so } )
		{
			so.AddChild( string.Empty, SceneObject );
		}
	}
}
