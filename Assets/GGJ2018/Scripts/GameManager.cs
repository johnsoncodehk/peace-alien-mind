using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance {
		get {
			if (!GameManager.m_Instance) {
				GameManager.m_Instance = FindObjectOfType<GameManager>();
			}
			return GameManager.m_Instance;
		}
	}
	private static GameManager m_Instance;

	public Player player;
	public int state;
	public Camera mainCamera;
	public PostProcessingBehaviour postProcessing;
	public bool showBlurry;
	// UI
	public MainMenu mainMenu;
	public SettingsPanel settingsPanel;
	public Button settingsButton, backButton;
	public int step;

	// runtime
	[HideInInspector] public Stage currentStage;

	private Scene m_PlayingLevel;
	private int currentStageIndex;
	private bool playingAnimation;
	private bool loadingLevel;
	private int level;
	private Image backButtonImage;

	void Awake() {
		this.settingsButton.onClick.AddListener(AudioManager.instance.PlayClickButton);
		this.settingsButton.onClick.AddListener(GameManager.instance.settingsPanel.Show);
		this.backButton.onClick.AddListener(AudioManager.instance.PlayClickButtonBack);
		this.backButton.onClick.AddListener(() => {
			StartCoroutine(this.NextStageAsync(true, true));
		});
		this.backButtonImage = this.backButton.GetComponent<Image>();
		this.postProcessing.profile = Instantiate(this.postProcessing.profile);
	}
	void Update() {
		if (Input.GetButtonDown("Horizontal")) {
			if (!this.currentStage) return;
			if (Input.GetAxisRaw("Horizontal") > 0) {
				StartCoroutine(this.NextStageAsync(false, true));
			}
			else {
				this.currentStageIndex -= 2;
				this.currentStageIndex = Mathf.Max(this.currentStageIndex, -1);
				StartCoroutine(this.NextStageAsync(false, true));
			}
		}
		this.SetBlurry(this.showBlurry ? 0.1f : 5);
		if (!this.playingAnimation) {
			if (this.currentStage) {
				if (this.currentStage.IsWin()) {
					StartCoroutine(this.NextStageAsync());
				}
			}
		}
		this.backButtonImage.raycastTarget = !this.playingAnimation && this.currentStage;
		this.backButton.interactable = m_PlayingLevel.isLoaded;
	}

	public IEnumerator SetBloom(float from, float to, float s) {
		BloomModel.Settings settings = this.postProcessing.profile.bloom.settings;
		settings.bloom.intensity = from;
		this.postProcessing.profile.bloom.settings = settings;
		float time1 = Time.time;
		while (Time.time - time1 < s) {
			float currentTime = Time.time - time1;
			settings.bloom.intensity = Mathf.Lerp(from, to, currentTime / s);
			this.postProcessing.profile.bloom.settings = settings;
			yield return new WaitForEndOfFrame();
		}
		settings.bloom.intensity = to;
		this.postProcessing.profile.bloom.settings = settings;
	}
	public void SetBlurry(float targetValue) {
		float min = 0.1f;
		float max = 5;
		float time = 0.25f;
		float speed = (max - min) / time;

		var settings = this.postProcessing.profile.depthOfField.settings;
		float currentValue = settings.aperture;
		float newValue = currentValue;
		if (targetValue > currentValue) newValue += speed * Time.deltaTime;
		if (targetValue < currentValue) newValue -= speed * Time.deltaTime;
		newValue = Mathf.Clamp(newValue, min, max);
		if (newValue == currentValue) return;
		settings.aperture = newValue;
		this.postProcessing.profile.depthOfField.settings = settings;
	}
	public Vector3 ScreenToWorldPoint(float targetObjectZ) {
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = targetObjectZ - GameManager.instance.mainCamera.transform.position.z;
		return GameManager.instance.mainCamera.ScreenToWorldPoint(mousePosition);
	}
	public void PlayLevel(int level) {
		StartCoroutine(this.PlayLevelAsync(level));
	}

	private IEnumerator PlayLevelAsync(int level) {
		if (this.m_PlayingLevel.isLoaded) yield break;
		if (this.loadingLevel) yield break;
		this.loadingLevel = true;
		this.level = level;
		if (level == 1) {
			yield return SceneManager.LoadSceneAsync("Level_1", LoadSceneMode.Additive);
			yield return new WaitForEndOfFrame();
			Scene scene = SceneManager.GetSceneByName("Level_1");
			this.PlayStages(scene);
		}
		else if (level == 2) {
			yield return SceneManager.LoadSceneAsync("Level_2", LoadSceneMode.Additive);
			yield return new WaitForEndOfFrame();
			Scene scene = SceneManager.GetSceneByName("Level_2");
			this.PlayStages(scene);
		}
		else if (level == 3) {
			yield return SceneManager.LoadSceneAsync("Level_3", LoadSceneMode.Additive);
			yield return new WaitForEndOfFrame();
			Scene scene = SceneManager.GetSceneByName("Level_3");
			this.PlayStages(scene);
		}
		this.loadingLevel = false;
	}
	private void PlayStages(Scene level) {
		if (this.playingAnimation) return;
		this.m_PlayingLevel = level;
		this.currentStageIndex = -1;
		StartCoroutine(this.NextStageAsync());
	}
	private IEnumerator NextStageAsync(bool isBack = false, bool skipWait = false) {
		if (this.playingAnimation) yield break;;
		this.playingAnimation = true;

		Transform hideTrans = this.mainMenu.transform;
		if (this.currentStage) {
			hideTrans = this.currentStage.transform;
			yield return new WaitForSeconds(skipWait ? 0 : 2);
			foreach (var start in hideTrans.GetComponentsInChildren<StageStartPosition>()) {
				start.enabled = false;
			}
		}

		this.step++;

		Transform showTrans = this.mainMenu.transform;
		this.currentStageIndex++;
		if (!isBack && this.currentStageIndex < this.m_PlayingLevel.GetRootGameObjects().Length) {
			Stage newStage = Instantiate(this.m_PlayingLevel.GetRootGameObjects()[this.currentStageIndex]).GetComponent<Stage>();
			newStage.gameObject.SetActive(true);
			showTrans = newStage.transform;
		}
		else {
			yield return SceneManager.UnloadSceneAsync(this.m_PlayingLevel);
		}
		showTrans.gameObject.SetActive(true);

		Vector3 showStart = new Vector3(0, 0, 100);
		Vector3 showEnd = new Vector3(0, 0, 0);
		Vector3 hideStart = new Vector3(0, 0, 0);
		Vector3 hideEnd = this.mainCamera.transform.position;

		if (hideTrans.GetComponent<MainMenu>()) {
			hideEnd -= (this.level - 2) * new Vector3(3.5f, 0);
		}

		float t = 0;
		float v = 0;
		while (t < 0.999999f) {
			t = Mathf.SmoothDamp(t, 1, ref v, 0.25f);
			showTrans.position = Vector3.Lerp(showStart, showEnd, t);
			hideTrans.position = Vector3.Lerp(hideStart, hideEnd, t);
			yield return new WaitForEndOfFrame();
		}
		showTrans.position = showEnd;
		hideTrans.position = hideEnd;
		if (showTrans.GetComponent<Stage>()) {
			showTrans.GetComponent<Stage>().OnStartGame();
			this.currentStage = showTrans.GetComponent<Stage>();
		}
		else {
			this.currentStage = null;
		}
		if (hideTrans.GetComponent<Stage>()) {
			Destroy(hideTrans.gameObject);
		}
		else {
			hideTrans.gameObject.SetActive(false);
		}

		this.playingAnimation = false;
	}
}
