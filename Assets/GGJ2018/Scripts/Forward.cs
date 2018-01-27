using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forward : MonoBehaviour {

    Rigidbody2D ball;


	// Use this for initialization
	void Start () {
        ball = GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {
        ball.velocity = ball.transform.right * 1;

		
	}
}
