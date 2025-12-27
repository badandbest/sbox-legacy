using System;
using System.Text.Json;

namespace Sandbox;

public partial class Entity : IJsonConvert
{
	public static object JsonRead( ref Utf8JsonReader reader, Type typeToConvert )
	{
		if ( reader.TryGetGuid( out var id ) )
		{
			return (Entity)Game.ActiveScene.Directory.FindByGuid( id );
		}

		return null;
	}

	public static void JsonWrite( object value, Utf8JsonWriter writer )
	{
		if ( value is Entity { GameObject.Id: var id } )
		{
			writer.WriteStringValue( id );
		}
	}
}
