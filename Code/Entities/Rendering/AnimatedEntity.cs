namespace Sandbox;

public class AnimatedEntity : ModelEntity
{
	[Hide]
	public override SceneModel SceneObject { get; } = new SceneModel( Game.ActiveScene.SceneWorld, Model.Error, Transform.Zero );

	/// <summary>
	/// Retrieve parameter value of currently active Animation Graph.
	/// </summary>
	/// <param name="name">Name of the parameter to look up value of.</param>
	/// <returns>The value of given parameter.</returns>
	public bool GetAnimParameterBool( string name ) => SceneObject.GetBool( name );

	/// <inheritdoc cref="M:Sandbox.AnimatedEntity.GetAnimParameterBool(System.String)" />
	public float GetAnimParameterFloat( string name ) => SceneObject.GetFloat( name );

	/// <inheritdoc cref="M:Sandbox.AnimatedEntity.GetAnimParameterBool(System.String)" />
	public Vector3 GetAnimParameterVector( string name ) => SceneObject.GetVector3( name );

	/// <inheritdoc cref="M:Sandbox.AnimatedEntity.GetAnimParameterBool(System.String)" />
	public int GetAnimParameterInt( string name ) => SceneObject.GetInt( name );

	/// <inheritdoc cref="M:Sandbox.AnimatedEntity.GetAnimParameterBool(System.String)" />
	public Rotation GetAnimParameterRotation( string name ) => SceneObject.GetRotation( name );

	/// <summary>
	/// Sets the animation graph parameter.
	/// </summary>
	/// <param name="name">Name of the parameter to set.</param>
	/// <param name="value">Value to set.</param>
	public void SetAnimParameter( string name, bool value ) => SceneObject.SetAnimParameter( name, value );

	/// <inheritdoc cref="M:Sandbox.AnimatedEntity.SetAnimParameter(System.String,System.Boolean)" />
	public void SetAnimParameter( string name, float value ) => SceneObject.SetAnimParameter( name, value );

	/// <inheritdoc cref="M:Sandbox.AnimatedEntity.SetAnimParameter(System.String,System.Boolean)" />
	public void SetAnimParameter( string name, Vector3 value ) => SceneObject.SetAnimParameter( name, value );

	/// <inheritdoc cref="M:Sandbox.AnimatedEntity.SetAnimParameter(System.String,System.Boolean)" />
	public void SetAnimParameter( string name, Rotation value ) => SceneObject.SetAnimParameter( name, value );

	/// <inheritdoc cref="M:Sandbox.AnimatedEntity.SetAnimParameter(System.String,System.Boolean)" />
	public void SetAnimParameter( string name, int value ) => SceneObject.SetAnimParameter( name, value );

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

	protected override void OnPreRender()
	{
		base.OnPreRender();

		SceneObject.Update( Time.Delta );
	}
}
