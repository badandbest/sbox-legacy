using Sandbox;
using System.Text.Json.Serialization;

namespace Legacy;

public interface IComponent
{
	/// <summary>
	/// Whether this component is enabled or not.
	/// </summary>
	bool Enabled { get; set; }

	/// <summary>
	/// True if this component only exists on the client.
	/// </summary>
	[JsonIgnore, Hide]
	bool IsClientOnly { get; }

	/// <summary>
	/// True if this component only exists on the server.
	/// </summary>
	[JsonIgnore, Hide]
	bool IsServerOnly { get; }

	/// <summary>
	/// Name of the component, will be shown in entity inspector.
	/// </summary>
	string Name { get; set; }
}
