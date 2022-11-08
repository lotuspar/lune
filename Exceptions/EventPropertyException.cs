using System;

namespace Lune.Exceptions;

/// <summary>
/// Exception for <see cref="Event"/> IO property issues
/// </summary>
public class EventPropertyException : Exception
{
	public EventPropertyException()
	{
	}

	public EventPropertyException( string property, string message = "Property not provided" )
		: base( $"{property}: {message}" )
	{
	}

	public EventPropertyException( IEventIO io, string message = "Property not provided" )
		: base( $"{io.Name}: {message}" )
	{
	}

	public EventPropertyException( string property, string message, Exception inner )
		: base( $"{property}: {message}", inner )
	{
	}
}
