using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public GameObject selectionUI;
	public GameObject ngui;
	public Material emptyMaterial;
	public GameObject[] cards;
	public GameObject[] slots;
	public GameObject[] particleEmitter;

	public float varX = 10;
	public float varY = 3;
	public float varZ = 6;
	public float offsetX = 0;
	public float offsetY = -18;
	public float offsetZ = 7;

	int currentSlot = 0;

	int[] fireworkSelection;

	// Use this for initialization
	void Start () 
	{
		fireworkSelection = new int[slots.Length];
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButtonDown("Fire1")) {
			OnClick();
		}
	}

	void OnClick()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if(Physics.Raycast(ray, out hit)) {
			if(hit.collider.CompareTag("Card") && currentSlot < slots.Length) {
				// we clicked a card
				int index = GetIndex(hit.collider.gameObject);
				Debug.Log("index: " + index);
				slots[currentSlot].renderer.material = cards[index].renderer.sharedMaterial;
				fireworkSelection[currentSlot] = index;
				currentSlot++;
			}
		}
	}

	int GetIndex(GameObject go) {
		for (int i = 0; i < cards.Length; i++) {
			if(go.renderer.sharedMaterial == cards[i].renderer.sharedMaterial) {
				return i;
			}
		}
		return -1;
	}

	public void Reset()
	{
		fireworkSelection = new int[slots.Length];
		currentSlot = 0;

		foreach(GameObject slot in slots) {
			slot.renderer.material = emptyMaterial;
		}
	}

	public void RandomSetup()
	{
		Reset();
		currentSlot = slots.Length;
		for (int i = 0; i < fireworkSelection.Length; i++) {
			fireworkSelection[i] = Random.Range(0,cards.Length);
			slots[i].renderer.material = cards[fireworkSelection[i]].renderer.sharedMaterial;
		}
	}

	public void StartFirework()
	{
		selectionUI.SetActive(false);
		ngui.SetActive(false);

		StartCoroutine(FireworkRoutine());
	}

	IEnumerator FireworkRoutine() {
		int index = 0;

		while(index < slots.Length) {
			GameObject handle = (GameObject) Instantiate(particleEmitter[fireworkSelection[index]]);
			float x = Random.Range(0,100)/100f * 2 * varX - varX + offsetX;
			float y = Random.Range(0,100)/100f * 2 * varY - varY + offsetY;
			float z = Random.Range(0,100)/100f * 2 * varZ - varZ + offsetZ;
			handle.transform.position = new Vector3(x,y,z);

			yield return new WaitForSeconds(Random.Range(0,100)/100f);
			index++;
		}
		yield return new WaitForSeconds(6);
		selectionUI.SetActive(true);
		ngui.SetActive(true);
	}
}