namespace Sandbox;

/// <summary>
/// If a component is marked with this, then only one of that type of component
/// will be allowed. When adding a component, it'll automatically replace any others.
/// </summary>
public interface ISingletonComponent;
