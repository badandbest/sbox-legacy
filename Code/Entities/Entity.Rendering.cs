namespace Sandbox;

public partial class Entity
{
	/// <summary>
	/// If this entity is being viewed through, or is a child of an entity that is being view
	/// through - will return true. This can be read are we in first person mode.
	/// </summary>
	[Hide]
	public virtual bool IsFirstPersonMode => Parent?.IsFirstPersonMode ?? Camera.FirstPersonViewer == this;

	/// <summary>
	/// Turning this off will completely prevent the entity from drawing
	/// </summary>
	[Category( "Rendering" )]
	public bool EnableDrawing { get; set; } = true;

	/// <summary>
	/// Don't cast no shadow
	/// </summary>
	[Category( "Rendering" )]
	public bool EnableShadowCasting { get; set; } = true;

	/// <summary>
	/// Render Shadows when hidden due to being in first person
	/// </summary>
	[Category( "Rendering" )]
	public bool EnableShadowInFirstPerson { get; set; }

	/// <summary>
	/// Hide this model when in first person, or our parent is in first person
	/// </summary>
	[Category( "Rendering" )]
	public bool EnableHideInFirstPerson { get; set; }

	/// <summary>
	/// Enable Viewmodel Rendering
	/// </summary>
	[Category( "Rendering" )]
	public bool EnableViewmodelRendering { get; set; }
}
