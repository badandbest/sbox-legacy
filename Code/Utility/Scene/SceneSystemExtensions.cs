using Sandbox;

namespace Legacy;

/// <summary>
/// Legacy compatability extensions for scene system.
/// </summary>
public static class SceneSystemExtensions
{
	extension( Input )
	{
		public static bool StopProcessing { get => Input.Suppressed; set => Input.Suppressed = value; }
	}

	extension( Time )
	{
		public static int Tick => (Time.Now / Game.ActiveScene.FixedDelta).CeilToInt();
	}

	extension( ClothingContainer source )
	{
		public void LoadFromClient( IClient cl ) => source.Deserialize( Connection.Local.GetUserData( "avatar" ) );

		public void DressEntity( AnimatedEntity entity ) => source.Apply( entity.Renderer );
	}
}
