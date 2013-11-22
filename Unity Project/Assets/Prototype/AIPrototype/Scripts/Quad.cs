using UnityEngine;
using System.Collections;

public class Quad {

	public Vector2 position;
	public int penalty;
	
	// the gameobject this quad instace does represent
	private GameObject gameobject;
	
	private Material defaultMaterial;
	
	public Quad(Vector2 pos, int pen, GameObject go) {
		position = pos;
		penalty = pen;
		gameobject = go;
		defaultMaterial = go.renderer.material;
	}
	
	public void ChangeMaterial( Material newMat ) {
		gameobject.renderer.material = newMat;
	}
	
	public void ResetMaterial() {
		gameobject.renderer.material = defaultMaterial;
	}
	
	public override string ToString ()
	{
		return "Quad: (" + (int) position.x + ", " + (int) position.y + ")  penalty = " + penalty;
	}
}
