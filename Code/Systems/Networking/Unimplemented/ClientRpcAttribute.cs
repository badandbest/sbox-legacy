using System.Linq;

namespace Sandbox;

/// <summary>
/// A method that can be called by the server.
/// </summary>
[CodeGenerator( CodeGeneratorFlags.WrapMethod | CodeGeneratorFlags.Static | CodeGeneratorFlags.Instance, "Sandbox.ClientRpcAttribute.Filter", 1 )]
public class ClientRpcAttribute : Rpc.BroadcastAttribute
{
	public static void Filter( WrappedMethod m, params object[] argumentList )
	{
		using ( Game.ClientScope() )
		{
			var targets = argumentList.OfType<To?>().FirstOrDefault();

			if ( !targets.HasValue )
			{
				m.Resume();
				return;
			}

			using ( Rpc.FilterInclude( targets.Value.OfType<ClientEntity>().Select( x => x.Network.Owner ) ) )
			{
				m.Resume();
			}
		}
	}
}
