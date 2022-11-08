using System.Collections.Generic;

namespace Lune;

public class Scene
{
	public int TicksCurrent;
	public int TicksLength;
	public int TicksPerSecond = 1000;

	/// <summary>
	/// List of all events
	/// </summary>
	public List<Event> Events;

	/// <summary>
	/// Cache for events without parents
	/// </summary>
	private List<Event> _rootEventCache;

	/// <summary>
	/// Cache for currently active events
	/// </summary>
	private List<Event> _activeEventCache;

	public void CacheEvents()
	{
		_rootEventCache.Clear();
		foreach ( var @event in Events )
		{
			if ( @event.Previous == null )
				_rootEventCache.Add( @event );
		}
	}

	public void Reset()
	{
		foreach ( var @event in Events )
		{
			@event.EventSimulated = false;
		}

		_activeEventCache.Clear();

		CacheEvents();
	}

	public void Handle()
	{
		// first, go through root events to find ones that should be started
		foreach ( var @event in _rootEventCache )
		{
			if ( TicksCurrent <= @event.TicksStart )
			{
				continue;
			}

			// event should be active
			// check if it's already been used / simulated...
			if ( @event.EventSimulated )
			{
				continue;
			}

			// add to active events
			_activeEventCache.Add( @event );
			@event.Start();
		}

		// go through active events to find ones that should tick or end
		for ( var i = _activeEventCache.Count - 1; i >= 0; i-- )
		{
			var @event = _activeEventCache[i];
			if ( TicksCurrent > @event.TicksStart + @event.TicksDuration )
			{
				// event should end!
				@event.End();
				_activeEventCache.RemoveAt( i );
				continue; // continue so the event doesn't skip
			}

			// event should tick normally
			@event.Tick();
		}
	}
}
