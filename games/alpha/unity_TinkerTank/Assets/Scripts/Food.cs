using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {

	private float _size;
	private Material _flavour;
	private Rigidbody rBody;
	public Renderer renderer;
	public bool isEaten = false;
	private Transform _eater;

	void Awake() {
		rBody = GetComponent<Rigidbody> ();
	}

	public void Init() {
		StartCoroutine ("FloatCR");
		StartCoroutine ("TimeoutCR");
	}

	public float size {
		get { return this._size; }
		set { 
			_size = value; 
			transform.localScale = new Vector3 (Randomize(_size), Randomize(_size), Randomize(_size));
		}
	}

	public Material flavour {
		get { return this._flavour; }
		set { 
			_flavour = value; 
			renderer.material = _flavour;
		}
	}

	public void GetEatenBy(Transform eater) {
		_eater = eater;
		GetComponentInChildren<Rigidbody> ().isKinematic = true;
		GetComponent<Collider> ().enabled = false;
	}

	private IEnumerator TimeoutCR() {
		yield return new WaitForSeconds (Random.Range(20f, 30f));
		Destroy (this.gameObject);
	}
		
	private float Randomize(float size) {
		if (Random.value < .3f) {
			return size;
		} 
		return size * Random.Range (.8f, 1.2f);
	}

	// Update is called once per frame

	private Vector3 _floatDirection = Vector3.zero;

	private IEnumerator FloatCR() {
		float downwardEnergy = Random.Range (.5f, .7f);
		_floatDirection = Vector3.down * downwardEnergy;

		yield return new WaitForSeconds (Random.Range (.5f, .7f));

		while (true) {
			if (downwardEnergy < 0f) {
				downwardEnergy *= Random.Range(-.7f, -.8f);
			}
			_floatDirection = Vector3.down * downwardEnergy + new Vector3(Random.Range(-downwardEnergy,downwardEnergy),0f,Random.Range(-downwardEnergy,downwardEnergy));

			downwardEnergy -= Random.Range (.12f, .18f);
			yield return new WaitForSeconds (Random.Range (1f, 2f));
		}
			
	}

	void Update() {
		if (_eater != null) {
			transform.position = Vector3.Lerp (transform.position, _eater.position, Time.deltaTime);
		} else {
			rBody.AddForce (_floatDirection, ForceMode.Acceleration);
		}

	}

}
