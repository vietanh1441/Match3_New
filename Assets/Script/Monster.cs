using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {
	public int maxHp;
	public int hp, atk;
	public bool up,down,right,left;
	public GameObject hpSlider_obj;
	public GameObject hud_obj;
	public HUDText hd;
	public UISlider hpSlider;
	public Vector2[] hitPos = new Vector2[5];
	//public Transform hpBar;
	public int turnCount = 0;
	private GameObject gemHolder_obj;
	private GemHolder gemHolder_scr;
	public GameObject damager;
	public GameObject warner; 
	public Vector3 hpBarScale = new Vector3(1,1,1);
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

		hp = maxHp;
		InitUI();
		//InitHudText();
		gemHolder_obj = GameObject.FindGameObjectWithTag ("GemHolder");
		gemHolder_scr = gemHolder_obj.GetComponent<GemHolder> ();
		transform.parent = gemHolder_obj.transform;
		//gemHolder_scr.monsterList.Add (gameObject);
		hd.Add("testing", Color.white, 2);
	}
	
	void InitUI()
	{
		//GameObject g = Instantiate(hpSlider_obj, transform.position, Quaternion.identity)as GameObject;
		//g.transform.parent = GameObject.Find ("UI Root").transform;
		GameObject g = NGUITools.AddChild(GameObject.Find ("UI Root"), hpSlider_obj);
		hpSlider = g.GetComponent<UISlider>();
		GameObject h = NGUITools.AddChild(GameObject.Find ("UI Root"), hud_obj);
		hd = h.GetComponent<HUDText>();
		h.SendMessage("FollowTarget",  transform);
		g.transform.localScale = hpBarScale;
		g.SendMessage("FollowTarget",transform);
		DisplayHpBar();
	}



	void DoAction()
	{
		Debug.Log ("Monster DO ACTION");	
		if (turnCount % 3 == 0) {
			Warning();
			//Color tile
		} else if (turnCount % 3 == 1) {
			//Hit tiles
			Attacking();
		} else if (turnCount % 3 == 2) {
			//Moving
		}
		turnCount++;
		Invoke ("FinishAction", 2);
	}

	void Attacking()
	{
		Vector2 pos;
	
		for(int i = 0; i < hitPos.Length; i++)
		{
			if(hitPos[i]!=null)
				pos = hitPos[i];

			if(up)
				HitTile(pos);
			if(down)
				HitTile(Downize(pos));
			if(right)
				HitTile(Rightize(pos));
			if(left)
				HitTile(Leftize(pos));
		}
		//HitTile (1,0);
	}

	void Warning()
	{
		Vector2 pos;
		
		for(int i = 0; i < hitPos.Length; i++)
		{
			if(hitPos[i]!=null)
				pos = hitPos[i];
			
			if(up)
				WarnTile(pos);
			if(down)
				WarnTile(Downize(pos));
			if(right)
				WarnTile(Rightize(pos));
			if(left)
				WarnTile(Leftize(pos));
		}
		//HitTile (1,0);
	}

	static Vector2 Downize(Vector2 pos)
	{
		Vector2 tmp;
		pos.x = -pos.x;
		pos.y = - pos.y;
		return pos;
	}

	static Vector2 Rightize(Vector2 pos)
	{
		Vector2 tmp;
		tmp.x = pos.x;
		pos.x = pos.y;
		pos.y = -tmp.x;
		return pos;
	}

	static Vector2 Leftize(Vector2 pos)
	{
		Vector2 tmp;
		tmp.x = pos.x;
		pos.x = -pos.y;
		pos.y = tmp.x;
		return pos;
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


	void HitTile(Vector2 v)
	{
		GameObject d = Instantiate(damager, new Vector3(transform.position.x + v.x, transform.position.y + v.y, transform.position.z), Quaternion.identity) as GameObject;
		d.SendMessage("SetDamage",atk);
	}

	void WarnTile(Vector2 v)
	{
		GameObject d = Instantiate(damager, new Vector3(transform.position.x + v.x, transform.position.y + v.y, transform.position.z), Quaternion.identity) as GameObject;
		d.SendMessage("SetDamage",-1);
	}

	void ApplyDamage(int damage)
	{
		Debug.Log ("Damage Applied");
		if(gemHolder_scr.ult_active)
		{
			damage = (int)damage*2;
		}
		if(gemHolder_scr.cap_active)
		{
			damage = (int)(damage*0.3f);
		}
		hp = hp - damage;
		hd.Add (-damage, Color.red, 3);
			if(hp <= 0)
		{
			if(gemHolder_scr.cap_active)
			{
				//Add a new char;
			}
			Destroy(gameObject);
		}

		DisplayHpBar();
	}

	void OnDestroy()
	{
		gemHolder_scr.monsterList.Remove(gameObject);
		gemHolder_scr.CheckGameStatus();
	}

	void DisplayHpBar()
	{

		Debug.Log (hp/maxHp);
		hpSlider.value = (float)hp/maxHp;
	}
}
