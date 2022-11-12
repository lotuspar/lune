using System;
using Lune.CoreTypes;
using Lune.Exceptions;

namespace Lune.CoreEvents;

[LibraryEvent( "Actor", "Set Actor Position",
	Description = "Set actor position using a Vector3 input" )]
public class SetActorPosition : Event
{
	public override string Name => "Set Actor Position";
	[EventIO] public EventInput<Vector3> Position { get; set; }
	[EventIO] public EventInput<Actor> Actor { get; set; }

	public override void Start()
	{
		base.Start();

		if ( Actor.Data == null )
		{
			throw new EventPropertyException( Actor );
		}

		Actor.Data.Position = Position;
	}
}
