using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {

	public int hp, atk;
	
	public int turnCount = 0;
	private GameObject gemHolder_obj;
	private GemHolder gemHolder_scr;
	public GameObject damager;
	public int XCoord
	{
		get
		{
			return Mathf.RoundToInt(transform.localPosition.x);
		}
	}
	public int YCoord
	{
		get
		{
			return Mathf.RoundToInt(transform.localPosition.y);
		}
	}

	// Use this for initialization
	void Start () {
		gemHolder_obj = GameObject.Find ("GemHolder");
		gemHolder_scr = gemHolder_obj.GetComponent<GemHolder> ();
		transform.parent = gemHolder_obj.transform;
		gemHolder_scr.monsterList.Add (gameObject);
	}
	

	void DoAction()
	{
		Debug.Log ("Monster DO ACTION");	
		if (turnCount % 3 == 0) {
			//Color tile
		} else if (turnCount % 3 == 1) {
			//Hit tiles
			HitTile (1,0);
		} else if (turnCount % 3 == 2) {
			//Moving
		}
		turnCount++;
		Invoke ("FinishAction", 2);
	}

	void FinishAction()
	{
		gemHolder_scr.FinishMonster ();
	}

	void HitTile(int x, int y)
	{
		GameObject d = Instantiate(damager, new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z), Quaternion.identity) as GameObject;
		d.SendMessage("SetDamage",atk);
	}

	void ApplyDamage(int damage)
	{
		Debug.Log ("Damage Applied");
		hp = hp - damage;
			if(hp <= 0)
		{
			Destroy(gameObject);
		}
	}

	void OnDestroy()
	{
		gemHolder_scr.monsterList.Remove(gameObject);
	}
}
