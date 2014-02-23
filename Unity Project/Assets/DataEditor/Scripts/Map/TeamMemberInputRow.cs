using UnityEngine;
using System.Collections.Generic;

namespace GameDataUI {
	public class TeamMemberInputRow : MonoBehaviour, IInputListElement {

		public UIPopupList popupInput;
		public UIInput xInput;
		public UIInput yInput;

		public string name {
			get {
				return popupInput.value;
			}
			set {
				popupInput.value = value;
			}
		}

		public int x {
			get {
				return int.Parse(xInput.value);
			}
			set {
				xInput.value = value.ToString();
			}
		}

		public int y {
			get {
				return int.Parse(yInput.value);
			}
			set {
				yInput.value = value.ToString();
			}
		}

		IInputListParent parent;
		
		public void Init(string name, int x, int y, string[] options, IInputListParent parent) 
		{
			UpdateOptions(options);
			this.name = name;
			this.x = x;
			this.y = y;
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