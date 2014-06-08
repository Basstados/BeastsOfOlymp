using System.Collections;
using UnityEngine;

public abstract class Topping {

    // Is this topping blocking the field?
    protected bool isObstacle = false;
    public bool IsObstacle { get { return isObstacle; } }
    // How many turn will this topping persist?
    protected int duration = -1; // duration < 0 leads to infinit stay
    // The name of the prefab that will be instantiate on the topping position
    public string prefabName;

    public MapTile mapTile;

    public Topping() 
    {
        EventProxyManager.RegisterForEvent(EventName.RoundSetup, HandleRoundSetup);
    }

    /// <summary>
    /// Place this instance on a MapTile.
    /// </summary>
    /// <param name="mapTile">The parent MapTile where to spawn</param>
    public void Spawn(MapTile mapTile)
    {
        this.mapTile = mapTile;
        EventProxyManager.FireEvent(this, new ToppingSpawnEvent(this));
    }

    /// <summary>
    /// Remove this Topping from the game.
    /// </summary>
    public void Destroy()
    {
        this.mapTile.topping = null;
        EventProxyManager.FireEvent(this, new ToppingDestroyedEvent(this));
    }

    /// <summary>
    /// Count the duration down to 0; If duration == 0 remove this obstacle;
    /// If duration is < 0 this instance will stay forever
    /// </summary>
    private void HandleRoundSetup(object sender, EventProxyArgs args)
    {
        if (duration > 0) duration--;
        if (duration == 0) Destroy();// remove this topping
    }

	/// <summary>
	/// The effect that will happen, if a unit stands on the field with this topping.
	/// </summary>
	/// <param name="mapTile">The mapTile the obstacle and the unit stand on.</param>
	/// <param name="unit">The unit wich will effected by what ever will happen.</param>
	public abstract void OnStayEffect(Unit unit);
	/// <summary>
	/// The effect hat will happen, if this field will be hit by an attack.
	/// </summary>
	/// <param name="mapTile">The mapTile the obstacle and the unit stand on.</param>
	/// <param name="attack">The attack wich triggerd this effect.</param>
	public abstract void OnAttackEffect(Attack attack, Model model);
}

public class ToppingSpawnEvent : EventProxyArgs
{
    public Topping target;

    public ToppingSpawnEvent(Topping topping)
    {
        this.name = EventName.ToppingSpawned;
        this.target = topping;
    }
}

public class ToppingDestroyedEvent : EventProxyArgs
{
    public Topping target;

    public ToppingDestroyedEvent(Topping topping)
    {
        this.name = EventName.ToppingDestroyed;
        this.target = topping;
    }
}