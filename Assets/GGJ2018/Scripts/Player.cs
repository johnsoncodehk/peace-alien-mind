using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Character))]
public class Player : MonoBehaviour
	, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {

	public enum Control {
		None,
		Program,
		Mouse,
		Keybroad,
	}

	public Control control = Control.None;
	public Signal signal;
	public PlayerEnergys energys;
	[HideInInspector] public Character character;

	void Awake() {
		this.character = this.GetComponent<Character>();
	}
	void Update() {
		if (this.control == Control.Mouse) {
			this.character.enabledMove = true;
			this.character.targetPosition = GameManager.instance.ScreenToWorldPoint(this.transform.position.z);
		}
		else if (this.control == Control.Keybroad) {
			this.character.enabledMove = true;
			Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
			this.character.targetPosition = this.transform.position + input * 10;
		}
		else if (this.control == Control.Program) {
			this.character.enabledMove = true;
		}
	}

	public void OnPointerDown(PointerEventData eventData) { }
	public void OnPointerUp(PointerEventData eventData) { }
	public void OnPointerClick(PointerEventData eventData) {
		// if (GameManager.instance.currentStage && this.energys.remain <= 0) return;
		this.energys.remain--;
		GameManager.instance.step++;
		AudioManager.instance.PlaySignal();
		StartCoroutine(Signal.ShootAsync(this.signal, this.transform.position, Direction.Up, this.transform, GameManager.instance.step));
	}
}
