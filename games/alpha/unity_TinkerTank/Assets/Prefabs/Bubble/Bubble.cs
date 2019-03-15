using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {


	private Rigidbody _rBody;

	public delegate void OnBubblePop(Bubble b);
	public event OnBubblePop onBubblePop;
	public float maxYValue = 0f;

	private bool _isActive;

	// Use this for initialization
	void Awake () {
		_isActive 	= false;
		_rBody 		= GetComponent<Rigidbody> ();
	}

	public void Init() {
		_isActive = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (_isActive == false)
			return;

		if (transform.position.y > maxYValue) {
			if (onBubblePop != null) {
				onBubblePop (this);
				_rBody.velocity = Vector3.zero;
				_isActive = false;
			}
		} else {
			_rBody.AddForce (Vector3.up * .15f, ForceMode.Force);
		}

	}

	void OnTriggerEnter(Collider col) {
		

	}
		

}
