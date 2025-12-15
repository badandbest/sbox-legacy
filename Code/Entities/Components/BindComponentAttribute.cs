using System;

namespace Sandbox;

[AttributeUsage( AttributeTargets.Property )]
[CodeGenerator( CodeGeneratorFlags.WrapPropertyGet | CodeGeneratorFlags.Instance, "BindComponent" )]
public class BindComponentAttribute : Attribute
{
	public bool IncludeDisabled { get; set; } = false;
}
