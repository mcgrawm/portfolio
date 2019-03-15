using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour {

	private RoboFish _roboFish;
	private Transform _greenBar;
	private Transform _redBar;


	void Awake() {
		_greenBar = transform.Find ("GreenBar");
		_redBar = transform.Find ("RedBar");
	}

	public void Init(RoboFish fish) {
		_roboFish = fish;
	}

	void Update() {
		if (_roboFish == null)
			return;

		_redBar.localScale = new Vector3 (1f, Mathf.Lerp (_redBar.localScale.y, (float)_roboFish.energy / 100f, .5f * Time.deltaTime), 1f);
		_greenBar.localScale = new Vector3 (1f, Mathf.Lerp (_greenBar.localScale.y, (100f-(float)_roboFish.energy) / 100f, .5f * Time.deltaTime), 1f);
	}
}
