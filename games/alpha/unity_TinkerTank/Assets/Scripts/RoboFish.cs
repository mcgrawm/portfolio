using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboFish : MonoBehaviour {

	public float baseSpeed;
	public FishTarget fishTargetPrefab;
	public GameObject eyes;
	public Transform bearing;
	public Transform eatPoint;

	public float social;
	public float curious;
	public float depth;
	public List<Material> favouriteFood;

	public string state = "";

	private float _energy;

	private float _waypointTimeoutCounter;

	public Color baseColour;
	public List<Renderer> renderers;

	private Rigidbody _rBody;
	private FishTarget _fishTarget;

	private float _currentSpeed = 1f;
	private bool _isPropelling = false;
	private Tank _tank;

	private List<EnergyBar> _energyBars;
	private Animation _anim;
	private FoodDetector _foodDetector;

	void Awake() {
		_tank 	= FindObjectOfType<Tank> ();
		_rBody 	= GetComponent<Rigidbody> ();
		_anim 	= GetComponent<Animation> ();
		_energy 	= 1f;

		foreach(Renderer r in renderers) {
			r.material.color = baseColour;
		}
		_fishTarget = Instantiate<FishTarget> (fishTargetPrefab);

		List<PropulsionTail> ptails = new List<PropulsionTail> (GetComponentsInChildren<PropulsionTail> ());
		foreach (PropulsionTail p in ptails) {
			p.Init (this);
		}

		GetComponentInChildren<FoodDetector> ().Init (this);
		_fishTarget.Init (this,_tank);

		_energyBars = new List<EnergyBar> (GetComponentsInChildren<EnergyBar> ());
		foreach (EnergyBar eBar in _energyBars) {
			eBar.Init (this);
		}

	}

	public float energy {
		get {
			return _energy;
		}
		set {
			_energy = Mathf.Clamp(value, 0f, 1f);
		}
	}

	// Use this for initialization
	void Start () {
		StartCoroutine ("SwimThrustCR");	// AIms and propels fish at current target
		StartCoroutine ("BlinkCR");			// Makes eyes blink
		StartCoroutine ("LoseEnergyCR");	// tarcks energy loss over time
	}

	public float GetSpeed() {
		return _currentSpeed * baseSpeed * 6f  * Mathf.Max(.5f,energy);
	}
		
	public void SetTargetVisible(bool vis) {
		_fishTarget.SetTargetVisible (vis);
	}
		
	void Update () {
		if (_isPropelling) {
			_rBody.AddRelativeForce (Vector3.forward * GetSpeed(), ForceMode.Force);

			if (Vector3.Distance (transform.position, _fishTarget.transform.position) > 2f) {
				bearing.LookAt (_fishTarget.transform);
				transform.rotation = Quaternion.Slerp (transform.rotation, bearing.rotation, baseSpeed * Time.deltaTime);
			} else {
				//reached target
				ChooseNewWaypoint();
			}

			// only bother with timeout logic if we're currently propelling
			_waypointTimeoutCounter -= Time.deltaTime;
			if (_waypointTimeoutCounter <= 0f) {
				ChooseNewWaypoint ();
			}
		}
	}
		
	public void EatFood(Food food) {
		if (food == null)
			return;
		food.isEaten = true;
		if(_anim != null)
			_anim.Play ("RoboFishChomp");
		StartCoroutine ("EatFoodCR",food);
	}

	private IEnumerator EatFoodCR (Food food) {
		if (food != null) {
			food.GetEatenBy (eatPoint);
			yield return new WaitForSeconds (.4f);
			if (food != null) {
				int energyFromFood = Mathf.RoundToInt(food.size);
				if (favouriteFood.Contains (food.flavour)) {
					energyFromFood *= 2;
				}

				energy += energyFromFood;
				Destroy (food.gameObject);
			}
		}
	}

	private void ChooseNewWaypoint() {
		_fishTarget.Recalculate ();
		_waypointTimeoutCounter = 12f;
	}

	private IEnumerator LoseEnergyCR() {
		while (true) {
			yield return new WaitForSeconds (1);
			if (_energy > 0f)
				_energy -= _rBody.mass/500f;
		}
	}

	private IEnumerator SwimThrustCR() {
		while (true) {
			yield return new WaitForSeconds (Random.Range(0.5f, 2.5f));

			// swim / no swim
			if (_isPropelling) {
				if (Random.value < .1f)
					_isPropelling = false;
			} else {
				if (Random.value < .7f)
					_isPropelling = true;
			}

			// if swimming, recalculate current speed
			if (_isPropelling) {
				_currentSpeed = Random.Range (.25f, 1f);
			} else {
				_currentSpeed = 0f;
			}
		}
	}
		
	private IEnumerator BlinkCR() {
		while (true) {
			yield return new WaitForSeconds (Random.Range(2.5f, 8f) - curious * 2);

			int numBlinks = Random.Range (1, 3);

			for (int b = 0; b < numBlinks; b++) {
				eyes.SetActive (false);
				yield return new WaitForSeconds (Random.Range(.06f, .15f));
				eyes.SetActive (true);
				yield return new WaitForSeconds (Random.Range(.1f, .4f));
			}
		}
	}

}
