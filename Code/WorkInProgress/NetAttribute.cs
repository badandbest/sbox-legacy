using System;

namespace Legacy;

/// <summary>
/// Mark a property as networked, so it is sent from the server to the client.
/// </summary>
[AttributeUsage( AttributeTargets.Property )]
public class NetAttribute : Attribute;
