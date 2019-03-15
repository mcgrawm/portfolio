using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feeder : MonoBehaviour {

	public Food foodPrefab;
	public List<Material> flavours;
	public List<Renderer> bands;
	public GameObject feedingPoint;


	void Awake() {
		foreach (Renderer band in bands) {
			band.enabled = false;
		}
	}

	private bool _isGivingFood = false;

	// Update is called once per frame
	void Update () {
		if (!_isGivingFood && Input.GetKeyDown (KeyCode.DownArrow)) {
			_isGivingFood = true;
			StartCoroutine ("GiveFood");
		} 
	}



	// Use this for initialization
	private IEnumerator GiveFood () {

		StartCoroutine (BlinkCR());
		// Give food
		float foodAmount = Random.Range (12f, 16f);
		while (foodAmount > 0f) {
			Food food = Instantiate<Food> (foodPrefab, this.transform);
			food.size = Mathf.Min (foodAmount, Random.Range (.8f, 1.2f));
			food.flavour = flavours [Random.Range (0, flavours.Count)];
			food.Init ();
			foodAmount -= food.size;
			yield return new WaitForSeconds (Random.Range (food.size / 4f, food.size / 2f));
		}

		_isGivingFood = false;
		_blinking = false;

	}

	private bool _blinking = false;
	private IEnumerator BlinkCR() {
		_blinking = true;
		while (_blinking) {
			//blink lights
			foreach (Renderer band in bands) {
				band.enabled = true;
				yield return new WaitForSeconds (.2f);
				band.enabled = false;
			}
		}
	}


}
