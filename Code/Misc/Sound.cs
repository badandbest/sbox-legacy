using Sandbox;

namespace Legacy;

/// <summary>
/// Allows playback and modification of sounds during playback.
/// </summary>
public readonly struct Sound( SoundHandle handle )
{
	/// <summary>
	/// Set an override for the position and rotation of the local client's audio listener.
	/// </summary>
	public static Transform? Listener
	{
		get => Sandbox.Sound.Listener;
		set => Sandbox.Sound.Listener = value ?? Transform.Zero;
	}

	/// <summary>
	/// Stop all currently playing sounds.
	/// </summary>
	public static void StopAll( bool immediately )
	{
		Sandbox.Sound.StopAll( 0 );
	}

	/// <summary>
	/// Return the elapsed time from the start of the sound in seconds.
	/// </summary>
	public float ElapsedTime => handle.ElapsedTime;

	/// <summary>
	/// Return if sound has finished playing.
	/// </summary>
	public bool Finished => handle.Finished;

	/// <summary>
	/// Returns if a sound is currently playing.
	/// </summary>
	public bool IsPlaying => handle.IsPlaying;

	/// <summary>
	/// Create a sound originating from world position.
	/// </summary>
	[ClientRpc]
	public static Sound FromWorld( string name, Vector3 position )
	{
		var sound = ResourceLibrary.Get<SoundEvent>( name );

		return new Sound( Sandbox.Sound.Play( sound, position ) );
	}

	/// <summary>
	/// Create a sound originating from an entity.
	/// </summary>
	[ClientRpc]
	public static Sound FromEntity( string name, Entity entity )
	{
		var sound = ResourceLibrary.Get<SoundEvent>( name );

		return new Sound( entity.GameObject.PlaySound( sound ) );
	}

	/// <summary>
	/// Create a sound originating from an entity attachment.
	/// </summary>
	[ClientRpc]
	public static Sound FromEntity( string name, Entity entity, string attachment )
	{
		var sound = ResourceLibrary.Get<SoundEvent>( name );
		var renderer = entity.GameObject.GetComponent<ModelRenderer>();

		return new Sound( renderer?.GetAttachmentObject( attachment ).PlaySound( sound ) );
	}

	/// <summary>
	/// Create a sound originating from an screen coordinates.
	/// </summary>
	[ClientRpc]
	public static Sound FromScreen( string name, float x = 0.5f, float y = 0.5f )
	{
		var sound = ResourceLibrary.Get<SoundEvent>( name );
		var camera = Game.ActiveScene.Camera.GameObject;

		return new Sound( camera.PlaySound( sound, new(x, y) ) );
	}
}
