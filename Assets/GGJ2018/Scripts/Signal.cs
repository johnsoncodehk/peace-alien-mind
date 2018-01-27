using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal : MonoBehaviour {

	public AnimationCurve sizeCurve, colorCurve;
	public Vector3 startSize, endSize;
	public Color startColor, endColor;
	public float lifeTime;
	public bool isFirst, isLast;
	public float speed = 1;
	
	public float smoothTime = 0.2f, maxSpeed = 10, deltaTime = 0.1f;

	[HideInInspector] public Signal child;
	[HideInInspector] public Player player;

	private float m_StartTime;
	private SpriteRenderer m_SpriteRenderer;
	private Rigidbody2D m_Rigidbody;
	private Collider2D m_Collider;
	private Vector2 m_GoSpeed;
	private int m_State; // 1 = do not think
	
	void Awake() {
		this.m_StartTime = Time.time;
		this.m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
		this.m_Rigidbody = this.GetComponent<Rigidbody2D>();
		this.m_Collider = this.GetComponent<Collider2D>();

		this.transform.localScale = Vector3.zero;
	}
	void Update() {
		if (this.m_State == 1) {
			return;
		}
		if (this.player) {
			this.transform.position = Vector2.SmoothDamp(this.transform.position, this.player.attractPoint.transform.position, ref this.m_GoSpeed, this.smoothTime, this.maxSpeed, this.deltaTime);
			if (this.isLast) {
				this.player.StartCoroutine(this.player.Shoot());
			}
			StartCoroutine(this.StartSmall());
			this.m_State = 1;
		}
		else {
			float currentTime = Time.time - this.m_StartTime;
			float t = currentTime / lifeTime;
			this.transform.localScale = Vector3.Lerp(this.startSize, this.endSize, this.sizeCurve.Evaluate(t));
			this.m_SpriteRenderer.color = Color.Lerp(this.startColor, this.endColor, this.colorCurve.Evaluate(t));
			this.m_Rigidbody.velocity = this.m_Rigidbody.transform.up * this.speed;
			if (t > 1) {
				Destroy(this.gameObject);
			}
		}
	}
	public void OnTriggerEnter2D(Collider2D other) {
		Player player = other.GetComponent<Player>();
		if (player && this.isFirst) {
			this.ToPlayer(player);
		}
		Planet planet = other.GetComponent<Planet>();
		if (planet) {
			planet.saveSignal++;
			if (planet.saveSignal >= 3) {
				planet.saveSignal = 0;
				planet.LevelUp();
				MusicPlayer.index++;
			}
			Destroy(this.gameObject);
		}
	}

	private void ToPlayer(Player player) {
		this.player = player;
		if (this.child) {
			this.child.ToPlayer(player);
		}
	}
	private IEnumerator StartSmall() {
		while (this.transform.localScale.x > 0.1f) {
			this.transform.localScale -= Vector3.one * Time.deltaTime * 0.5f;
			yield return new WaitForEndOfFrame();
		}
		Destroy(this.gameObject);
	}
}
