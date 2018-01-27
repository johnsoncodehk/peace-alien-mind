using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public bool isWin {
		get {
			foreach (Planet planet in this.planets) {
				if (!planet.isWin) {
					return false;
				}
			}
			return true;
		}
	}

	public int state;
	public PlanetAxis planetAxis;
	public Camera mainCamera;
	
	private Planet[] planets = new Planet[0];

	void Awake() {
		GameManager.instance = this;
		this.planets = FindObjectsOfType<Planet>();
		// StartCoroutine(this.WinAction()); // test

		StartCoroutine(this.CameraZoon());
	}
	void Update() {
		if (this.state == 0) {
			if (this.isWin) {
				StartCoroutine(this.WinAction());
			}
		}
	}

	private IEnumerator CameraZoon() {
		GameManager.instance.mainCamera.orthographicSize = 10;
		yield return new WaitForSeconds(5);
		float v = 0;
		while (GameManager.instance.mainCamera.orthographicSize < 40) {
			GameManager.instance.mainCamera.orthographicSize = Mathf.SmoothDamp(GameManager.instance.mainCamera.orthographicSize, 40, ref v, 10);
			yield return new WaitForEndOfFrame();
		}
	}
	private IEnumerator WinAction() {
		yield return new WaitForSeconds(5);
		GameManager.instance.mainCamera.orthographic = false;
		this.planetAxis.gameObject.SetActive(true);
		for (int i = 0; i < this.planets.Length; i++) {
			Planet planet = this.planets[i];
			planet.holder = this.planetAxis.transform.GetChild(i);
		}
	}
}
