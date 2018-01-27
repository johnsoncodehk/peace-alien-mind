using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Testing : MonoBehaviour {

    Rigidbody2D planet;
    public float speed;
    public GameObject prefab;
    public GameObject tower;

    private float nextShootTime;

	// Use this for initialization
	void Start () {
        planet = GetComponent<Rigidbody2D>();
        this.nextShootTime = Random.Range(1, 2f);

    }
	

	// Update is called once per frame
	void FixedUpdate () {
        if (Time.time >= this.nextShootTime)
        {
            this.nextShootTime += Random.Range(1, 2f);
            Instantiate(prefab, tower.transform.position, planet.transform.rotation);
        }
        //if (Random.Range(0, 100) == 1)
        //{
        //    Instantiate(prefab, tower.transform.position , planet.transform.rotation);
        //}
        planet.MoveRotation( planet.rotation + speed *Time.deltaTime);
	}
}
