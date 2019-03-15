using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTarget : MonoBehaviour {


	private RoboFish _roboFish;
	private Tank _tank;

	private Vector3 _targetPosition;
	private bool _isFollowingOtherFish = false;
	private Food _targetFood;



	public void Init(RoboFish roboFish, Tank tank) {
		_roboFish 	= roboFish;
		_tank 		= tank;
		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			r.material.color = new Color(_roboFish.baseColour.r, _roboFish.baseColour.g, _roboFish.baseColour.b, .4f) ;
		}
	}

	public void SetTargetVisible(bool vis) {
		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			r.enabled = vis;
		}
	}

	public void Recalculate() {
		// are we chasing food?
		if (_targetFood == null) {
			if (Random.Range (30, 60) > _roboFish.energy) {
				_targetFood = _tank.GetFoodSource (_roboFish);
				if (_targetFood != null) {
					_isFollowingOtherFish = false;
					StartCoroutine ("ChasingFoodCR");
					_roboFish.state = "eating";
				}
				_isFollowingOtherFish = false;
				_targetPosition = AdjustForDepthPreference(GetRandomPoint ());
			} else if (Random.value < _roboFish.social) {
				_isFollowingOtherFish = true;
				StartCoroutine ("FollowOtherFishCR");
				_roboFish.state = "socializing";
			} else if (Random.value < _roboFish.curious) {
				_isFollowingOtherFish = false;
				_targetPosition = AdjustForDepthPreference(GetCuriousWaypoint ());
				_roboFish.state = "exploring";
			} else {
				_isFollowingOtherFish = false;
				_targetPosition = AdjustForDepthPreference(GetRandomPoint ());
				_roboFish.state = "wandering";
			}
		}
	}
		
	private Vector3 GetCuriousWaypoint() {
		return _tank.GetCuriousThing ().transform.position + GetPositionShift ();
	}


	private Vector3 GetPositionShift() {
		return new Vector3 (Random.Range (-.5f, .5f), Random.Range (-.5f, .5f), Random.Range (-.5f, .5f));
	}

	private Vector3 GetRandomPoint() {
		return new Vector3 (Random.Range (-10f, 10f), Random.Range (-5f, 5f), Random.Range (-5f, 3f));
	}

	private Vector3 AdjustForDepthPreference(Vector3 v3Input) {
		// Average v3Input with preferred depth
		float idealDepth = _roboFish.depth * 10f -5f;
		float depthWeight = Random.Range (.2f, 1.2f);
		float newY = (v3Input.y + (idealDepth * depthWeight)) / (1f + depthWeight);

		return new Vector3 (v3Input.x, newY, v3Input.z);
	}

	private Vector3 GetFriendlyFishPoint() {
		// this finds a point near the fish you like the best
		List<RoboFish> allFish = Tank.instance.GetAllFish();

		float xAvg = 0f;
		float yAvg = 0f;
		float zAvg = 0f;

		foreach (RoboFish f in allFish) {
			if (f == this._roboFish) {
				xAvg += f.transform.position.x;
				yAvg += f.transform.position.y;
				zAvg += f.transform.position.z;
			}
			xAvg += f.transform.position.x;
			yAvg += f.transform.position.y;
			zAvg += f.transform.position.z;
		}

		xAvg /= (allFish.Count+1);
		yAvg /= (allFish.Count+1);
		zAvg /= (allFish.Count+1);

		return new Vector3 (xAvg, yAvg, zAvg);

	}

	private Vector3 GetClosestFishPoint() {
		// this finds a point near the fish you like the best
		List<RoboFish> allFish = Tank.instance.GetAllFish();

		float closestDistance = 1000f;
		RoboFish closestFish = null;

		foreach (RoboFish f in allFish) {
			if (f != _roboFish) {
				float thisDistance = Vector3.Distance (_roboFish.transform.position, f.transform.position);
				if (thisDistance < closestDistance) {
					closestDistance = thisDistance;
					closestFish = f;
				}
			}
		}

		return closestFish.transform.position;

	}

	private IEnumerator FollowOtherFishCR() {
		while (_isFollowingOtherFish) {
			_targetPosition = AdjustForDepthPreference(GetClosestFishPoint ());
			yield return new WaitForSeconds (.25f);
		}
	}

	private IEnumerator ChasingFoodCR() {
		while (_targetFood != null) {
			_targetPosition = _targetFood.transform.position;
			yield return new WaitForSeconds (.1f);
		}
	}

	private void LateUpdate() {
		this.transform.position = Vector3.Lerp (this.transform.position, _targetPosition, .1f);
	}

}
