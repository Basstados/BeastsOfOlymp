using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(BMap))]
public class EditorBMap : Editor {

	 public override void OnInspectorGUI ()
	{
		BMap myTarget = (BMap) target;

		DrawDefaultInspector();
		if(GUILayout.Button("Inistantiate Map"))
			myTarget.InstantiateMap();
	}
}
