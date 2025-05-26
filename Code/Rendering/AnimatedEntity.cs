using Sandbox;

namespace Legacy;

public class AnimatedEntity : ModelEntity
{
	internal override SkinnedModelRenderer Renderer => GameObject.GetOrAddComponent<SkinnedModelRenderer>();

	/// <summary>
	/// Retrieve parameter value of currently active Animation Graph.
	/// </summary>
	/// <param name="name">Name of the parameter to look up value of.</param>
	/// <returns>The value of given parameter.</returns>
	public bool GetAnimParameterBool( string name ) => Renderer.GetBool( name );

	/// <inheritdoc cref="M:Sandbox.AnimatedEntity.GetAnimParameterBool(System.String)" />
	public float GetAnimParameterFloat( string name ) => Renderer.GetFloat( name );

	/// <inheritdoc cref="M:Sandbox.AnimatedEntity.GetAnimParameterBool(System.String)" />
	public Vector3 GetAnimParameterVector( string name ) => Renderer.GetVector( name );

	/// <inheritdoc cref="M:Sandbox.AnimatedEntity.GetAnimParameterBool(System.String)" />
	public int GetAnimParameterInt( string name ) => Renderer.GetInt( name );

	/// <inheritdoc cref="M:Sandbox.AnimatedEntity.GetAnimParameterBool(System.String)" />
	public Rotation GetAnimParameterRotation( string name ) => Renderer.GetRotation( name );

	/// <summary>
	/// Sets the animation graph parameter.
	/// </summary>
	/// <param name="name">Name of the parameter to set.</param>
	/// <param name="value">Value to set.</param>
	public void SetAnimParameter( string name, bool value ) => Renderer.Set( name, value );

	/// <inheritdoc cref="M:Sandbox.AnimatedEntity.SetAnimParameter(System.String,System.Boolean)" />
	public void SetAnimParameter( string name, float value ) => Renderer.Set( name, value );

	/// <inheritdoc cref="M:Sandbox.AnimatedEntity.SetAnimParameter(System.String,System.Boolean)" />
	public void SetAnimParameter( string name, Vector3 value ) => Renderer.Set( name, value );

	/// <inheritdoc cref="M:Sandbox.AnimatedEntity.SetAnimParameter(System.String,System.Boolean)" />
	public void SetAnimParameter( string name, Rotation value ) => Renderer.Set( name, value );

	/// <inheritdoc cref="M:Sandbox.AnimatedEntity.SetAnimParameter(System.String,System.Boolean)" />
	public void SetAnimParameter( string name, int value ) => Renderer.Set( name, value );

	/// <inheritdoc cref="M:Sandbox.AnimatedEntity.SetAnimParameter(System.String,System.Boolean)" />
	public void SetAnimParameter( string name, Transform value )
	{
		SetAnimParameter( name + ".position", value.Position );
		SetAnimParameter( name + ".rotation", value.Rotation );
	}

	/// <summary>
	/// Converts value to vector local to this entity's eyepos and passes it to SetAnimVector
	/// </summary>
	public void SetAnimLookAt( string name, Vector3 eyePositionInWorld, Vector3 lookatPositionInWorld )
	{
		var direction = (lookatPositionInWorld - eyePositionInWorld) * Rotation.Inverse;
		SetAnimParameter( name, direction );
	}
}
