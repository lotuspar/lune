using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
	/// List of all actors
	/// </summary>
	private List<Event> _actors;

	/// <summary>
	/// Read-only actors list 
	/// </summary>
	public ReadOnlyCollection<Event> Actors => _actors.AsReadOnly();

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
		foreach ( var @event in Events.Where( @event => @event.Previous == null ) )
		{
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
		if ( _rootEventCache.Count == 0 )
		{
			CacheEvents();
		}

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

			if ( TicksCurrent < @event.TicksStart )
			{
				// event shouldn't even be active...
				// reset back to before being simulated
				@event.EventSimulated = false;
				_activeEventCache.RemoveAt( i ); // and remove it from active events
				continue;
			}

			// event should tick normally
			@event.Tick();
		}
	}
}
