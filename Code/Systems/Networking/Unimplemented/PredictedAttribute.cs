using System;

namespace Sandbox;

/// <summary>
/// Variable gets saved and destroyed with other predicted variables. If it's also <see cref="NetAttribute">[Net]</see>worked it'll be checked with the networked version to make sure it matches for each tick.
/// </summary>
[AttributeUsage( AttributeTargets.Property )]
public class PredictedAttribute : Attribute;
