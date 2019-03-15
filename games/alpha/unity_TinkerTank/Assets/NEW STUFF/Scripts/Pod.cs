using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

/*
 * Dual-Control FishPod-7000
 * Controller A and B are connected by default. Both inputs are combined during the Update loop, for the following controls:
 * LS: Point Pod Up, Down, Left, Right
 * RS: Thrust: Forward, Back, Left, Right
 * 
 * The FishPod-7000 has an EnergyPod and A BubbleJet Drive.
*/

public class Pod : MonoBehaviour {

	// Public Properties
	public float tiltSpeed;
	public float tiltMax;
	public float tiltMin;
	public float rotateSpeed;
	public float forwardSpeed;
	public float backwardSpeed;
	public float lateralSpeed;
	public float clawRotateSpeed;
	public float clawRotateMax;

	public EnergyPod energyPod;
	public BubbleJet bubbleJet;
	public float energyRefreshRate;
	public float energyShutDownThreshold;
	public float RSenergyDrain;
	public float LSenergyDrain;
	public float RTenergyDrain;
	public float LTenergyDrain;


	public Transform leftClaw;
	public Transform rightClaw;

	private Rigidbody _rBody;

	private float _RSX;
	private float _RSY;
	private float _LSX;
	private float _LSY;
	private float _RT;
	private float _LT;

	private bool _isSoloMode;
	private float _energy = 1f;

	private Player _playerA;
	private Player _playerB;

	void Awake () {
		_rBody = GetComponent<Rigidbody> ();
		_rBody.mass = 2f;
		RefreshControllers ();
	}

	private void RefreshControllers() {
		_playerA = ReInput.players.GetPlayer(0);
		Debug.Log ("player A " + _playerA.id);
		_playerB = ReInput.players.GetPlayer(1);
		_isSoloMode = true;
	}


	void Update () {

		_energy += energyRefreshRate * Time.deltaTime;
		_energy = Mathf.Clamp (_energy, 0f, 1f);
		energyPod.energyLevel = _energy;

		if (_energy < energyShutDownThreshold)
			return;

		if (_isSoloMode) {
			_RSX = _playerA.GetAxis ("RSX");
			_RSY = _playerA.GetAxis ("RSY");
			_LSX = _playerA.GetAxis ("LSX");
			_LSY = _playerA.GetAxis ("LSY");
			_RT = _playerA.GetAxis ("RT");
			_LT = _playerA.GetAxis ("LT");
		} else {
			// _RSU =  (??? + ???)/2f;
			// _RSD =  (??? + ???)/2f;
			// _RSL =  (??? + ???)/2f;
			// _RSR =  (??? + ???)/2f;
			// _LSU =  (??? + ???)/2f;
			// _LSD =  (??? + ???)/2f;
			// _LSL =  (??? + ???)/2f;
			// _LSR =  (??? + ???)/2f;
			// _RT =  (??? + ???)/2f;
			// _LT =  (??? + ???)/2f;
		}
			

		Debug.Log ("RS: " + _RSX + " " + _RSY);

		// Rotate left / right
		_rBody.AddRelativeTorque(Vector3.up * Time.deltaTime * (_RSX) * rotateSpeed);

		// Rotate up / down
		_rBody.AddRelativeTorque(Vector3.right * Time.deltaTime * (_RSY) * tiltSpeed);
		// TODO apply min / max tilt through easing?

		// forward / back
		if (_LSY > 0) {
			_rBody.AddForce(Vector3.forward * Time.deltaTime * _LSY * forwardSpeed, ForceMode.Acceleration);
		} else {
			_rBody.AddForce(Vector3.back * Time.deltaTime * _LSY * backwardSpeed, ForceMode.Acceleration);
		}
		 
		// sideways
		_rBody.AddForce(Vector3.right * Time.deltaTime * _LSX * lateralSpeed, ForceMode.Acceleration); 

		// Energy drain
		_energy -= (Mathf.Abs(_RSX) + Mathf.Abs(_RSY)) * RSenergyDrain * Time.deltaTime;
		_energy -= (Mathf.Abs(_LSX) + Mathf.Abs(_LSY)) * LSenergyDrain * Time.deltaTime;
		_energy -= ((_RT * RTenergyDrain) + (_LT * LTenergyDrain)) * Time.deltaTime;

	}



}
