﻿using UnityEngine;
using System.Collections.Generic;

public class BaseMusicController : MonoBehaviour
{
	[SerializeField] private List<MusicController> musicList;

	[System.NonSerialized] public static BaseMusicController Instance;
	
	private void Awake()
	{
		Init();
	}

	private void Start()
	{
		// keep this object alive
		DontDestroyOnLoad(this.gameObject);
	}
	
	void Init()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	public void UpdateVolume()
	{
		foreach (MusicController item in musicList)
		{
			item.UpdateVolume();
		}
	}

	public void StopMusic(int num)
	{
		MusicController temp = musicList[num];

		if (temp)
		{
			temp.loopMusic = false;
			temp.FadeOut(15f);
		}
	}

	public void StopMusicButPlayToEnd(int num)
	{
		MusicController temp = musicList[num];

		if (temp)
		{
			temp.loopMusic = false;
		}
	}

	public void PlayMusic(int num)
	{
		MusicController temp = musicList[num];

		if (temp)
		{
			temp.loopMusic = true;
			temp.FadeIn(15f);
		}
	}
}
