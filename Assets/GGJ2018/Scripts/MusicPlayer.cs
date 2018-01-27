using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

	private float nextShootTime;

	public AudioSource[] sound = new AudioSource[13];
	public static int index, currentIndex;

	// private int[] musicArray = { 1, 1, 3, 5, 5, 10,10 ,8,8,1,1,3,5,5,11,11,9,9,2,2,4,6,6,4,5,10,8,3,3,2,6,5,1};
	// Use this for initialization
	private int[] musicArray = { 1, 1, 1, 13, 13, 13, 13 };
	void Start()
	{
		AudioSource sound1 = GetComponent<AudioSource>();
		index = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (index <= currentIndex)
		{
			return;
		}
		currentIndex = index;
		print("index=  " + index + "  musicArray[index]-1  " + musicArray[index]);
		sound[musicArray[index] - 1].Play();
		if (index >= musicArray.Length)
		{
			index = 0;
		}
	}
}
