using System;
using System.Collections.Generic;

public class EventProxyManager
{
	#region singelton
	static EventProxyManager instance;
	static EventProxyManager Instance {
		get {
			if(instance == null)
				instance = new EventProxyManager();
			return instance;
		}
	}
	private EventProxyManager() {}
	#endregion

	Dictionary<EventName, EventProxy> proxyDict = new Dictionary<EventName, EventProxy>();

    #region external
	public static void FireEvent(object sender, EventProxyArgs args)
    {
        Instance._FireEvent(sender, args);
    }

    public static void RegisterForEvent(EventName name, EventProxy.EventProxyHandler handler)
    {
        Instance._RegisterForEvent(name, handler);
    }

    public static void Clear()
    {
        Instance._Clear();
    }

    #endregion

    #region internal
	void _FireEvent(object sender, EventProxyArgs args)
	{
		if (proxyDict.ContainsKey(args.name))
			proxyDict[args.name].FireEvent(sender, args);
		// else
		// fire error event
	}

	void _RegisterForEvent(EventName name, EventProxy.EventProxyHandler handler) 
	{
		if (!proxyDict.ContainsKey(name))
			proxyDict.Add(name, new EventProxy());

		proxyDict[name].EventFired += handler;
    }

    private void _Clear()
    {
        proxyDict.Clear();
    }
    #endregion
}

public enum EventName {
	DefaultEvent, // just a placehoder for EventProxyArgs
	Initialized,
	UnitSpawned,
	UnitActivated,
	UnitMoved,
	UnitAttacked,
	UnitDied,
	BMapTileTapped,
    RoundSetup,
	TurnStarted,
	Gameover,
	EventDone
}
