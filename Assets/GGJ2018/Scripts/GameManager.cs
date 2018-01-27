using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

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
	public PostProcessingProfile postProcessing;
	public Planet[] planets = new Planet[0];
	public Player player2;

	void Awake() {
		GameManager.instance = this;

		StartCoroutine(this.CameraZoon());
	}
	void Update() {
		if (this.state == 0) {
			if (this.isWin) {
				StartCoroutine(this.WinAction());
			}
		}
		if (Input.GetButtonDown("Jump")) {
			if (Player.instances.Count == 1) {
				Player newPlayer = Instantiate(this.player2);
				newPlayer.isNewPlayer = true;
				newPlayer.transform.position = new Vector3(0, -50, 0);
			}
		}
	}

	private IEnumerator CameraZoon() {
		GameManager.instance.mainCamera.orthographicSize = 10;
		yield return StartCoroutine(this.SetBloom(2, 0.5f, 2));

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
		yield return new WaitForSeconds(5);
		yield return StartCoroutine(this.SetBloom(0.5f, 2, 5));
	}
	public IEnumerator SetBloom(float from, float to, float s) {
		BloomModel.Settings bSettings = this.postProcessing.bloom.settings;
		bSettings.bloom.intensity = from;
		this.postProcessing.bloom.settings = bSettings;
		float time1 = Time.time;
		while (Time.time - time1 < s) {
			float currentTime = Time.time - time1;
			bSettings.bloom.intensity = Mathf.Lerp(from, to, currentTime / s);
			this.postProcessing.bloom.settings = bSettings;
			yield return new WaitForEndOfFrame();
		}
		bSettings.bloom.intensity = to;
		this.postProcessing.bloom.settings = bSettings;
	}
}
