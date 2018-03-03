using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance {
		get {
			if (!AudioManager.m_Instance) {
				AudioManager.m_Instance = FindObjectOfType<AudioManager>();
			}
			return AudioManager.m_Instance;
		}
	}
	private static AudioManager m_Instance;

	public AudioSource musicSource, soundSource, signalEffectSource, signalEffectSource2;
	public AudioClip buttonClick, buttonBack, signalBounce;

	private bool m_IsChangeBackground;
	private AudioSource m_CurrentBackground;

	void Awake() {
		this.musicSource.gameObject.SetActive(false);
		this.m_CurrentBackground = Instantiate(this.musicSource);
		this.m_CurrentBackground.gameObject.SetActive(true);
		this.m_CurrentBackground.transform.SetParent(this.transform, false);
	}
	void Update() {
		if (!this.m_IsChangeBackground) {
			float remainTime = this.m_CurrentBackground.clip.length - this.m_CurrentBackground.time;
			if (remainTime < 5f) {
				AudioSource newBg = Instantiate(this.musicSource);
				newBg.gameObject.SetActive(true);
				newBg.transform.SetParent(this.transform, false);
				newBg.time = 5; // preload time
				StartCoroutine(this.ChangeVolumes(this.m_CurrentBackground, newBg));
				this.m_CurrentBackground = newBg;
			}
		}
	}

	public void PlayClickButton() {
		this.soundSource.PlayOneShot(this.buttonClick);
	}
	public void PlayClickButtonBack() {
		this.soundSource.PlayOneShot(this.buttonBack);
	}
	public void PlaySignal() {
		this.signalEffectSource.PlayOneShot(this.signalEffectSource.clip);
	}
	public void PlaySignal2() {
		StartCoroutine(this.PlayerSignal2Async());
	}
	public void PlaySignalBounce() {
		// this.soundSource.PlayOneShot(this.signalBounce);
		StartCoroutine(this.PlaySignalBounceAsync());
	}

	private IEnumerator PlaySignalBounceAsync() {
		AudioSource a = Instantiate(this.soundSource);
		a.pitch = Random.Range(1f, 2f);
		a.clip = this.signalBounce;
		a.Play();
		yield return new WaitForSeconds(this.signalBounce.length);
		Destroy(a.gameObject);
	}
	private IEnumerator PlayerSignal2Async() {
		AudioSource a = Instantiate(this.signalEffectSource2);
		a.pitch = Random.Range(2f, 3f);
		a.Play();
		yield return new WaitForSeconds(a.clip.length);
		Destroy(a.gameObject);
	}
	private IEnumerator ChangeVolumes(AudioSource oldAs, AudioSource newAs) {
		this.m_IsChangeBackground = true;
		while (oldAs.volume > 0) {
			oldAs.volume -= Time.deltaTime / 5;
			newAs.volume = 1 - oldAs.volume;
			yield return new WaitForEndOfFrame();
		}
		newAs.volume = 1;
		Destroy(oldAs.gameObject);
		this.m_IsChangeBackground = false;
	}
}
