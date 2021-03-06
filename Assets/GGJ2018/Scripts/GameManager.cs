﻿using System.Linq;
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
	public GameConfig gameConfig;
	public GameData gameData;
	public int step;
	public float score;

	// UI
	public MainMenu mainMenu;
	public SettingsPanel settingsPanel;
	public Button settingsButton, backButton;
	public GameMessage gameMessage;

	// runtime
	[HideInInspector] public Stage currentStage;

	private LevelData m_PlayingLevel;
	private int currentStageIndex;
	private bool runningNext;
	private bool loadingLevel;
	private int level;
	private Image backButtonImage;

	void Awake() {
		this.settingsButton.onClick.AddListener(AudioManager.instance.PlayClickButton);
		this.settingsButton.onClick.AddListener(GameManager.instance.settingsPanel.Show);
		this.backButton.onClick.AddListener(AudioManager.instance.PlayClickButtonBack);
		this.backButton.onClick.AddListener(() => {
			StartCoroutine(this.NextStageAsync(true));
		});
		this.backButtonImage = this.backButton.GetComponent<Image>();
		this.postProcessing.profile = Instantiate(this.postProcessing.profile);
	}
	void Start() {
		try {
			if (RedBullMindGamersPlatform.IsMobile()) {
				// this.postProcessing.enabled = false;
				Destroy(this.postProcessing);
			}
		}
		catch { }

		StartCoroutine(GameConfig.Load((gameconfig) => {
			this.gameConfig = gameconfig;
		}));

		this.gameMessage.ShowMessage("Downloading...", true);
		StartCoroutine(GameData.Load(this.gameConfig.gameDataUpdate, (gameData, errorCode) => {
			this.gameData = gameData;
			if (this.gameConfig.gameDataUpdate.enabled) {
				if (errorCode == 0) {
					this.gameMessage.ShowMessage(
						"Data Update Completed\nData Version: {data_version}"
						.Replace("{data_version}", this.gameData.version.ToString())
					);
				}
				else {
					this.gameMessage.ShowMessage(
						"Data Update Failed\nError Code: {error_code}\nData Version: {data_version}"
						.Replace("{error_code}", errorCode.ToString())
						.Replace("{data_version}", this.gameData.version.ToString())
					);
				}
			}
			else {
				this.gameMessage.ShowMessage(
					"Data Version: {data_version}"
					.Replace("{data_version}", this.gameData.version.ToString())
				);
			}
		}));

		if (this.gameConfig.debugMode) {
			this.gameMessage.ShowMessage("Debug Mode Enabled");
		}
	}
	void Update() {
		if (this.gameConfig.debugMode && Input.GetButtonDown("Horizontal")) {
			if (!this.currentStage) return;
			if (Input.GetAxisRaw("Horizontal") > 0) {
				this.NextStage(0);
			}
			else {
				this.currentStageIndex -= 2;
				this.currentStageIndex = Mathf.Max(this.currentStageIndex, -1);
				this.NextStage(0);
			}
		}
		this.SetBlurry(this.showBlurry ? 0.1f : 5);
		this.backButtonImage.raycastTarget = !this.runningNext && this.currentStage;
		this.backButton.interactable = m_PlayingLevel != null;
	}

	public void NextStage(float addScore) {
		this.score += addScore;
		StartCoroutine(this.NextStageAsync());
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
		if (this.m_PlayingLevel != null) return;
		if (this.gameData.levels.Count < level) return;
		this.level = level;
		this.PlayStages(this.gameData.levels[level - 1]);
	}

	private void PlayStages(LevelData level) {
		if (this.runningNext) return;
		this.m_PlayingLevel = level;
		this.currentStageIndex = -1;
		this.score = 0;
		this.NextStage(0);
	}
	private IEnumerator NextStageAsync(bool isBack = false) {
		if (this.runningNext) yield break;
		this.runningNext = true;

		Transform hideTrans = this.mainMenu.transform;
		if (this.currentStage) {
			hideTrans = this.currentStage.transform;
			foreach (var start in this.currentStage.GetComponentsInChildren<StageStartPosition>()) {
				start.enabled = false;
			}
		}

		this.step++;

		Transform showTrans = this.mainMenu.transform;
		this.currentStageIndex++;
		if (!isBack && this.currentStageIndex < this.m_PlayingLevel.stages.Count) {
			StageData stageData = this.m_PlayingLevel.stages[this.currentStageIndex];

			Stage stage = Instantiate(AssetsManager.instance.stage);
			foreach (StageObjectData stageObjectData in stageData.objects) {
				GridTransform prefab = AssetsManager.instance.stageObjects.Find(sod => sod.name == stageObjectData.name);
				if (!prefab) {
					Debug.LogError("Prefab Missing: " + stageObjectData.name);
					this.runningNext = false;
					this.NextStage(0);
					yield break;
				}
				GridTransform stageObject = Instantiate(prefab, Vector3.zero, Quaternion.identity, stage.transform);
				stageObject.position = stageObjectData.position;
				stageObject.direction = stageObjectData.direction;
			}
			showTrans = stage.transform;
			showTrans.name = stageData.name;
		}
		else {
			if (!isBack) {
				int finalScore = Mathf.FloorToInt(Mathf.Clamp(this.score, 0, 100));
				this.gameMessage.ShowMessage("Score: " + finalScore);
				try {
					RedBullMindGamersPlatform.GameOver(this.level, finalScore);
				}
				catch { }
			}
			this.m_PlayingLevel = null;
		}

		GameManager.instance.player.energys.show = false;

		yield return StartCoroutine(this.ShowHide(showTrans, hideTrans));

		this.currentStage = null;
		if (showTrans.GetComponent<Stage>()) {
			showTrans.GetComponent<Stage>().OnStartGame();
			GameManager.instance.player.energys.show = true;
			GameManager.instance.player.energys.remain = 10;
			this.currentStage = showTrans.GetComponent<Stage>();
			
			if (this.gameConfig.debugMode) {
				this.gameMessage.ShowMessage(this.currentStage.name, true);
			}
		}
		if (hideTrans.GetComponent<Stage>()) {
			Destroy(hideTrans.gameObject);
		}

		this.runningNext = false;
	}
	private IEnumerator ShowHide(Transform showTrans, Transform hideTrans) {
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
		while (t < 0.9999f) {
			t = Mathf.SmoothDamp(t, 1, ref v, 0.25f);
			showTrans.position = Vector3.Lerp(showStart, showEnd, t);
			hideTrans.position = Vector3.Lerp(hideStart, hideEnd, t);
			yield return new WaitForEndOfFrame();
		}
		showTrans.position = showEnd;
		hideTrans.position = hideEnd;

		hideTrans.gameObject.SetActive(false);
	}
}
