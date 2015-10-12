using UnityEngine;
using System.Collections;

public class PlayScreen : MonoBehaviour {


	public GameObject playButton;
	public GameObject unPauseButton;
	public GameObject settingButton;
	public GameObject runButton;
	public GameObject winButton;
	public GameObject loseButton;


	void Start()
	{
		unPauseButton.SetActive(false);
		settingButton.SetActive(false);
		runButton.SetActive(false);
		winButton.SetActive(false);
		loseButton.SetActive (false);
	}

	void Pause()
	{
		playButton.SetActive(false);
		unPauseButton.SetActive(true);
		settingButton.SetActive(true);
		runButton.SetActive(true);
		winButton.SetActive(false);
		loseButton.SetActive (false);
	}

	void Win()
	{
		playButton.SetActive(false);
		unPauseButton.SetActive(false);
		settingButton.SetActive(false);
		runButton.SetActive(false);
		winButton.SetActive(true);
		loseButton.SetActive (false);
	}

	void Lose()
	{
		playButton.SetActive(false);
		unPauseButton.SetActive(false);
		settingButton.SetActive(false);
		runButton.SetActive(false);
		winButton.SetActive(true);
		loseButton.SetActive (true);
	}
}
