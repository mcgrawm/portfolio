using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropulsionTail : MonoBehaviour {

	public enum AXIS {X,Y,Z

	};

	public bool spin;
	public float startRotationDirection;

	public AXIS axis;
	public float range;

	private RoboFish _roboFish;

	public delegate void OnTailSweep(float kickPower);
	public event OnTailSweep onTailSweep;

	public void Init(RoboFish fish) {
		_roboFish = fish;
	}
		
	private float _rotationDirection 	= 0f;
	private float _targetRotation		= 0f;

	private float _origXRotation;
	private float _origYRotation;
	private float _origZRotation;

	void Awake() {
		_origXRotation = transform.localRotation.x;
		_origYRotation = transform.localRotation.y;
		_origZRotation = transform.localRotation.z;
	}

	void Update () {
		if (spin)
			DoSpin ();
		else
			DoFlap ();

	}

	private void DoFlap() {
		if (_roboFish.GetSpeed () == 0f) {
			_rotationDirection = 0f;
			_targetRotation = 0f;

			switch (axis) {
			case AXIS.X:
				transform.localRotation = Quaternion.Euler (_origXRotation, transform.localRotation.y, transform.localRotation.z);
				break;
			case AXIS.Y:
				transform.localRotation = Quaternion.Euler (transform.localRotation.x, _origYRotation, transform.localRotation.z);
				break;
			case AXIS.Z:
				transform.localRotation = Quaternion.Euler (transform.localRotation.x, transform.localRotation.y, _origZRotation);
				break;
			}
				
		} else {

			if (_rotationDirection == 0f) {
				// we're moving, but we need a target tail rotation
				if (startRotationDirection != 0f) {
					_rotationDirection = startRotationDirection;
				} else if (Random.value > .5f) {
					_rotationDirection = 1f;
				} else {
					_rotationDirection = -1f;
				}
			}

			// rotate tail toward target rotation
			_targetRotation += 100f * Time.deltaTime * _rotationDirection * _roboFish.GetSpeed();

			switch (axis) {
			case AXIS.X:
				transform.localRotation = Quaternion.Euler (transform.localRotation.x + _targetRotation, transform.localRotation.y,  transform.localRotation.z);
				break;
			case AXIS.Y:
				transform.localRotation = Quaternion.Euler (transform.localRotation.x, transform.localRotation.y + _targetRotation, transform.localRotation.z);
				break;
			case AXIS.Z:
				transform.localRotation = Quaternion.Euler (transform.localRotation.x, transform.localRotation.y, transform.localRotation.z + _targetRotation);
				break;
			}

			if ((_rotationDirection == 1f && _targetRotation > range) || (_rotationDirection == -1f && _targetRotation < -1f*range)) {
				_rotationDirection *= -1f;
			}
		}
	}

	private void DoSpin() {
		float spin = _roboFish.GetSpeed () * Time.deltaTime * 500f;
		switch (axis) {
		case AXIS.X:
			transform.Rotate (spin, 0f, 0f);
			break;
		case AXIS.Y:
			transform.Rotate (0f, spin, 0f);
			break;
		case AXIS.Z:
			transform.Rotate (0f, 0f, spin);
			break;
		}
	}


	void LateUpdate() {

	}


}
