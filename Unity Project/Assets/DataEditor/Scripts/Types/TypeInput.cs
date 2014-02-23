using UnityEngine;
using System.Collections.Generic;

namespace GameDataUI {
	public class TypeInput : MonoBehaviour, IInputListParent {

		public GameObject popupInputPrefab;
		public UIInput nameInput;
		public GameObject weaknessesAnchor;
		public GameObject addWeaknessButton;
		public GameObject strengthAnchor;
		public GameObject addStrengthButton;
		public float rowHeight;

		TypePanel parent;
		List<PopupListElementInput> weaknesses = new List<PopupListElementInput>();
		List<PopupListElementInput> strengths = new List<PopupListElementInput>();

		public void Init(Type type, TypePanel parent) 
		{
			this.parent = parent;

			if(type.name != null)
				nameInput.value = type.name;

			if(type.weaknesses != null)
				foreach(string weakness in type.weaknesses) {
					AddWeakness(Database.GetType(weakness));
				}

			if(type.strengths != null)
				foreach(string strength in type.strengths) {
					AddStrength(Database.GetType(strength));
				}
		}

		public void AddWeakness()
		{
			AddWeakness(new Type());
		}

		public void AddWeakness(Type type)
		{
			AddPopup(type, weaknessesAnchor, weaknesses);
		}

		public void AddStrength()
		{
			AddStrength(new Type());
		}

		public void AddStrength(Type type)
		{
			AddPopup(type, strengthAnchor, strengths);
		}

		void AddPopup(Type type, GameObject anchor, List<PopupListElementInput> list)
		{
			string[] options = GetTypeArray();
			GameObject handle = (GameObject) Instantiate(popupInputPrefab);
			handle.transform.parent = anchor.transform;
			
			PopupListElementInput element = handle.GetComponent<PopupListElementInput>();
			element.Init (type.name, options, this);
			list.Add(element);

			UpdatePositions();
		}

		void UpdatePositions()
		{
			for (int i = 0; i < weaknesses.Count; i++) {
				weaknesses[i].gameObject.name = "Weakness " + i;
				weaknesses[i].gameObject.transform.localPosition = new Vector3(0,-i * rowHeight,0f);
				weaknesses[i].gameObject.transform.localScale = Vector3.one;
			}
			addWeaknessButton.transform.localPosition = new Vector3(0,- weaknesses.Count * rowHeight,0f);

			for (int i = 0; i < strengths.Count; i++) {
				strengths[i].gameObject.name = "Strength " + i;
				strengths[i].gameObject.transform.localPosition = new Vector3(0,-i * rowHeight,0f);
				strengths[i].gameObject.transform.localScale = Vector3.one;
			}
			addStrengthButton.transform.localPosition = new Vector3(0,- strengths.Count * rowHeight,0f);
		}

		string[] GetTypeArray()
		{
			string[] options = new string[Database.GetTypes().Length];
			for (int i = 0; i < options.Length; i++) {
				options[i] = Database.GetTypes()[i].name;
			}
			return options;
		}

		public void Refresh()
		{
			string[] options = GetTypeArray();

			foreach (PopupListElementInput popup in weaknesses) {
				popup.UpdateOptions(options);
			}
			foreach (PopupListElementInput popup in strengths) {
				popup.UpdateOptions(options);
			}
		}

		public void RemoveListElement (IInputListElement child)
		{
			strengths.Remove((PopupListElementInput) child);
			weaknesses.Remove((PopupListElementInput) child);
			UpdatePositions();
		}

		public void Delete()
		{
			parent.Remove(this);
			Destroy(this.gameObject);
		}

		public Type GetTypeData()
		{
			Type result = new Type();
			result.name = nameInput.value;
			result.weaknesses = new string[weaknesses.Count];
			for (int i = 0; i < weaknesses.Count; i++) {
				result.weaknesses[i] = weaknesses[i].value;
			}
			result.strengths = new string[strengths.Count];
			for (int i = 0; i < strengths.Count; i++) {
				result.strengths[i] = strengths[i].value;
			}

			return result;
		}
	}
}