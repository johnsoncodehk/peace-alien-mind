using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cover : MonoBehaviour {
	
	private CanvasGroup canvasGroup;
	private Scene mainScene;
	private AsyncOperation loadMainScene;

	void Awake() {
		this.canvasGroup = this.GetComponent<CanvasGroup>();
	}
	void Start() {
		StartCoroutine(this.LoadMainScene());
		StartCoroutine(this.LogoAction());
	}
	IEnumerator LoadMainScene() {
		this.loadMainScene = SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Additive);
		this.loadMainScene.allowSceneActivation = false;
		yield return this.loadMainScene;
		SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));
	}
	IEnumerator LogoAction() {
		yield return new WaitForSeconds(4);
		this.loadMainScene.allowSceneActivation = true;
		while (this.canvasGroup.alpha > 0) {
			this.canvasGroup.alpha -= 2f * Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Cover"));
	}
}
