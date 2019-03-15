using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDetector : MonoBehaviour {

	private RoboFish _roboFish;

	public void Init(RoboFish fish) {
		_roboFish = fish;
	}

	void OnTriggerEnter (Collider other) {
		Food food = other.GetComponent<Food> ();

		if (_roboFish != null && food != null && !food.isEaten) {
			_roboFish.EatFood (food);
		}
	}

}
