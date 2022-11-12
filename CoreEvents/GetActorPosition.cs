using Lune.CoreTypes;
using Lune.Exceptions;

namespace Lune.CoreEvents;

[LibraryEvent( "Actor", "Get Actor Position",
	Description = "Outputs actor position as a Vector3" )]
public class GetActorPosition : Event
{
	public override string Name => "Get Actor Position";

	[EventIO] public EventInput<Actor> Actor { get; set; } = new();
	[EventIO] public EventOutput<Vector3> Position { get; set; } = new();

	public GetActorPosition()
	{
		Position.DataAction = () =>
		{
			if ( Actor.Data == null )
			{
				throw new EventPropertyException( Actor );
			}

			Position.Data = new Vector3(
				Actor.Data.Position.x, Actor.Data.Position.y, Actor.Data.Position.z );
		};
	}
}
