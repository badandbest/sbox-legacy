using System;

namespace Sandbox;

[AttributeUsage( AttributeTargets.Property )]
[CodeGenerator( CodeGeneratorFlags.WrapPropertyGet | CodeGeneratorFlags.Instance, "Sandbox.BindComponentAttribute.Bind" )]
public class BindComponentAttribute : RequireComponentAttribute
{
	public bool IncludeDisabled { get; set; } = false;

	public static T Bind<T>( WrappedPropertyGet<T> p ) where T : Component
	{
		if ( p.Object is Entity entity )
		{
			return entity.GetComponent<T>();
		}

		return null;
	}
}
