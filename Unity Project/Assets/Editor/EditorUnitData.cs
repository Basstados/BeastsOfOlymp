using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(UnitData))]
public class EditorUnitData : Editor {

	public override void OnInspectorGUI ()
	{
		GUIStyle xButton = new GUIStyle(GUI.skin.button);
		xButton.fixedWidth = 25f;

		UnitData myTarget = (UnitData) target;

		// name
		string oldName = myTarget.unitName; // save old name rename asset later
		myTarget.unitName 			= EditorGUILayout.TextField("Name", myTarget.unitName);
		EditorGUILayout.Separator();
		// base states
		myTarget.element 			= (Element) EditorGUILayout.ObjectField("Element", myTarget.element, typeof(Element));
		myTarget.baseHealth 		= EditorGUILayout.IntField("Base Health", myTarget.baseHealth);
		myTarget.baseAttack 		= EditorGUILayout.IntField("Base Attack", myTarget.baseAttack);
		myTarget.baseInitiative 	= EditorGUILayout.IntField("Base Initiative", myTarget.baseInitiative);
		myTarget.baseMoveRange 		= EditorGUILayout.IntField("Base Move Range", myTarget.baseMoveRange);
		EditorGUILayout.Separator();

		EditorGUILayout.LabelField("Attacks");
		if(myTarget.attacks != null) {
			for (int i = 0; i < myTarget.attacks.Length; i++) {
				EditorGUILayout.BeginHorizontal();
				myTarget.attacks[i] = (Attack) EditorGUILayout.ObjectField(myTarget.attacks[i], typeof(Attack));
				if(GUILayout.Button("x", xButton))
					RemoveFromArray<Attack>(ref myTarget.attacks, i);
				EditorGUILayout.EndHorizontal();
			}
		}
		if(GUILayout.Button("Add attack"))
			IncreaseArraySize<Attack>(ref myTarget.attacks);

		// sync asset name and unitName
		AssetDatabase.RenameAsset("Assets/Database/Units/" + oldName + ".asset", myTarget.unitName);
	}

	/// <summary>
	/// Increases the size of the array by adding an empty entry at the end.
	/// </summary>
	/// <param name="array">Array.</param>
	/// <typeparam name="T">The type of the array.</typeparam>
	private void IncreaseArraySize<T>(ref T[] array)
	{
		T[] newArray = new T[1];
		if(array != null) {
			newArray = new T[array.Length + 1];
			for (int i = 0; i < array.Length; i++) {
				newArray[i] = array[i];
			}
		} 
		array = newArray;
	}
	
	/// <summary>
	/// Removes entry with given index from array.
	/// </summary>
	/// <param name="array">Array.</param>
	/// <param name="index">Index of the element to remove.</param>
	/// <typeparam name="T">The type of the array.</typeparam>
	private void RemoveFromArray<T>(ref T[] array, int index)
	{
		List<T> list = new List<T>(array);
		list.RemoveAt(index);
		array = list.ToArray();
	}
}
