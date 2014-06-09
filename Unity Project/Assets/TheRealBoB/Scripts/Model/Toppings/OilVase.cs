using System;

public class OilVase : Topping
{
	
	/// <summary>
	/// Constructor.
	/// Inistalize this instance.
	/// </summary>
	public OilVase() : base()
	{
		// How many round will this topping life?
		duration = -1; // this one will stay forever (or till destroyed)
		// Does units must move around this topping?
		isObstacle = true;
		isTargetable = true; // this is a obstacle with an OnAttackEffect
		isLinked = false;
		// What is the name of the Prefab to be instantiate? (without ".prefab")
		prefabName = "OilVase"; // Prefab path is: Asset/Resources/Toppings/
	}
	
	/// <summary>
	/// The effect that will happen, if a unit stands on the field with this topping.
	/// Deal damage each turn to the unit on this burning oil.
	/// </summary>
	/// <param name="mapTile">The mapTile the obstacle and the unit stand on.</param>
	/// <param name="unit">The unit which will affected by whatever will happen.</param>
	public override void OnStayEffect(Unit unit)
	{
		// its an obstacle, so this won't be called anyway
	}
	
	/// <summary>
	/// The effect that will happen, if this field will be hit by an attack.
	/// On Attack, this one will be destroyed and oilField will be placed on all empty Field around (inkl. this one)
	/// </summary>
	/// <param name="mapTile">The mapTile the obstacle and the unit stand on.</param>
	/// <param name="attack">The attack which triggered this effect.</param>
	public override void OnAttackEffect(Attack attack, Model model)
	{	
		// remove this topping
		Destroy();

		// these 8 vectors have the relative coordinates of all 8 neighbour fields (inkluding diagonal)
		Vector[] neighbours = new Vector[]{new Vector(1,0),new Vector(1,1), new Vector(0,1), 
				new Vector(-1,1), new Vector(-1,0), new Vector(-1,-1),
				new Vector(0,-1), new Vector(1,-1), new Vector(0,0)};

		// calculate (vector.x + maptile.x, vector.y + maptile.y) to convert the relative coordinates into absolute once
		// now iterate over all listes fields
		// and add an OilField to all free fields
		foreach(Vector vec in neighbours) {
			// calculate absolute coordinates as described before
			Vector vecAbsolute = new Vector(vec.x + mapTile.x, vec.y + mapTile.y);

			// first check if the new point is still on grid
			// this should be done allways to make sure, we don't juse any unvalid coordiantes 
			if(model.IsPointOnGrid(vecAbsolute)) {
				// now we are sure, our absolute coordinates are valid (on the grid)
				// this is the way to access ANY mapTile if the absolute coordinates are given
				MapTile neighbourMaptile = model.mapTiles[vecAbsolute.x][vecAbsolute.y];

				// if the seleced mapTile has no topping jet, it will now get an OilField
				if(neighbourMaptile.topping == null) {
					// add the new topping
					neighbourMaptile.topping = new OilField();
					// don't forget to call respawn after placeing a new topping
					neighbourMaptile.topping.Spawn(neighbourMaptile);
				}
			}
		}


	}
}


