using Editor;
using System.Linq;
using System.Reflection;

namespace Sandbox;

[CustomEditor( typeof( IEntity ) )]
internal sealed class EntityWidget : ComponentEditorWidget
{
	public EntityWidget( SerializedObject obj ) : base( obj )
	{
		Layout = Layout.Column();
		Layout.Add( ControlSheet.Create( obj, IsPropertyAcceptable ) );
	}

	bool IsPropertyAcceptable( SerializedProperty x )
	{
		if ( TypeLibrary.GetType<Component>().GetProperty( x.Name ) != null )
		{
			return false;
		}

		if ( !x.IsPublic ) return false;
		if ( !x.IsProperty ) return false;

		var browsable = !x.HasAttribute<HideAttribute>() && !x.HasAttribute<HideInEditorAttribute>();
		return browsable;
	}

	protected override void DoLayout()
	{
		if ( Parent is not ComponentSheet { Header: var header } cs )
		{
			return;
		}

		//
		// Make it look like part of the object, like the transform header.
		//

		header.Color = InspectorHeader.HeaderColor;

		var toggle = header.GetDescendants<BoolControlWidget>().First();
		toggle.SetEffectOpacity( 0 );
		toggle.Enabled = false;
	}
}
