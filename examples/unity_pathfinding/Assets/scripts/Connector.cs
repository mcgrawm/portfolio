using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour {

	// the stick gameObject that connects nodes (balls)
	public GameObject cylinderSmall;
	public GameObject cylinderLarge;

	// Use this for initialization
	void Start () {
		SetDefaultLook();
	}
	
	public void SetDefaultLook() {
		cylinderSmall.gameObject.SetActive(true);
		cylinderLarge.gameObject.SetActive(false);
	}

	public void SetPathLook() {
		cylinderLarge.gameObject.SetActive(true);
		cylinderSmall.gameObject.SetActive(false);
	}


}
