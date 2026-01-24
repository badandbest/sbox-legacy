using System.Collections.Generic;

namespace Sandbox;

/// <summary>
/// A common base we can use for weapons so we don't have to implement the logic over and over
/// again.
/// </summary>
[Category( "ViewModel" )]
[Title( "ViewModel" ), Icon( "pan_tool" )]
public class BaseViewModel : AnimatedEntity
{
	/// <summary>
	/// All active view models.
	/// </summary>
	public static List<BaseViewModel> AllViewModels = [];

	public BaseViewModel()
	{
		AllViewModels.Add( this );
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		AllViewModels.Remove( this );
	}

	/// <summary>
	/// Position your view model here.
	/// </summary>
	[GameEvent.Client.PostCamera]
	public virtual void PlaceViewmodel()
	{
		Position = Camera.Position;
		Rotation = Camera.Rotation;
	}

	public override SoundHandle PlaySound( string soundName, string attachment )
	{
		return Owner?.PlaySound( soundName, attachment ) ?? base.PlaySound( soundName, attachment );
	}
}
