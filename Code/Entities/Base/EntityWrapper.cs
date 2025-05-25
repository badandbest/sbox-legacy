using Sandbox;

namespace Legacy;

/// <summary>
/// Lets us get entities from game objects.
/// Also lets us listen to creation/destruction events.
/// Ticks are handled in <see cref="GameManager"/>.
/// </summary>
public sealed class EntityWrapper : Component
{
	public Entity Entity { get; set; }

	protected override void OnStart() => Entity.Spawn();
	protected override void OnDestroy() => Entity.Delete();
}
