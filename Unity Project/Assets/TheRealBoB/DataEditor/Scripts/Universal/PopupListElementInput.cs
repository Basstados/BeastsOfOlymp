using UnityEngine;
using System.Collections.Generic;

namespace GameDataUI {
	public class PopupListElementInput : MonoBehaviour, IInputListElement {

		public UIPopupList popupInput;

		IInputListParent parent;

		public string value {
			get {
				return popupInput.value;
			}
			set {
				popupInput.value = value;
			}
		}

		public void Init(string name, string[] options, IInputListParent parent) 
		{
			UpdateOptions(options);
			this.value = name;
			this.parent = parent;
		}

		public void UpdateOptions(string[] options)
		{
			popupInput.items = new List<string>(options);
		}

		public void Delete()
		{
			parent.RemoveListElement(this);
			Destroy(this.gameObject);
		}
	}
}
