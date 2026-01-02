namespace Sandbox;

/// <summary>
/// Extensions for <see cref="GameTags"/> that adds members from the old entity system.
/// </summary>
public static class TagExtensions
{
	extension( ITagSet source )
	{
		/// <summary>
		/// 
		/// </summary>
		public void Clear() => source.RemoveAll();

		internal void SetFrom( string tags )
		{
			source.RemoveAll();

			foreach ( var tag in tags.SplitQuotesStrings() )
			{
				source.Add( tag );
			}
		}
	}
}
