using System;

public class EventProxy
{
	// define signature for methods
	public delegate void EventProxyHandler(object sender, EventProxyArgs args);
	public event EventProxyHandler EventFired;

	public void FireEvent(object sender, EventProxyArgs e)
	{
		if (EventFired != null)
			EventFired(sender, e);
	}
}

