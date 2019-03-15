using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPod : MonoBehaviour {

	private Transform _greenBar;
	private Transform _redBar;

	public float energyLevel = 1.0f;

	void Awake() {
		_greenBar = transform.Find ("GreenBar");
		_redBar = transform.Find ("RedBar");
	}		

	void Update() {
		_redBar.localScale = new Vector3 (1f, Mathf.Lerp (_redBar.localScale.y, energyLevel / 100f, .5f * Time.deltaTime), 1f);
		_greenBar.localScale = new Vector3 (1f, Mathf.Lerp (_greenBar.localScale.y, (100f-energyLevel) / 100f, .5f * Time.deltaTime), 1f);
	}
}
