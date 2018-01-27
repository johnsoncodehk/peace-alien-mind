using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    private float nextShootTime;

    public AudioSource[] sound = new AudioSource[13];
    //public AudioSource sound2;
    //public AudioSource sound3;
    //public AudioSource sound4;
    //public AudioSource sound5;
    //public AudioSource sound6;
    //public AudioSource sound7;
    //public AudioSource sound8; // 1'
    //public AudioSource sound9; // 2'
    //public AudioSource sound10;// 3'
    //public AudioSource sound11;// 4'
    //public AudioSource sound12;// 5'
    //public AudioSource sound13;// 6'
    static int index;

   // private int[] musicArray = { 1, 1, 3, 5, 5, 10,10 ,8,8,1,1,3,5,5,11,11,9,9,2,2,4,6,6,4,5,10,8,3,3,2,6,5,1};
    // Use this for initialization
    private int[] musicArray = { 1,1,1, 13,13,13,13 };
    void Start () {
     AudioSource sound1 = GetComponent<AudioSource>();
        index = 0;
    }
	
	// Update is called once per frame
	void Update () {
        
            if (Time.time >= this.nextShootTime)
            {
                
                this.nextShootTime += 1;

                print("index=  " + index + "  musicArray[index]-1  " + musicArray[index]);
                sound[musicArray[index]-1].Play();
           // sound[musicArray2[index] - 1].Play();
            index++;
            if (index >= musicArray.Length)
            {
                index = 0;
            }
       
        }
            //sound[musicArray[0]].Play();

            //for (int i = 0; i < musicArray.Length; i++)
            //{
            //    sound[musicArray[i ]].Play();
            //}

            
       
	}
}
