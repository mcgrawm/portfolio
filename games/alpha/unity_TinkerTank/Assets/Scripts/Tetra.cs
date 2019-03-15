using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetra : MonoBehaviour {

	public List<Color> colors;
	public Renderer innerBody;

	private int _colorIndex = 0;

	private RoboFish _roboFish;

	// Use this for initialization
	void Awake () {
		_colorIndex = Random.Range (0, colors.Count);
		_roboFish = gameObject.GetComponent<RoboFish> ();
	}

	void Start() {
		StartCoroutine ("ColorCycleCR");
	}
	
	private IEnumerator ColorCycleCR() {
		int nextColorIndex;
		Color col;

		while (true) {
	
			_colorIndex++;
			if (_colorIndex >= colors.Count)
				_colorIndex = 0;
			
			nextColorIndex = _colorIndex + 1;
			if (nextColorIndex >= colors.Count)
				nextColorIndex = 0;

			int count = 4;
			while (count > 0) {
				SetColor(Color.Lerp (colors [_colorIndex], colors [nextColorIndex], .5f));

				yield return new WaitForSeconds (.05f);
				count--;
			}

		}
	}

	private void SetColor(Color col) {
		foreach(Renderer r in _roboFish.renderers) {
			r.material.color = new Color(col.r, col.g, col.b, .2f);
		}
		innerBody.material.color = col;
	}

}
