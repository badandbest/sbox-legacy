using System;

namespace Legacy;

public static class Prediction
{
	/// <summary>
	/// Temporarily disables prediction.
	///
	/// <para>
	/// Expected usage is like so:
	/// <code>using ( Prediction.Off() )
	/// {
	/// 	// Code that needs prediction disabled here...
	/// }
	/// </code>
	/// </para>
	/// </summary>
	public static IDisposable Off() => new EmptyDisposable();
}

file struct EmptyDisposable : IDisposable
{
	public void Dispose() { }
}
