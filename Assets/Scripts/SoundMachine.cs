using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*
 * 
 * 
 * Copyright (c) 2017 Gento Morikawa
 * Released under the MIT license
 * 
 * 
 * 
 * 
*/

[RequireComponent (typeof(AudioSource))]
public class SoundMachine : Singleton<SoundMachine>
{
	private List<AudioClip> audioClips = new List<AudioClip> ();

	const string bgmPass = "BGM";

	private AudioSource audioSpeaker;
	private int currentNumber;
	private int previousNumber;
	private float switchingTimeToNextClip = 0f;

	private bool isNotFirst = false;

	override protected void Awake ()
	{
		base.Awake ();
		AudioClip[] audioclips = Resources.LoadAll <AudioClip> (bgmPass);
		foreach (AudioClip clip in audioclips) {
			audioClips.Add (clip);
		}
	}

	void Start ()
	{
		audioSpeaker = this.gameObject.GetComponent<AudioSource> ();
		audioSpeaker.loop = true;
		StartCoroutine (BGMPlay ());
	}

	int InitClip (int i)
	{
		return Random.Range (i, audioClips.Count);
	}

	//
	IEnumerator BGMPlay ()
	{
		while (true) {
			int i = InitClip (0);
			if (previousNumber == i && isNotFirst) {
				if (i == audioClips.Count) {
					i -= InitClip (1);
				} else {
					i += InitClip (previousNumber + 1);
				}
			}
			audioSpeaker.clip = audioClips [i];
			previousNumber = i;
			if (!audioSpeaker.isPlaying)
				audioSpeaker.Play ();
			if (!isNotFirst)
				isNotFirst = true;
			yield return new WaitForSeconds (audioSpeaker.clip.length);
		}
	}
}
