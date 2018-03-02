using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridTransform))]
public class Signal : MonoBehaviour {

	public AnimationCurve sizeCurve, colorCurve;
	public Vector3 startSize, endSize;
	public Color startColor, endColor;
	public float lifeTime;
	public float speed = 1;
	public int shootAt;
	public Transform target;

	public float smoothTime = 0.2f, maxSpeed = 10, deltaTime = 0.1f;

	[HideInInspector] public bool isLast;
	[HideInInspector] public Transform createBy;
	[HideInInspector] public GridTransform gridTransform;

	private float m_StartTime;
	private SpriteRenderer m_SpriteRenderer;
	private Rigidbody2D m_Rigidbody;
	private Vector2 m_GoSpeed;

	void Awake() {
		this.m_StartTime = Time.time;
		this.m_SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
		this.m_Rigidbody = this.GetComponent<Rigidbody2D>();
		this.gridTransform = this.GetComponent<GridTransform>();
		this.gridTransform.ingnorePosition = true;

		this.m_SpriteRenderer.transform.localScale = Vector3.zero;
	}
	void Update() {
		if (this.target) {
			if (Vector3.Distance(target.position, this.transform.position) < 0.1f) {
				Destroy(this.gameObject);
			}
			else {
				float startD = Vector3.Distance(this.transform.position, target.position);
				// this.m_SpriteRenderer.transform.localScale -= Vector3.one * Time.deltaTime * 0.5f;
				this.transform.position = Vector2.SmoothDamp(this.transform.position, target.position, ref this.m_GoSpeed, this.smoothTime, this.maxSpeed, this.deltaTime);
				float currendD = Vector3.Distance(this.transform.position, target.position);
				this.m_SpriteRenderer.transform.localScale = startSize * (currendD / startD);
			}
		}
		else {
			if (this.shootAt != GameManager.instance.step) {
				this.lifeTime -= Time.deltaTime * 8;
			}
			if (lifeTime > 0) {
				float currentTime = Time.time - this.m_StartTime;
				float t = currentTime / lifeTime;
				this.m_SpriteRenderer.transform.localScale = Vector3.Lerp(this.startSize, this.endSize, this.sizeCurve.Evaluate(t));
				this.m_SpriteRenderer.color = Color.Lerp(this.startColor, this.endColor, this.colorCurve.Evaluate(t));
				this.m_Rigidbody.velocity = this.m_Rigidbody.transform.up * this.speed;
				if (t > 1) {
					Destroy(this.gameObject);
				}
			}
			else {
				Destroy(this.gameObject);
			}
		}
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (this.target) return;
		ISignalReceiverHandler receiver = other.GetComponent<ISignalReceiverHandler>();
		if (receiver != null && other.transform != this.createBy) {
			receiver.OnSignalReceiver(this);
		}
	}

	public void ResetStartTime() {
		this.m_StartTime = Time.time;
	}
	public static IEnumerator ShootAsync(Signal signal, Vector2 shootPos, Direction direction, Transform createBy, int shootAt) {
		Signal.Create(signal, shootPos, direction, createBy, shootAt);
		yield return new WaitForSeconds(0.05f);
		Signal.Create(signal, shootPos, direction, createBy, shootAt);
		yield return new WaitForSeconds(0.05f);
		Signal.Create(signal, shootPos, direction, createBy, shootAt).isLast = true;
	}
	public static Signal Create(Signal prefab, Vector2 pos, Direction direction, Transform createBy, int shootAt) {
		var signal = Instantiate(prefab);
		signal.transform.position = pos;
		signal.gridTransform.direction = direction;
		signal.createBy = createBy;
		signal.shootAt = shootAt;
		return signal;
	}
}
