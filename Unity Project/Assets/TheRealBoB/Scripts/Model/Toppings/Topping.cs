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

    public Topping(MapTile mapTile) : base()
    {
        this.mapTile = mapTile;
    }

    /// <summary>
    /// Count the duration down to 0; If duration == 0 remove this obstacle
    /// </summary>
    private void HandleRoundSetup(object sender, EventProxyArgs args)
    {
        Debug.Log("Duration is now: " + duration);
        if (duration > 0) duration--;
        if (duration == 0) mapTile.topping = null; // remove this topping
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
	public abstract void OnAttackEffect(Attack attack);
}
