using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour {

	private string baseURL		= "https://github.com/mcgrawm/TinkerTank/blob/master/";
	private string URLsuffix 	= "?raw=true";
	public List<string> assetURLs;
	public Renderer screenRanderer;

	public List<Material> fishImages;


	void Start() {
		StartCoroutine (CycleImage_CR ());
	}

	private IEnumerator CycleImage_CR() {
		while (true) {

			screenRanderer.material = fishImages [Random.Range (0, fishImages.Count - 1)];
			yield return new WaitForSeconds (Random.Range (4f, 6f));

		}
	}

	/*
	private IEnumerator CycleImage_CR() {
		while (true) {

			Texture2D tex;
			tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
			WWW www = new WWW(baseURL + assetURLs[Random.Range(0,assetURLs.Count-1)] + URLsuffix);
			yield return www;
			www.LoadImageIntoTexture(tex);
			screenRanderer.material.mainTexture = tex;

			yield return new WaitForSeconds (Random.Range (2f, 3f));

		}
	}*/




}
