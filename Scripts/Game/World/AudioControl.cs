using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioControl : MonoBehaviour {

	public AudioMixer[] mixer;
	public AudioSource[] source;
	public AudioClip[] ambience;
	public AudioClip[] effect;



	public void PlaySoundEffect (int _effect){

		source [1].clip = effect [_effect];
		source [1].Play ();
	}
	public void PlayAmbience (int _ambience){
		source [0].clip = ambience [_ambience];
		source [0].Play ();
	}
}
