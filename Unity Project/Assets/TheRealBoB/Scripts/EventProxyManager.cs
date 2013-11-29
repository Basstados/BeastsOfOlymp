using System;
using System.Collections.Generic;

public class EventProxyManager
{
	#region singelton
	static EventProxyManager instance;
	public static EventProxyManager Instance {
		get {
			if(instance == null)
				instance = new EventProxyManager();
			return instance;
		}
	}
	private EventProxyManager() {}
	#endregion

	Dictionary<EventName, EventProxy> proxyDict = new Dictionary<EventName, EventProxy>();

	public void FireEvent(EventName name, object sender, EventArgs args)
	{
		if (proxyDict.ContainsKey(name))
			proxyDict[name].FireEvent(sender, args);
		// else
		// fire error event
	}

	public void RegisterForEvent(EventName name, EventProxy.EventProxyHandler handler) 
	{
		if (!proxyDict.ContainsKey(name))
			proxyDict.Add(name, new EventProxy());

		proxyDict[name].EventFired += handler;
	}

	/**
	 * ATTANTION! Need to clear dictionary on Scene change
	 */
}

public enum EventName {
	Initialized,
	UnitSpawned,
	UnitMoved,
	MapTileTapped
}
