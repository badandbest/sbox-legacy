using Sandbox.Utility;
using System;

namespace Sandbox;

public static class GameObjectExtensions
{
	private static GameObject _scoped;

	extension( GameObject instance )
	{
		internal static GameObject Scoped => _scoped;

		/// <summary>
		/// Push this game object to be used for new entities instead of creating their own.
		/// </summary>
		/// <returns></returns>
		public IDisposable Push()
		{
			_scoped = instance;

			return DisposeAction.Create( () => _scoped = null );
		}
	}
}
