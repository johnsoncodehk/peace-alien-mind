using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsPanel : UIController {

	public int boardStyle {
		get { return this.boardDropdown.value; }
	}

	public Button resumeButton;
	public Toggle musicToggle, soundToggle;
	public Dropdown boardDropdown;
	public AudioMixerGroup music, sound;
	public Text infoText;

	private string info;

	void Awake() {
		this.info = this.infoText.text;
		this.resumeButton.onClick.AddListener(AudioManager.instance.PlayClickButtonBack);
		this.resumeButton.onClick.AddListener(this.Hide);
		this.musicToggle.onValueChanged.AddListener((v) => {
			this.UpdateAudios();
		});
		this.soundToggle.onValueChanged.AddListener((v) => {
			this.UpdateAudios();
		});
		this.boardDropdown.onValueChanged.AddListener((v) => {
			foreach (Stage stage in FindObjectsOfType<Stage>()) {
				stage.UpdateBoardStyle(v);
			}
		});
		this.UpdateInfo();
	}
	protected override void OnEnable() {
		this.UpdateInfo();
	}

	public override void Show() {
		base.Show();
		GameManager.instance.showBlurry = true;
	}
	public override void Hide() {
		base.Hide();
		GameManager.instance.showBlurry = false;
	}

	private void UpdateInfo() {
		string text = this.info;
		text = text.Replace("{version}", Application.version);
		text = text.Replace("{data_version}", GameManager.instance.gameData.version.ToString());
		this.infoText.text = text;
	}
	private void UpdateAudios() {
		this.music.audioMixer.SetFloat("my_volume", this.musicToggle.isOn ? 0 : -80);
		this.sound.audioMixer.SetFloat("my_volume", this.soundToggle.isOn ? 0 : -80);
	}
}
