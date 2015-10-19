using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Central : MonoBehaviour {
	public GameObject GemHolder;
	public GameObject playScreen;
	public GameObject[] charPrefab = new GameObject[2];
	public List<GameObject> monsterPrefab = new List<GameObject>();
	public List<int> char1Skill = new List<int>();
	public List<int> char1Lvl = new List<int>();
	public List<int> char2Skill = new List<int>();
	public List<int> char2Lvl = new List<int>();
	public List<int> char1Exp = new List<int>();
	public List<int> char2Exp = new List<int>();
	public int char1_maxHp;
	public int char2_maxHp;
	public int char1_dmg;
	public int char2_dmg;
	public int char1_def;
	public int char2_def;

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);

		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}
	}



	void GoInside(List<GameObject> l)
	{
		monsterPrefab.Clear ();
		foreach ( GameObject m in l)
		{
			monsterPrefab.Add (m);
		}
		Application.LoadLevel("test1");
	}
	
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
		if(playScreen != null)
		{
		playScreen.SetActive (true);
		playScreen.SendMessage("Win");
		}
		//Send message to win message;
	}

	public void Lose()
	{
		if(playScreen != null)
		{
		playScreen.SetActive (true);
		playScreen.SendMessage("Lose");
		}
		//Send message to lose message;
	}


}
