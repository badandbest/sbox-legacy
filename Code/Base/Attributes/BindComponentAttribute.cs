using System;

namespace Legacy;

[AttributeUsage( AttributeTargets.Property )]
public class BindComponentAttribute : Attribute
{
	public bool IncludeDisabled { get; set; } = false;
}
