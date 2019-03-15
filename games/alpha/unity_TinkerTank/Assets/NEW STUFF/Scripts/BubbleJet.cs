using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleJet : MonoBehaviour {

	public Bubble bubblePrefab;
	private List<Bubble> _bubblePool;

	void Awake() {
		_bubblePool = new List<Bubble> ();
	}

	public void SpawnBubble() {

		Bubble b;

		if (_bubblePool.Count > 0) {
			b = _bubblePool [0];
			_bubblePool.Remove (b);
		} else {
			b = Instantiate<Bubble> (bubblePrefab, this.transform);
		}

		b.transform.localPosition 	= new Vector3 (Random.Range (-.2f, .2f), 0f, Random.Range (-.2f, .2f));
		float scale 				= Random.Range (0.4f, 1.0f);
		b.transform.localScale 		= new Vector3 (scale, scale, scale);

		b.onBubblePop += OnBubblePop;
		b.gameObject.SetActive (true);

		_bubblePool = new List<Bubble> ();
		b.maxYValue = 5.5f;
		b.Init ();
	}

	private void OnBubblePop(Bubble b) {
		// return bubble to bubble pool

		b.onBubblePop -= OnBubblePop;
		_bubblePool.Add (b);
		b.gameObject.SetActive (false);
	}
}
