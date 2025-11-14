using Legacy;
using System;
using System.Linq;

namespace Legacy;

[AttributeUsage( AttributeTargets.Method, Inherited = true, AllowMultiple = true )]
public class EventAttribute( string eventName ) : Attribute
{
	/// <summary>
	/// The internal event identifier.
	/// </summary>
	public string EventName { get; set; } = eventName;

	/// <summary>
	/// Events with lower numbers are run first. This defaults to 0, so setting it to
	/// -1 will mean your event will run before all other events that don't define it.
	/// Setting it to 1 would mean it'll run after all events that don't.
	/// </summary>
	public int Priority { get; set; }
}

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
}
