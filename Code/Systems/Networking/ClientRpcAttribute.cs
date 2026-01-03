using System;

namespace Sandbox;

/// <summary>
/// A method that can be called by the server.
/// </summary>
[AttributeUsage( AttributeTargets.Method )]
[CodeGenerator( CodeGeneratorFlags.WrapMethod | CodeGeneratorFlags.Instance, "__rpc_Wrapper", 0 )]
[CodeGenerator( CodeGeneratorFlags.WrapMethod | CodeGeneratorFlags.Static, "Sandbox.Rpc.OnCallRpc", 0 )]
public class ClientRpcAttribute : Rpc.BroadcastAttribute;
