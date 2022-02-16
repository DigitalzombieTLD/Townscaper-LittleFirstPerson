using System;
using MelonLoader;
using Harmony;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using Placemaker;
using System.Collections.Generic;

namespace LittleFirstPerson
{
	public static class AudioMain
	{

		public static AudioSource playerSource;
		public static Dictionary<String, AudioClip> audioLib = new Dictionary<string, AudioClip>();
		public static bool isPlaying;
		public static string nowPlaying;

		public static void LoadAudioFromBundle()
		{
			audioLib.Clear();
			audioLib.Add("WALK01_TILE", LittleFirstPersonMain.littleFirstPersonBundle.LoadAsset<AudioClip>("Footsteps_Tile_Walk_08"));
			audioLib.Add("RUN01_TILE", LittleFirstPersonMain.littleFirstPersonBundle.LoadAsset<AudioClip>("Footsteps_Tile_Run_03"));

			audioLib.Add("WALK01_WATER", LittleFirstPersonMain.littleFirstPersonBundle.LoadAsset<AudioClip>("Footsteps_WaterV1_Walk_06"));
			audioLib.Add("RUN01_WATER", LittleFirstPersonMain.littleFirstPersonBundle.LoadAsset<AudioClip>("Footsteps_WaterV1_Walk_09"));

			audioLib.Add("JETPACK", LittleFirstPersonMain.littleFirstPersonBundle.LoadAsset<AudioClip>("jetpack"));
		}

		public static void PlayRepeat(string clipName)
		{
			if(nowPlaying!=clipName)
			{
				StopPlay();
			}

			if (!isPlaying)
			{
				playerSource.clip = audioLib[clipName];
				nowPlaying = clipName;
				playerSource.Play();
				isPlaying = true;
			}
		}
		
		public static void StopPlay()
		{
			if (isPlaying)
			{
				playerSource.Stop();
				isPlaying = false;
			}
		}
	}
}
