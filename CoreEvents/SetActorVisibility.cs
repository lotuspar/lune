using System;
using Lune.CoreTypes;
using Lune.Exceptions;

namespace Lune.CoreEvents;

[LibraryEvent( "Core" )]
public class SetActorVisibility : Event
{
	public override string Name => "Set Actor Visibility";
	[EventIO] public EventInput<bool> IsVisible { get; set; }
	[EventIO] public EventInput<Actor> Actor { get; set; }

	public override void Start()
	{
		base.Start();

		if ( Actor.Data == null )
		{
			throw new EventPropertyException( Actor );
		}

		Actor.Data.Model.RenderingEnabled = IsVisible?.Data ?? true;
	}
}
