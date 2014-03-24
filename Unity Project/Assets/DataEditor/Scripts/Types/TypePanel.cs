using UnityEngine;
using System.Collections.Generic;

namespace GameDataUI {
	public class TypePanel : MonoBehaviour {

		public GameObject typeInputPrefab;
		public GameObject addTypeButton;
		public float columnWidth = 420f;
		public float rowHeight = 170f;
		
		int columns = 3;
		
		List<TypeInput> typeInputs;
		
		public void Init(Type[] types) 
		{
			typeInputs = new List<TypeInput>();
			
			for (int i = 0; i < types.Length; i++) {
				Add(types[i]);
			}
			
			UpdatePositions();
		}
		
		void UpdatePositions() 
		{
			for (int i = 0; i < typeInputs.Count; i++) {
				typeInputs[i].gameObject.name = "Type " + i;
				typeInputs[i].gameObject.transform.localPosition = new Vector3((i%columns) * columnWidth,-(i/columns) * rowHeight,0f);
				typeInputs[i].gameObject.transform.localScale = Vector3.one;
			}
			addTypeButton.transform.localPosition = new Vector3((typeInputs.Count%columns) * columnWidth,-(typeInputs.Count/columns) * rowHeight,0f);
		}

		public void Refresh()
		{
			foreach (TypeInput input in typeInputs) {
				input.Refresh();
			}
		}

		public void Add() 
		{
			Add (new Type());
		}
		
		public void Add(Type type) 
		{
			GameObject handle = (GameObject) Instantiate(typeInputPrefab);
			handle.transform.parent = this.transform;
			TypeInput typeInput = handle.GetComponent<TypeInput>();
			typeInput.Init(type, this);
			typeInputs.Add(typeInput);
			
			UpdatePositions();
		}
		
		public void Remove(TypeInput typeInput) 
		{
			typeInputs.Remove(typeInput);
			UpdatePositions();
		}
		
		public void Save()
		{
			Type[] types = new Type[typeInputs.Count];
			for (int i = 0; i < typeInputs.Count; i++) {
				types[i] = typeInputs[i].GetTypeData();
			}
			
			GameData.ClearTypes();
			GameData.AddTypeArray(types);
			Debug.Log("Updated all the types!!!");
		}
		
	}
}
