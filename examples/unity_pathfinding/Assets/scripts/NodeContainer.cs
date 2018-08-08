using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeContainer : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		// slow rotate of node mesh
		transform.RotateAround(Vector3.up,.15f * Time.deltaTime);
	}
}
