using UnityEngine;
using System.Runtime.InteropServices;

public static class RedBullMindGamersPlatform {

	[DllImport("__Internal")]
	public static extern void GameOver(int level, int score);

	[DllImport("__Internal")]
	public static extern bool IsMobile();

}