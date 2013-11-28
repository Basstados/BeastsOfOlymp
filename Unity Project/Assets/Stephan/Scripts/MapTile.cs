using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MapTile {

	public int penalty = 1;
	// properties will not be serialized by unity
	public skrUnit unit {get; set;}
}
