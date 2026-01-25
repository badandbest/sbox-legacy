using System.Linq;

namespace Sandbox;

/// <summary>
/// Extensions for <see cref="ClothingContainer"/> that adds members from the old entity system.
/// </summary>
public static class ClothingContainerExtensions
{
	extension( ClothingContainer source )
	{
		/// <summary>
		/// Load the clothing from this client's data. This is a different entry
		/// point than just calling Deserialize directly because if we have
		/// inventory based skins at some point, we can validate ownership here
		/// </summary>
		public void LoadFromClient( IClient cl ) => source.Deserialize( cl.GetClientData( "avatar" ) );

		/// <summary>
		/// Dress this citizen with clothes defined inside this class. We'll save the created entities in ClothingModels.
		/// All clothing entities are tagged with "clothes".
		/// </summary>
		public void DressEntity( AnimatedEntity citizen, bool hideInFirstPerson = true, bool castShadowsInFirstPerson = true )
		{
			// TODO: Apply materials body groups

			foreach ( var clothing in source.Clothing.Select( x => x.Clothing ) )
			{
				var anim = new AnimatedEntity( clothing.Model, citizen );

				anim.Tags.Add( "clothes" );

				anim.EnableHideInFirstPerson = hideInFirstPerson;
				anim.EnableShadowInFirstPerson = castShadowsInFirstPerson;
				//anim.EnableTraceAndQueries = false;
			}
		}
	}
}
