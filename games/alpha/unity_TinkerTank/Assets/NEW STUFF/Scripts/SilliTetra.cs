using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * SilliTetra are very binary creatures.
 * They are either curious, or spooked.
 * 
 * Each Tetra will get spooked by one of three things: A Pod or a Spooked Tetra.
 * Each individual has a curious colour and an afraid colour, but each is set is unique. All colors come from a common set of 6.
 * 
 * When a SilliTetra is calm, it is in colorA. 
 * When it is not calm, it is in colorB, and moving rapidly.
 * 
 * SilliTetra become stressed when they are spooked. 
*/


public class SilliTetra : MonoBehaviour {

	// ROBOFISH
	public float baseSpeed;
	public GameObject eyes;
	public Transform bearing;
	public List<Renderer> renderers;
	public Renderer innerBody;
	public Transform spinTail;

	private Rigidbody _rBody;
	private float _currentSpeed = 1f;
	private Tank _tank;

	private Transform _targetTransform;
	private Vector3 _targetPosition;


	// TETRA
	public List<Color> allColors;

	private Color _currentColor;
	public Color curiousColor;
	public Color spookedColor;
	public Color favouriteColor;
	public Color fearedColor;

	private int _spookTimer;
	private int _colorIndex = 0;
	private bool _isPropelling = true;

	public Color currentColor {
		get { return _currentColor; }
	}

	void Awake () {
		_tank 	= FindObjectOfType<Tank> ();
		_rBody 	= GetComponent<Rigidbody> ();

		// what are this Tetras true colors?

		curiousColor = allColors[Random.Range(0,allColors.Count)];
		spookedColor = allColors[Random.Range(0,allColors.Count)];
		while (spookedColor == curiousColor) {
			spookedColor = allColors[Random.Range(0,allColors.Count)];
		}
		SetColor (curiousColor);

		favouriteColor = allColors[Random.Range(0,allColors.Count)];
		fearedColor = allColors[Random.Range(0,allColors.Count)];
		while (fearedColor == favouriteColor) {
			fearedColor = allColors[Random.Range(0,allColors.Count)];
		}

	}

	// Use this for initialization
	void Start () {
		ChooseNewTarget ();
		StartCoroutine ("SwimThrustCR");	// AIms and propels fish at current target
		StartCoroutine ("BlinkCR");			// Makes eyes blink
	}

	public void OnTouch() {
		Debug.Log ("Poke!");
	}

	private void ChooseNewTarget() {
		List<SilliTetra> otherTetra = new List<SilliTetra> (FindObjectsOfType<SilliTetra> ());

		// Already following a Tetra of the right color?
		if (_targetTransform != null) {
			SilliTetra tt = _targetTransform.gameObject.GetComponent<SilliTetra> ();
			if (tt != null && tt.currentColor == favouriteColor) {
				// already have a friend!
				return;
			}
		}

		// Is there a Tetra that is the right color?
		List<SilliTetra> candidates = new List<SilliTetra>();
		foreach (SilliTetra t in otherTetra) {
			if (t != this && t.currentColor == favouriteColor) {
				candidates.Add (t);
			}
		}

		if (candidates.Count > 0) {
			_targetTransform = candidates[Random.Range(0,candidates.Count)].transform;
		} else {
			// Still no candidate? choose random point in tank
			_targetPosition = GetRandomPoint ();
			_targetTransform = null;
		}


	}

	private Vector3 GetRandomPoint() {
		return new Vector3 (Random.Range (-10f, 10f), (Random.Range (-5f, 5f) + transform.position.y*2f)/3f, Random.Range (-5f, 3f));
	}

	public float GetSpeed() {
		return _currentSpeed * baseSpeed;
	}
		

	// Update is called once per frame
	void Update () {
		// spook timer
		if (_spookTimer > 0) {
			_spookTimer--;
			if (_spookTimer <= 0)
				SetSpooked (false);
		} 
			
		if (_isPropelling) {
			_rBody.AddRelativeForce (Vector3.forward * baseSpeed * _currentSpeed, ForceMode.Force);
			if (_targetTransform != null) {
				bearing.LookAt (_targetTransform);
			} else {
				bearing.LookAt (_targetPosition);
			}
			transform.rotation = Quaternion.Slerp (transform.rotation, bearing.rotation, baseSpeed * Time.deltaTime);
			spinTail.Rotate (0f, 0f, GetSpeed () * Time.deltaTime * 1000f);
		}
			

	}

	private IEnumerator TargetChoiceCR() {
		while (true) {
			yield return new WaitForSeconds (Random.Range(1f, 2f));
			ChooseNewTarget ();
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
			yield return new WaitForSeconds (Random.Range(2f, 8f));

			int numBlinks = Random.Range (1, 3);

			for (int b = 0; b < numBlinks; b++) {
				eyes.SetActive (false);
				yield return new WaitForSeconds (Random.Range(.06f, .15f));
				eyes.SetActive (true);
				yield return new WaitForSeconds (Random.Range(.1f, .4f));
			}
		}
	}

	private void SetSpooked(bool spooked) {
		if (spooked) {
			SetColor (spookedColor);
			_spookTimer = 1000;
		} else {
			SetColor (curiousColor);
		}
	}

	private void SetColor(Color col) {
		_currentColor = col;
		foreach(Renderer r in renderers) {
			r.material.color = new Color(_currentColor.r, _currentColor.g, _currentColor.b, .2f);
		}
		innerBody.material.color = _currentColor;
	}
}
