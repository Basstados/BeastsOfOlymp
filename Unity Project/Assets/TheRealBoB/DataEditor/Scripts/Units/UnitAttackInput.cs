using UnityEngine;
using System.Collections;

public class UnitAttackInput : MonoBehaviour {

	public UIInput attackNameInput;

	UnitDataInput parent;

	public string name {
		get {
			return attackNameInput.value;
		}
		set {
			attackNameInput.value = value;
		}
	}

	public void Init(string name, UnitDataInput parent) 
	{
		this.name = name;
		this.parent = parent;
	}

	public void Delete()
	{
		parent.Remove(this);
		Destroy(this.gameObject);
	}
}
