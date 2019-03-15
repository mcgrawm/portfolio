using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {

	static int touch_mask;
	public CodePanel codePanel;

	// Use this for initialization
	void Awake () {
		touch_mask = LayerMask.GetMask("fish");
	}

	void Update () {

		if(Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}

		for (int i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch(i).phase == TouchPhase.Began) {
				Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, 100, touch_mask)) {
					RoboFish rf = hit.transform.gameObject.GetComponent<RoboFish> ();
					if (rf != null) {
						codePanel.SetRoboFish (rf);
					} else {
						//codePanel.Hide ();
					}
				} else {
					//codePanel.Hide ();
				}
			}
		}

	}




}
