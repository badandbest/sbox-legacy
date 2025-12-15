namespace Sandbox;

/// <summary>
/// Extensions for <see cref="Surface"/> that adds members from the old entity system.
/// </summary>
public static class SurfaceExtensions
{
	extension( Surface source )
	{
		public void DoBulletImpact( TraceResult tr )
		{
			var particle = source.PrefabCollection.BulletImpact;

			var prefab = particle.Clone( tr.HitPosition, Rotation.LookAt( -tr.Normal ) );
			prefab.PlaySound( source.SoundCollection.Bullet );

			prefab.Flags |= GameObjectFlags.Hidden;
		}
	}
}
