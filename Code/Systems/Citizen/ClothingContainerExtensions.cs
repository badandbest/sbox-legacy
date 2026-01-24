namespace Sandbox;

/// <summary>
/// Extensions for <see cref="ClothingContainer"/> that adds members from the old entity system.
/// </summary>
public static class ClothingContainerExtensions
{
	extension( ClothingContainer source )
	{
		public void LoadFromClient( IClient cl ) => source.Deserialize( Connection.Local.GetUserData( "avatar" ) );

		//public void DressEntity( AnimatedEntity entity ) => source.Apply( entity.Renderer );
	}
}
