using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsPanel : UIController {

	public Button resumeButton;
	public Toggle musicToggle, soundToggle;
	public AudioMixerGroup music, sound;
	public Text infoText;
	
	void Awake() {
		this.resumeButton.onClick.AddListener(AudioManager.instance.PlayClickButtonBack);
		this.resumeButton.onClick.AddListener(this.Hide);
		this.musicToggle.onValueChanged.AddListener((v) => {
			this.UpdateAudios();
		});
		this.soundToggle.onValueChanged.AddListener((v) => {
			this.UpdateAudios();
		});
		this.infoText.text = this.infoText.text.Replace("{version}", Application.version);
	}

	public override void Show() {
		base.Show();
		GameManager.instance.showBlurry = true;
	}
	public override void Hide() {
		base.Hide();
		GameManager.instance.showBlurry = false;
	}

	private void UpdateAudios() {
		this.music.audioMixer.SetFloat("my_volume", this.musicToggle.isOn ? 0 : -80);
		this.sound.audioMixer.SetFloat("my_volume", this.soundToggle.isOn ? 0 : -80);
	}
}
