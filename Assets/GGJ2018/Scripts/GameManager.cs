using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

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
	
	private Planet[] planets = new Planet[0];

	void Awake() {
		this.planets = FindObjectsOfType<Planet>();
		// StartCoroutine(this.WinAction()); // test
	}
	void Update() {
		if (this.state == 0) {
			if (this.isWin) {
				StartCoroutine(this.WinAction());
			}
		}
	}

	private IEnumerator WinAction() {
		yield return new WaitForSeconds(5);
		Camera.main.orthographic = false;
		this.planetAxis.gameObject.SetActive(true);
		for (int i = 0; i < this.planets.Length; i++) {
			Planet planet = this.planets[i];
			planet.holder = this.planetAxis.transform.GetChild(i);
		}
	}
}
