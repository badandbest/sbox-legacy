using Sandbox;

namespace Legacy;

public partial class Entity
{
	public bool IsValid => GameObject.IsValid();

	public void Delete()
	{
		All.Remove( this );
		
		GameObject.Destroy();
	}
}
