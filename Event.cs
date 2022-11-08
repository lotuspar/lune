using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace Lune;

public interface IEventIO
{
	public string Name { get; set; }
	public Type Type { get; }
	public object Object { get; }
}

public interface IEventInput : IEventIO
{
}

public interface IEventOutput : IEventIO
{
}

/// <summary>
/// Wraps a type for use as an <see cref="Event"/> output
/// </summary>
/// <typeparam name="T">Wrapped type</typeparam>
public class EventOutput<T> : IEventOutput
{
	public string Name { get; set; }
	public object Object { get; set; }
	public Type Type => typeof(T);

	public T Data
	{
		get => (T)Object;
		set => Object = value;
	}

	public EventOutput() { }

	public EventOutput( T initial )
	{
		Data = initial;
	}

	public static implicit operator T( EventOutput<T> e ) => e.Data;
}

/// <summary>
/// Wraps a type for use as an <see cref="Event"/> input
/// </summary>
/// <typeparam name="T">Wrapped type</typeparam>
public class EventInput<T> : IEventInput
{
	public string Name { get; set; }
	public object Object => From?.Object ?? PreOutputData;
	public T Data => (T)From?.Object ?? PreOutputData;
	public Type Type => typeof(T);

	/// <summary>
	/// Data used when output is still null
	/// </summary>
	public T PreOutputData = default(T);

	/// <summary>
	/// Event to read from (this input from their output)
	/// </summary>
	public EventOutput<T> From;

	public EventInput() { }

	public EventInput( EventOutput<T> initial )
	{
		From = initial;
	}

	public EventInput( IEventOutput initial )
	{
		if ( initial is not EventOutput<T> instance )
		{
			throw new ArgumentException( $"Provided IEventOutput isn't EventOutput<{typeof(T).Name}>" );
		}

		From = instance;
	}

	public EventInput( T initial )
	{
		PreOutputData = initial;
	}

	public static implicit operator T( EventInput<T> e ) => e.Data;
}

[System.AttributeUsage( AttributeTargets.Property )]
public class EventIOAttribute : System.Attribute
{
}

/// <summary>
/// Single event "node"
/// </summary>
public class Event
{
	public virtual string Name => GetType().Name;

	public List<Event> Next;

	public List<IEventOutput> Outputs;
	public List<IEventInput> Inputs;

	public int TicksStart { get; set; }
	public int TicksDuration { get; set; }

	public virtual void Start() { }
	public virtual void Tick() { }
	public virtual void End() { }
	public virtual void UpdateOutput( IEventOutput output ) { }

	protected Event() => RegisterEventInputOutput();

	protected bool RegistrationFinished { get; private set; }

	/// <summary>
	/// Register all properties with <see cref="EventIOAttribute"/>.
	/// </summary>
	/// <exception cref="InvalidCastException">If property isn't based on IEventOutput or IEventInput</exception>
	protected void RegisterEventInputOutput()
	{
		if ( !RegistrationFinished )
		{
			return;
		}

		foreach ( var propertyDescription in TypeLibrary.GetPropertyDescriptions( this ) )
		{
			if ( propertyDescription.GetCustomAttribute<EventIOAttribute>() == null )
			{
				return;
			}

			var value = propertyDescription.GetValue( this );
			switch ( value )
			{
				case IEventOutput output:
					Outputs.Add( output );
					break;
				case IEventInput input:
					Inputs.Add( input );
					break;
				default:
					throw new InvalidCastException( "EventIO isn't an Output or Input" );
			}
		}

		RegistrationFinished = true;
	}
}
