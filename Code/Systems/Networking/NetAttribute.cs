using System;

namespace Sandbox;

/// <summary>
/// Mark a property as networked, so it is sent from the server to the client.
/// </summary>
[AttributeUsage( AttributeTargets.Property )]
[CodeGenerator( CodeGeneratorFlags.WrapPropertySet | CodeGeneratorFlags.Instance, "__sync_SetValue", 0 )]
[CodeGenerator( CodeGeneratorFlags.WrapPropertyGet | CodeGeneratorFlags.Instance, "__sync_GetValue", 0 )]
public class NetAttribute : SyncAttribute;
