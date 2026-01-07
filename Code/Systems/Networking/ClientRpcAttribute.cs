using System;
using System.Linq;

namespace Sandbox;

/// <summary>
/// A method that can be called by the server.
/// </summary>
[AttributeUsage( AttributeTargets.Method )]
[CodeGenerator( CodeGeneratorFlags.WrapMethod | CodeGeneratorFlags.Instance | CodeGeneratorFlags.Static, "Sandbox.ClientRpcAttribute.Filter", 1 )]
[CodeGenerator( CodeGeneratorFlags.WrapMethod | CodeGeneratorFlags.Instance, "__rpc_Wrapper", 0 )]
[CodeGenerator( CodeGeneratorFlags.WrapMethod | CodeGeneratorFlags.Static, "Sandbox.Rpc.OnCallRpc", 0 )]
public class ClientRpcAttribute : Rpc.BroadcastAttribute
{
	public static void Filter( WrappedMethod m, params object[] parameters )
	{
		using ( Game.ClientScope() )
		{
			var filter = parameters.OfType<To?>().FirstOrDefault();

			if ( !filter.HasValue )
			{
				m.Resume();
				return;
			}

			using ( Rpc.FilterInclude( [.. filter.Value.OfType<ClientEntity>()] ) )
			{
				m.Resume();
			}
		}
	}
}
