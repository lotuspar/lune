using Lune.CoreTypes;
using Lune.Exceptions;

namespace Lune.CoreEvents;

[LibraryEvent( "Core" )]
public class GetActorPosition : Event
{
	public override string Name => "Get Actor Position";
	[EventIO] public EventInput<Actor> Actor { get; set; }
	[EventIO] public EventOutput<Vector3> Position { get; set; }

	public override void UpdateOutput( IEventOutput output )
	{
		base.UpdateOutput( output );

		if ( output != Position )
		{
			return;
		}

		if ( Actor.Data == null )
		{
			throw new EventPropertyException( Actor );
		}

		Position.Data = new Vector3(
			Actor.Data.Position.x, Actor.Data.Position.y, Actor.Data.Position.z );
	}
}
