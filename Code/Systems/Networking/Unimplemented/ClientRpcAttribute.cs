using System;

namespace Sandbox;

/// <summary>
/// A method that can be called by the server.
/// </summary>
[AttributeUsage( AttributeTargets.Method )]
public class ClientRpcAttribute : Attribute;
