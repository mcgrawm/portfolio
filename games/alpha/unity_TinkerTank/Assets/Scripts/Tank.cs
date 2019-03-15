using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* names
 * John, Roger, Brenna, Jeff, Kyle?, Pam, Colin,
 * 
 */
// todo look up "Ken Wilber"

public class Tank : MonoBehaviour {

	private List<RoboFish> _rFish;
	private static Tank _instance;
	public CanvasGroup uiCanvas;
	public List<GameObject> curiousThings;
	public List<Feeder> feeders;

	private bool _targetsVisible = false;

	void Awake() {
		_instance = this;
		_rFish = new List<RoboFish> (FindObjectsOfType<RoboFish> ());
	}

	void Start() {
		SetTargetsVisible (false);
		Cursor.visible = false;
	}

	public static Tank instance {
		get { return _instance; }
	}

	public List<RoboFish> GetAllFish() {
		return _rFish;
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Space)) {
			SetTargetsVisible (!_targetsVisible);
		}
	}

	private void SetTargetsVisible(bool vis) {
		_targetsVisible = vis;
		foreach (RoboFish f in _rFish) {
			f.SetTargetVisible (_targetsVisible);
		}

		if (vis) {
			uiCanvas.alpha = 1f;
		} else {
			uiCanvas.alpha = 0f;
		}

	}

	public GameObject GetCuriousThing() {
		return curiousThings [Random.Range (0, curiousThings.Count - 1)];
	}


	public Food GetFoodSource(RoboFish fish) {
		List<Food> allFoods = new List<Food> (FindObjectsOfType<Food> ());
		List<Food> likedFoods = new List<Food> ();

		foreach (Food food in allFoods) {
			if (fish.favouriteFood.Contains (food.flavour)) {
				likedFoods.Add (food);
			}
		}

		if (likedFoods.Count > 0) {
			return likedFoods [Random.Range (0, likedFoods.Count)];
		} else if (allFoods.Count > 0) {
			return allFoods [Random.Range (0, allFoods.Count)];
		}

		return null;
	}

		
}
