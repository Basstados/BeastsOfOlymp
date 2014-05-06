using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Attack))]
public class EditorAttack : Editor {

	Vector areaCenter = new Vector(2,2);
	GUIStyle hAoEstyle = new GUIStyle();

	public EditorAttack ()
	{
		hAoEstyle.fixedWidth = 5 * 15f;
		hAoEstyle.fixedHeight = 15f;
	}
	

	public override void OnInspectorGUI ()
	{
		Attack myTarget = (Attack) target;
		string oldName = myTarget.attackName; // save old name to rename asset later
		myTarget.attackName 	= EditorGUILayout.TextField("Name",myTarget.attackName);
		EditorGUILayout.Separator();
		myTarget.element			= (Element) EditorGUILayout.ObjectField("Type",myTarget.element,typeof(Element),false);
		myTarget.damage 		= EditorGUILayout.IntField("Damage",myTarget.damage);
		myTarget.hitChance		= EditorGUILayout.Slider("Hit Chance", myTarget.hitChance, 0f, 1f);
		myTarget.range			= EditorGUILayout.IntField("Range",myTarget.range);

		bool[,] area = new bool[5,5];
		if(myTarget.area != null) {
			foreach (Vector vec in myTarget.area) {
				// subtract areaCenter since the origin of the AoE (0,0) should be in the center of our toggle0-fields
				area[vec.x + areaCenter.x, areaCenter.y - vec.y] = true;
			}
		}

		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("Area of effect");
		EditorGUILayout.BeginVertical();
		for (int j = 0; j < 5; j++) {
			EditorGUILayout.BeginHorizontal(hAoEstyle);
			for (int i = 0; i < 5; i++) {
				area[i,j] = EditorGUILayout.Toggle(area[i,j]);
			}
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndVertical();

		List<Vector> areaFields = new List<Vector>();
		// the default direction or "forward" is the vector (0,1) = "up"
		// 1st: subtract areaCenter to get a vector relative to the center
		// 2nd: invert y direction such that (0,1) is above the center and not below
		for (int i = 0; i < area.GetLength(0); i++) {
			for (int j = 0; j < area.GetLength(1); j++) {
				if(area[i,j]) areaFields.Add(new Vector(areaCenter.x - i,j - areaCenter.y));
			}
		}
		myTarget.area = areaFields.ToArray();

		// you need this to save changes in the custom inspector permanent in the .asset file
		EditorUtility.SetDirty(target);

		// keep attackName and asset name equal
		AssetDatabase.RenameAsset("Assets/Database/Attacks/" + oldName + ".asset", myTarget.attackName);
	}
}
