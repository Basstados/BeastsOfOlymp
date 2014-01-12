using System.Collections;

public class skrEventProxy {

	#region singelton
	static skrEventProxy instance;
	public static skrEventProxy Instance {
		get {
			if(instance == null)
				instance = new skrEventProxy();
			return instance;
		}
	}
	#endregion

	// define signature for methods
	public delegate void EventProxyHandler(string name, object sender, System.EventArgs args);
	public event EventProxyHandler EventFired;
	void OnEventFired(string name, object sender, System.EventArgs args) 
	{
		if (EventFired != null)
			EventFired (name, sender, args);
	}

	public void FireEvent(string name, object sender, System.EventArgs args)
	{
		OnEventFired (name, sender, args);
	}

	void Start()
	{
		// use proxy to fire event
		skrEventProxy.Instance.FireEvent("GameOver", this, new skrUnitMovedEvent(null,null,null));

		// register
		skrEventProxy.Instance.EventFired += HandleEventFired;
	}

	void HandleEventFired( string name, object sender, System.EventArgs args) 
	{

	}
}
