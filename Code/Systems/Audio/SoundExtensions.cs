namespace Sandbox;

/// <summary>
/// Extensions for <see cref="Sound"/> that adds members from the old entity system.
/// </summary>
public static class SoundExtensions
{
	extension( Sound )
	{
		/// <summary>
		/// Create a sound originating from world position.
		/// </summary>
		public static SoundHandle FromWorld( string name, Vector3 position )
		{
			var sound = ResourceLibrary.Get<SoundEvent>( name );

			return Sound.Play( sound, position );
		}

		/// <summary>
		/// Create a sound originating from an entity.
		/// </summary>
		public static SoundHandle FromEntity( string name, Entity entity )
		{
			var sound = ResourceLibrary.Get<SoundEvent>( name );

			return entity.GameObject.PlaySound( sound );
		}

		/// <summary>
		/// Create a sound originating from an entity attachment.
		/// </summary>
		public static SoundHandle FromEntity( string name, Entity entity, string attachment )
		{
			var sound = ResourceLibrary.Get<SoundEvent>( name );
			var renderer = entity.GameObject.GetComponent<ModelRenderer>();

			return renderer?.GetAttachmentObject( attachment ).PlaySound( sound );
		}

		/// <summary>
		/// Create a sound originating from an screen coordinates.
		/// </summary>
		public static SoundHandle FromScreen( string name, float x = 0.5f, float y = 0.5f )
		{
			var sound = ResourceLibrary.Get<SoundEvent>( name );
			var camera = Game.ActiveScene.Camera.GameObject;

			return camera.PlaySound( sound, new( x, y ) );
		}
	}
}
