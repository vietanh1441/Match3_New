using UnityEngine;
using System.Collections;

public class Central : MonoBehaviour {
	public GameObject GemHolder;
	public GameObject playScreen;

	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Debug.Log ("Pause");
			Pause();
		}
	}

	public void Play()
	{
		Instantiate(GemHolder, new Vector3(0,0,0), Quaternion.identity);
	}

	public void Ready()
	{
		playScreen= GameObject.Find ("PlayScreen");
		playScreen.SetActive(false);
	}

	public void Pause()
	{
		playScreen.SetActive (true);
		playScreen.SendMessage("Pause");
		//
	}

	public void UnPause()
	{
		playScreen.SetActive (false);
	}

	public void Win()
	{
		playScreen.SetActive (true);
		playScreen.SendMessage("Win");
		//Send message to win message;
	}

	public void Lose()
	{
		playScreen.SetActive (true);
		playScreen.SendMessage("Lose");
		//Send message to lose message;
	}


}
