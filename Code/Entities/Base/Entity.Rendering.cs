namespace Legacy;

public partial class Entity
{
	/// <summary>
	/// Turning this off will completely prevent the entity from drawing
	/// </summary>
	[Category( "Rendering" )]
	public bool EnableDrawing { get; set; }

	/// <summary>
	/// Don't cast no shadow
	/// </summary>
	[Category( "Rendering" )]
	public bool EnableShadowCasting { get; set; }

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
