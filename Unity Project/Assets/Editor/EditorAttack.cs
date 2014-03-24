//using UnityEngine;
//using UnityEditor;
//using System.Collections.Generic;
//
//[CustomEditor(typeof(Attack))]
//public class EditorAttack : Editor {
//
//	Vector areaCenter = new Vector(2,2);
//	GUIStyle hAoEstyle = new GUIStyle();
//
//	public EditorAttack ()
//	{
//		hAoEstyle.fixedWidth = 5 * 15f;
//		hAoEstyle.fixedHeight = 15f;
//	}
//	
//
////	public override void OnInspectorGUI ()
////	{
////		Attack myTarget = (Attack) target;
////		string oldName = myTarget.attackName;
////		myTarget.attackName 	= EditorGUILayout.TextField("Name",myTarget.attackName);
////		EditorGUILayout.Separator();
////		myTarget.type			= (Type) EditorGUILayout.ObjectField("Type",myTarget.type,typeof(Type),false);
////		myTarget.damage 		= EditorGUILayout.IntField("Damage",myTarget.damage);
////		myTarget.hitChance		= EditorGUILayout.Slider("Hit Chance", myTarget.hitChance, 0f, 1f);
////		myTarget.range			= EditorGUILayout.IntField("Range",myTarget.range);
////
////		bool[,] area = new bool[5,5];
////		foreach (Vector vec in myTarget.area) {
////			area[vec.x + areaCenter.x, vec.y + areaCenter.y] = true;
////		}
////
////		EditorGUILayout.Separator();
////		EditorGUILayout.LabelField("Area of effect");
////		EditorGUILayout.BeginVertical();
////		for (int i = 0; i < 5; i++) {
////			EditorGUILayout.BeginHorizontal(hAoEstyle);
////			for (int j = 0; j < 5; j++) {
////				area[i,j] = EditorGUILayout.Toggle(area[i,j]);
////			}
////			EditorGUILayout.EndHorizontal();
////		}
////		EditorGUILayout.EndVertical();
////
////		List<Vector> areaFields = new List<Vector>();
////		for (int i = 0; i < area.GetLength(0); i++) {
////			for (int j = 0; j < area.GetLength(1); j++) {
////				if(area[i,j]) areaFields.Add(new Vector(i - areaCenter.x,j - areaCenter.y));
////			}
////		}
////		myTarget.area = areaFields.ToArray();
////
////		AssetDatabase.RenameAsset("Assets/Database/Attacks/" + oldName + ".asset", myTarget.attackName);
//	}
//
//
//
//}
