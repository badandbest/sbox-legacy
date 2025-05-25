using Sandbox;

namespace Legacy;

/// <summary>
/// This entity defines the spawn point of the player in first person shooter gamemodes.
/// </summary>
[Library( "info_player_start" ), HammerEntity]
[EditorModel( "models/editor/playerstart.vmdl", FixedBounds = true )]
[Title( "Player Spawnpoint" ), Category( "Player" ), Icon( "place" )]
public class SpawnPoint : Entity;
