using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodePanel : MonoBehaviour {

	public Text nameField;
	public Text stateField;
	public Button exitButton;
	public Slider energySlider;
	public Slider socialSlider;
	public Slider curiousSlider;


	private RoboFish _roboFish;
	private CanvasGroup _canvasGroup;

	void Awake() {
		_canvasGroup = GetComponent<CanvasGroup> ();
		Hide ();

		exitButton.onClick.AddListener (() => {
			Hide ();
		});

		energySlider.onValueChanged.AddListener ((val) => {
			_roboFish.energy = val;
		});
			
		socialSlider.onValueChanged.AddListener ((val) => {
			_roboFish.social = val;
		});

		curiousSlider.onValueChanged.AddListener ((val) => {
			_roboFish.curious = val;
		});
			
	}
		


	public void SetRoboFish(RoboFish rf) {
		_roboFish = rf;

		if (_roboFish == null) {
			Hide ();
			return;
		}
			
		nameField.text = _roboFish.name;
		energySlider.value = _roboFish.energy;
		socialSlider.value = _roboFish.social;
		curiousSlider.value = _roboFish.curious;
		StartCoroutine (UpdateInfoCR ());
		Show();
	}

	private IEnumerator UpdateInfoCR() {
		while (_roboFish != null) {
			yield return new WaitForSeconds (1f);
			stateField.text = _roboFish.state;
			energySlider.value = _roboFish.energy;
		}
	}

	private void Show() {
		_canvasGroup.alpha = 1f;
		_canvasGroup.interactable = true;
		_canvasGroup.blocksRaycasts = true;
	}

	public void Hide() {
		_roboFish = null;
		_canvasGroup.alpha = 0f;
		_canvasGroup.interactable = false;
		_canvasGroup.blocksRaycasts = false;
	}

}
