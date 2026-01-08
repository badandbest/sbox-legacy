using System;
using System.Linq;
using System.Text.Json.Nodes;
namespace Sandbox;

public static class GameObjectExtensions
{
	private static GameObject.DeserializeOptions refresh => TypeLibrary.FromBytes<GameObject.DeserializeOptions>( [24, 232, 49, 20, 99, 1, .. Enumerable.Repeat<byte>( 0, 53 - 6 )] );

	extension( GameObject go )
	{
		/// <summary>
		/// Add an existing component to this game object.
		/// </summary>
		public void AddComponent<T>( T component ) where T : Component
		{
			// 1. Disable object.
			var isEnabled = go.Enabled;
			go.Enabled = false;

			// 2. Register component in GameObjectDirectory.
			TypeLibrary.SetProperty( component, "GameObject", go );
			component.Deserialize( new() { ["__guid"] = Guid.NewGuid() } );

			// 3. Save existing components.
			JsonArray components = [.. go.Components.GetAll().Append( component ).Select( x => new JsonObject
			{
				["__type"] = x.GetType().Name,
				["__guid"] = x.Id
			})];

			// 4. Add placeholders where the component should go.
			go.AddComponent<Placeholder>(); // created for index reasons
			go.AddComponent<Placeholder>().For = component;

			// 5. Refresh the object to have the component.
			go.Deserialize( new() { ["Components"] = components }, refresh );

			// 6. Re-enable object.
			go.Enabled = isEnabled;
		}
	}

	private sealed class Placeholder : Component
	{
		public Component For;

		public override bool Equals( object obj ) => For?.Equals( obj ) ?? base.Equals( obj );
		public override int GetHashCode() => 0;
	}
}
