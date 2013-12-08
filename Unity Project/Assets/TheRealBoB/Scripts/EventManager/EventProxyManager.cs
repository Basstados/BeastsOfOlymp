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
    public static void FireEvent(EventName name, object sender, EventArgs args)
    {
        Instance._FireEvent(name, sender, args);
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
    void _FireEvent(EventName name, object sender, EventArgs args)
	{
		if (proxyDict.ContainsKey(name))
			proxyDict[name].FireEvent(sender, args);
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
	Initialized,
	UnitSpawned,
	UnitActivated,
	UnitMoved,
	UnitAttacked,
	UnitDied,
	BMapTileTapped,
    RoundSetup,
	TurnStarted,
	Gameover
}
