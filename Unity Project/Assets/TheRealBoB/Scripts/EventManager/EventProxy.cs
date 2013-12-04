using System;

public class EventProxy
{
	// define signature for methods
	public delegate void EventProxyHandler(object sender, System.EventArgs args);
	public event EventProxyHandler EventFired;
	
	public void FireEvent(object sender, System.EventArgs e)
	{
		if (EventFired != null)
			EventFired(sender, e);
	}
}

