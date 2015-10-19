using UnityEngine;
using System.Collections;

public class newButton : MonoBehaviour {

	
	public string hit;
	private GameObject central;
	private Central central_scr;
	public int value;
	private bool on;
	public GameObject drag;
	
	// Use this for initialization
	void Start()
	{
		on = false;
		central = GameObject.Find("Central");
		central_scr = central.GetComponent<Central>();
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}
	
	void OnMouseDown()
	{
		Invoke(hit, 0.1f);
	}

	void Play()
	{
		central_scr.Play();
	}

	void UnPause()
	{
		central_scr.UnPause();
	}

	void Setting()
	{
		//nothing for now
	}

	void Run()
	{
		//nothing for now
	}

	void Win()
	{
		//nothing for now
	}

	void Lose()
	{
		//nothing for now
	}
}
