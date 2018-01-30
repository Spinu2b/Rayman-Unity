using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour {

	public GameObject soundsSource;
	public AudioClip footStep;
	public AudioClip groundRoll;
	public AudioClip jumpSound1;
	public AudioClip jumpSound2;
	public AudioClip helicopterSound;

	public void PlayFootstep() {
		AudioSource.PlayClipAtPoint(footStep, soundsSource.transform.position, 1f);
	}

	public void PlayGroundRoll() {
		AudioSource.PlayClipAtPoint(groundRoll, soundsSource.transform.position, 1f);
	}

	public void PlayRunJump() {
		AudioSource.PlayClipAtPoint (jumpSound1, soundsSource.transform.position, 1f);
	}

	public void PlayJump() {
		AudioSource.PlayClipAtPoint (jumpSound2, soundsSource.transform.position, 1f);
	}

	public void PlayHelicopter() {
		//AudioSource.PlayClipAtPoint (helicopterSound, soundsSource.transform.position, 0.75f);
	}

}
