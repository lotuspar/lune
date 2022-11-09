using Sandbox;

namespace Lune.CoreTypes;

/// <summary>
/// Actor for a Lune <see cref="Scene"/>
/// </summary>
public class Actor
{
	public string Name { get; set; }

	public Vector3 Position
	{
		get => Model.Position;
		set => Model.Position = value;
	}

	public Rotation Rotation
	{
		get => Model.Rotation;
		set => Model.Rotation = value;
	}

	public readonly SceneModel Model;

	public Actor( SceneModel model )
	{
		Model = model;
	}
}
