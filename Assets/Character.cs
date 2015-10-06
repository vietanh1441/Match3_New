using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	public int hp, atk;

	//The type of character: 
	/// <summary>
	/// 1: swordman
	/// 2: bowman
	/// 3: wizard
	/// 4: healer
	/// 5: ...
	/// </summary>
	public int type;
	public GameObject damager;
	private bool up = false, down= false, right=false, left=false;

	private GameObject gemHolder_obj;
	private GemHolder gemHolder_scr;
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ApplyDamage(int damage)
	{
		hp = hp - damage;
		if (hp <= 0) {
			Death();
		}
	}

	public void Death()
	{
		Debug.Log ("Death");
	}

	public void DoAction()
	{
		//Debug.Log ("Character Do Action");
		if(type==1)
		{
			SwordMan();
		}

		Invoke ("FinishAction", 1);
	}

	int SwordMan()
	{
		//Debug.Log ("SwordMan");
		//up = CheckSwordManUp();
		//down = CheckSwordManDown();
		if(CheckSwordManUp())
		{
			HitTile(0,1);
			HitTile(0, 2);
			//Debug.Log ("Success");
		}
		if(CheckSwordManDown())
		{
			HitTile(0,-1);
			HitTile(0,-2);
		}
		if(CheckSwordManRight())
		{
			HitTile(1,0);
			HitTile(2,0);
		}
	    if(CheckSwordManLeft())
		{
			HitTile(-1,0);
			HitTile(-2,0);
		}

		return 1;
	}

	/// <summary>
	/// - 0 -
	/// - x -
	/// 0 - 0
	/// </summary>
	/// <returns><c>true</c>, if sword man down was checked, <c>false</c> otherwise.</returns>
	bool CheckSwordManUp()
	{
		GameObject a,b,c;
		a = gemHolder_scr.gems[XCoord-1,YCoord-1];
		b = gemHolder_scr.gems[XCoord+1,YCoord-1];
		c = gemHolder_scr.gems[XCoord,YCoord+1];
		//Debug.Log ("CHeck up");
		if(a == null)
			return false;
		if(b == null)
			return false;
		if(c== null)
			return false;
		if(a.transform.CompareTag("Sword") && b.transform.CompareTag("Sword")&& c.transform.CompareTag("Sword") )
		{
			a.SendMessage("Mark",1);
			b.SendMessage("Mark",1);
			c.SendMessage("Mark",1);
			return true;
		}
		return false;
	}

	/// <summary>
	/// 0 - 0
	/// - x -
	/// - 0 -
	/// </summary>
	/// <returns><c>true</c>, if sword man down was checked, <c>false</c> otherwise.</returns>
	bool CheckSwordManDown()
	{
		GameObject a,b,c;
		a = gemHolder_scr.gems[XCoord-1,YCoord+1];
		b = gemHolder_scr.gems[XCoord+1,YCoord+1];
		c = gemHolder_scr.gems[XCoord,YCoord-1];
		//Debug.Log ("CHeck up");
		if(a == null)
			return false;
		if(b == null)
			return false;
		if(c== null)
			return false;
		if(a.transform.CompareTag("Sword") && b.transform.CompareTag("Sword")&& c.transform.CompareTag("Sword") )
		{
			a.SendMessage("Mark",1);
			b.SendMessage("Mark",1);
			c.SendMessage("Mark",1);
			return true;
		}
		return false;
	}

	/// <summary>
	/// 0 - -
	/// - x 0
	/// 0 - -
	/// </summary>
	/// <returns><c>true</c>, if sword man down was checked, <c>false</c> otherwise.</returns>
	bool CheckSwordManRight()
	{
		GameObject a,b,c;
		a = gemHolder_scr.gems[XCoord-1,YCoord+1];
		b = gemHolder_scr.gems[XCoord-1,YCoord-1];
		c = gemHolder_scr.gems[XCoord+1,YCoord];
		//Debug.Log ("CHeck up");
		if(a == null)
			return false;
		if(b == null)
			return false;
		if(c== null)
			return false;
		if(a.transform.CompareTag("Sword") && b.transform.CompareTag("Sword")&& c.transform.CompareTag("Sword") )
		{
			a.SendMessage("Mark",1);
			b.SendMessage("Mark",1);
			c.SendMessage("Mark",1);
			return true;
		}
		return false;
	}

	/// <summary>
	/// - - 0
	/// 0 x -
	/// - - 0
	/// </summary>
	/// <returns><c>true</c>, if sword man down was checked, <c>false</c> otherwise.</returns>
	bool CheckSwordManLeft()
	{
		GameObject a,b,c;
		a = gemHolder_scr.gems[XCoord+1,YCoord+1];
		b = gemHolder_scr.gems[XCoord+1,YCoord-1];
		c = gemHolder_scr.gems[XCoord-1,YCoord];
		//Debug.Log ("CHeck up");
		if(a == null)
			return false;
		if(b == null)
			return false;
		if(c== null)
			return false;
		if(a.transform.CompareTag("Sword") && b.transform.CompareTag("Sword")&& c.transform.CompareTag("Sword") )
		{
			a.SendMessage("Mark",1);
			b.SendMessage("Mark",1);
			c.SendMessage("Mark",1);
			return true;
		}

			
		return false;
	}

	void FinishAction()
	{
		gemHolder_scr.FinishChar ();
	}

	void HitTile(int x, int y)
	{
		GameObject d = Instantiate(damager, new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z), Quaternion.identity) as GameObject;
		d.SendMessage("SetDamage",atk);
	}

	void OnDestroy()
	{
		gemHolder_scr.characterList.Remove(gameObject);
	}
}
