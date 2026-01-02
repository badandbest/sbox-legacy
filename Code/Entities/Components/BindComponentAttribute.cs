using System;

namespace Sandbox;

[AttributeUsage( AttributeTargets.Property )]
[CodeGenerator( CodeGeneratorFlags.WrapPropertyGet | CodeGeneratorFlags.Instance, "Sandbox.BindComponentAttribute.BindComponent" )]
public class BindComponentAttribute : Attribute
{
	public bool IncludeDisabled { get; set; } = false;

	public static T BindComponent<T>( WrappedPropertyGet<T> component ) where T : EntityComponent, new()
	{
		var entity = component.Object as Entity;

		return entity.Components.GetOrCreate<T>();
	}
}
