using System.Collections.Generic;

namespace Sandbox;

/// <summary>
/// A base entity all other entities derive from.
/// </summary>
[Library( "entity" )]
public partial class Entity : IEntity
{
	/// <summary>
	/// A list of all active entities.
	/// </summary>
	public static List<Entity> All { get; } = [];

	/// <summary>
	/// The game object this entity belongs to.
	/// </summary>
	[Hide]
	public GameObject GameObject { get; }

	/// <summary>
	/// Create the entity.
	/// </summary>
	public Entity()
	{
		var scene = Game.ActiveScene;

		GameObject = scene.CreateObject();

		All.Add( this );
	}

	#region Unimplemented

	public IEntity Owner { get; }
	public IClient Client { get; }
	public int Id { get; }
	public bool IsDormant { get; }
	public bool IsOwnedByLocalClient { get; }
	public bool IsFromMap { get; }
	public BBox WorldSpaceBounds { get; }
	public string TagList { get; set; }
	public bool IsClientOnly { get; }
	public int NetworkIdent { get; }
	public Vector3 Velocity { get; set; }
	public bool IsAuthority { get; }
	public IComponentSystem Components { get; }

	#endregion
}
