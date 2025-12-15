using System;

namespace Sandbox;

/// <summary>
/// A property on a Client's pawn or a component attached to the Client's pawn that is sent to the server as an input.
/// </summary>
[AttributeUsage( AttributeTargets.Property )]
public class ClientInputAttribute : Attribute;
