using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UIPanel))]
public class BCombatMenu : MonoBehaviour {

	public bool moveToTarget = false;
	public GameObject attackButtonPrefab;

	public GameObject gameoverPanel;
	public UILabel gameoverLabel;
	public GameObject panel;
	public GameObject attackRing;
	public UIButton attackButton;
	public UIButton moveButton;
	public UIButton backButton;

	BUnit bUnit;

	void Update() 
	{
		if(panel.activeSelf && moveToTarget) {
			Vector3 screenPosition = Camera.main.WorldToScreenPoint(bUnit.transform.position);
			screenPosition.x -= Screen.width/2;
			screenPosition.y -= Screen.height/2;
			transform.localPosition = screenPosition;
		}
	}

	public void OpenForBUnit(BUnit bUnit) 
	{
		this.bUnit = bUnit;
		if(bUnit.unit.AIControled) {
			panel.SetActive(false);
			backButton.gameObject.SetActive(false);
		} else {
			panel.SetActive(true);
			attackButton.gameObject.SetActive(bUnit.unit.CanAttack);
			moveButton.gameObject.SetActive(bUnit.unit.CanMove);
			backButton.gameObject.SetActive(false);

			// clear attack ring ui
			List<GameObject> atkButtons = new List<GameObject>();
			foreach(Transform child in attackRing.transform) atkButtons.Add(child.gameObject);
			foreach(GameObject child in atkButtons) Destroy(child);
			atkButtons.Clear();

			// init attack buttons
			foreach(Attack atk in bUnit.unit.attacks.Values) {
				GameObject handle = (GameObject) Instantiate(attackButtonPrefab);
				handle.transform.parent = attackRing.transform;

				handle.GetComponent<BAttackButton>().Init(atk,this);
				atkButtons.Add(handle);
			}

			// arrange attack buttons
			RepositionAtkButtons(atkButtons);
		}
	}

	void RepositionAtkButtons(List<GameObject> atkButtons)
	{
		if(moveToTarget) {
			float radius = 80;
			int n = atkButtons.Count;

			for (int i = 0; i < n; i++) {
				float angle = (i)/((float) n) * 2 * Mathf.PI;

				atkButtons[i].transform.localPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
				atkButtons[i].transform.localScale = Vector3.one;
			}
		} else {
			float height = 70;
			for (int i = 0; i < atkButtons.Count; i++) {
				atkButtons[i].transform.localPosition = new Vector3(0, i*height);
				atkButtons[i].transform.localScale = Vector3.one;
			}
		}
	}

	public void ChooseAttack()
	{
		panel.SetActive (false);
		backButton.gameObject.SetActive (true);
		attackRing.SetActive(true);
	}
	
	public void ActionAttack(Attack attack) 
	{
		panel.SetActive(false);
		attackRing.SetActive(false);
		backButton.gameObject.SetActive(true);
		bUnit.DisplayAttackRange(attack);
	}
	
	public void ActionMove() 
	{
		panel.SetActive(false);
		backButton.gameObject.SetActive(true);
		bUnit.DisplayMovementRange();
	}

	public void ActionEndTurn()
	{
		Hide();
		bUnit.EndTurn();
	}

	public void Back()
	{
		panel.SetActive(true);
		backButton.gameObject.SetActive(false);
		attackRing.SetActive(false);
		bUnit.ClearDisplayRange();
	}

	public void ActionCompleted()
	{
//		if(bUnit.unit.CanMove || bUnit.unit.CanAttack)
			OpenForBUnit (bUnit);
//		else
//			backButton.gameObject.SetActive(false);
	}
	
	public void Hide() 
	{
		panel.SetActive(false);
	}

	public void DisplayGameover (string text)
	{
		gameoverPanel.SetActive(true);
		gameoverLabel.text = text;
	}
}