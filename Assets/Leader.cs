using UnityEngine;
using System.Collections;

public class Leader : MonoBehaviour {
	public Vector3 target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(transform.position, target) > 0.1f)
		{
			transform.position = Vector3.MoveTowards(transform.position, target, 0.1f);
		}
		else
		{
			transform.position = target;
		}
	}

	void SetTarget(Vector3 t)
	{
		target = t;
	}
}
