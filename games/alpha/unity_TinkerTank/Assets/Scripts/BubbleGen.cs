using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleGen : MonoBehaviour {

	public Bubble bubblePrefab;
	private List<Bubble> _bubblePool;

	void Start() {
		Init ();
	}

	public void Init() {
		_bubblePool = new List<Bubble> ();
		StartCoroutine (BubbleSpawnAsync ());
	}


	private IEnumerator BubbleSpawnAsync() {
		yield return new WaitForSeconds (Random.Range (0f, 1f));
		while (true) {
			Bubble b = SpawnBubble ();
			b.maxYValue = 5.5f;
			b.Init ();
			yield return new WaitForSeconds (Random.Range (0.2f, 0.4f));
		}
	}

	private Bubble SpawnBubble() {

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

		return b;
	}

	private void OnBubblePop(Bubble b) {
		// return bubble to bubble pool

		b.onBubblePop -= OnBubblePop;
		_bubblePool.Add (b);
		b.gameObject.SetActive (false);
	}

}
