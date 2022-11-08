﻿using System;
using Lune.CoreTypes;
using Lune.Exceptions;

namespace Lune.CoreEvents.Creator;

public class CreateVector3 : Event
{
	public override string Name => "Vector3";
	[EventIO] public EventInput<float> X { get; set; } = new(0);
	[EventIO] public EventInput<float> Y { get; set; } = new(0);
	[EventIO] public EventInput<float> Z { get; set; } = new(0);
	[EventIO] public EventOutput<Vector3> Result { get; set; } = new();

	public override void UpdateOutput( IEventOutput output )
	{
		base.UpdateOutput( output );

		if ( output == Result )
		{
			Result.Data = new Vector3( X, Y, Z );
		}
	}
}
