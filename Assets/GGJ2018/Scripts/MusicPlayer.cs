using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

	private float nextShootTime;

	public AudioSource[] sound = new AudioSource[13];
	public static int index, currentIndex;

    private int[] musicArray = { 1, 1, 3, 5, 5,3, 12, 12,3, 10, 10,3,
                                 1, 1, 3, 5, 5,4, 12, 12,4, 11, 11,4,
                                 0, 0, 2, 6, 6,4, 13, 13,4, 11, 11,4,
                                 0, 0, 2, 6, 6,3, 13, 13,3, 10, 10,3,
                                 1, 1, 3, 5, 8,5, 15, 15,3, 12, 12,3,
                                 1, 1, 3, 5, 8,6, 15, 15,4, 13, 13,4,
                                 2, 2, 4, 6, 6,4,4,0 ,4, 5, 10,3,3,3,
                                 8, 3, 3,3, 2, 6,6, 5,5, 1 ,1};
    private int[] musicArray2 = {1, 1, 3, 5, 1,3, 3, 1,3, 3, 1,3,//tick
                                 1, 1, 3, 3, 2,4, 4, 2,4, 11, 2,4,//tick
                                 0, 0, 2, 6, 0,4, 13, 0,4, 11, 0,4,
                                 0, 0, 2, 6, 1,3, 13, 1,3, 10, 1,3,//tick
                                 1, 1, 3, 5, 3,5, 15, 3,3, 12, 3,3,//
                                 1, 1, 3, 5, 4,6, 15, 4,6, 13, 4,6,//
                                 2, 2, 4, 6, 2,4,4,0 ,4, 5, 1,3,3,3,
                                 5, 5, 4,3, 2, 5,6, 5,5, 1 ,1};
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

        print("index=  " + index + "  musicArray[index]  " + musicArray[index]);
        sound[musicArray[index]].Play();
        sound[musicArray2[index]].Play();
        if (index >= musicArray.Length)
		{
			index = 0;
		}
	}
}
