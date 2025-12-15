using System.Linq;

namespace Sandbox;

public static class GameEvent
{
	public static void Run( string eventName, params object[] parameters )
	{
		// This sucks so much. This is super temporary.

		foreach ( var (method, eventAttribute) in TypeLibrary.GetMethodsWithAttribute<EventAttribute>( false )
			.Where( x => x.Attribute.EventName == eventName ) )
		{
			foreach ( var entity in Entity.All )
			{
				if ( !entity.GetType().IsAssignableTo( method.TypeDescription.TargetType ) )
				{
					continue;
				}

				method.Invoke( entity, parameters );
			}
		}
	}

	public static class Client
	{
		public sealed class PostCameraAttribute() : EventAttribute( "camera.post" );
	}
}
