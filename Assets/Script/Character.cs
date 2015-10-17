﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {

	/// <summary>
	/// what is needed to inilization of a character
	/// A Prefab of model
	/// maxHP, hp, skillset
	/// central will instantiate and initialize the character and then give it to gemHolder
	/// GemHolder will then put the transform inside the room
	/// </summary>


	public int max_hp = 5;
	public int hp, dmg, def;
	public List<int> charSkill = new List<int>();
	public List<int> charLvl = new List<int>();
	private GameObject central;
	private Central central_scr;
	//The type of character: 
	/// <summary>
	/// 1: swordman
	/// 2: bowman
	/// 3: wizard
	/// 4: healer
	/// 5: ...
	/// </summary>
	public int type;
	public SkillSet skillSet;

	//list of available skill and list of level of skill

	public GameObject damager;
	private bool up = false, down= false, right=false, left=false;
	public GameObject hpSlider_obj;
	public GameObject hud_obj;
	public HUDText hd;
	public UISlider hpSlider;
	//public Transform hpBar, damageHUD;
	public Vector2[] hitPos = new Vector2[5];
	private GameObject gemHolder_obj;
	private GemHolder gemHolder_scr;
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

		Sync();
		//Get info from central

		skillSet = GameObject.Find("SkillSet").GetComponent<SkillSet>();
		hp = max_hp;
		InitUI();
		gemHolder_obj = GameObject.FindGameObjectWithTag ("GemHolder");
		gemHolder_scr = gemHolder_obj.GetComponent<GemHolder> ();
	//	DisplayHpBar();
	}

	void Sync()
	{
		central = GameObject.Find("Central");
		central_scr = central.GetComponent<Central>();
		if(type == 1)
		{
			max_hp = central_scr.char1_maxHp;
			dmg = central_scr.char1_dmg;
			def = central_scr.char1_def;
			foreach( int skill in central_scr.char1Skill)
			{
				charSkill.Add (skill);
			}
			foreach(int lvl in central_scr.char1Lvl)
			{
				charLvl.Add (lvl);
			}
		}
		if(type == 2)
		{
			max_hp = central_scr.char2_maxHp;
			dmg = central_scr.char2_dmg;
			def = central_scr.char2_def;
			foreach( int skill in central_scr.char2Skill)
			{
				charSkill.Add (skill);
			}
			foreach(int lvl in central_scr.char2Lvl)
			{
				charLvl.Add (lvl);
			}
		}
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


	// Update is called once per frame
	void Update () {
	
	}

	public void ApplyDamage(int damage)
	{
		damage = damage-def;
		if(damage <= 1)
		{
			damage = 1;
		}
		hp = hp - damage;
		hd.Add(-damage, Color.red, 3);
		if (hp <= 0) {
			Death();
		}
		DisplayHpBar();
	}

	public void Death()
	{
		Debug.Log ("Death");
	}

	public void DoAction()
	{
		Check();
		Invoke ("FinishAction", 2);
	}

	void Check()
	{
		//Here there will be a list of all the skill number
		//and for each skill, check it


		//bool up, down, right, left
		CheckSkill(2);
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


	//Check type 1 skill
	//get the shape and the string
	//Compare, if correct, mark all tiles and deal damage
	void CheckSkill(int skill)
	{
		bool up = true,down=true,right=true,left=true;
		//bool b = true;
		Vector2[] v = skillSet.LoopUpVector(skill, type);
		string[] s = skillSet.LookUpElement(skill, type);
		GameObject[] g = new GameObject[10];
		for(int i = 0; i < v.Length; i++)
		{
			//Debug.Log (v[i]);
			if(v[i].x == 0 && v[i].y == 0)
			{
				break;
			}
			g[i] = gemHolder_scr.gems[XCoord+(int)v[i].x, YCoord+(int)v[i].y];
			if(g[i] != null)
			{
				if(g[i].transform.tag != s[i])
				{
					up = false;
					break;
				}
			}
			else
			{
				up = false;
				break;
			}
		}

		v = skillSet.LoopUpVector(skill, type);
		for(int i = 0; i < v.Length; i++)
		{
			//Debug.Log (v[i]);
			v[i] = Downize(v[i]);
			if(v[i].x == 0 && v[i].y == 0)
			{
				break;
			}
			g[i] = gemHolder_scr.gems[XCoord+(int)v[i].x, YCoord+(int)v[i].y];
			if(g[i] != null)
			{
				if(g[i].transform.tag != s[i])
				{
					down = false;
					break;
				}
			}
			else
			{
				down = false;
				break;
			}
		}
		v = skillSet.LoopUpVector(skill, type);
		for(int i = 0; i < v.Length; i++)
		{
			//Debug.Log (v[i]);
			v[i] = Rightize(v[i]);
			if(v[i].x == 0 && v[i].y == 0)
			{
				break;
			}
			g[i] = gemHolder_scr.gems[XCoord+(int)v[i].x, YCoord+(int)v[i].y];
			if(g[i] != null)
			{
				if(g[i].transform.tag != s[i])
				{
					right = false;
					break;
				}
			}
			else
			{
				right = false;
				break;
			}
		}
		v = skillSet.LoopUpVector(skill, type);
		for(int i = 0; i < v.Length; i++)
		{

			v[i] = Leftize(v[i]);
			Debug.Log (v[i]);
			if(v[i].x == 0 && v[i].y == 0)
			{
				break;
			}
			g[i] = gemHolder_scr.gems[XCoord+(int)v[i].x, YCoord+(int)v[i].y];
			if(g[i] != null)
			{
				if(g[i].transform.tag != s[i])
				{
					left = false;
					break;
				}
			}
			else
			{
				left = false;
				break;
			}
		}
		Debug.Log (up);
		Debug.Log (down);
		Debug.Log (right);
		Debug.Log (left);

		//Mark tiles and deal damage
		Vector2[] vs;
		int damage = 0;
		 v = skillSet.LoopUpVector(skill, type);
		GameObject summon = skillSet.LookUpSummon(skill,type);
		 vs = skillSet.LookUpDamageTile(skill, type);
		 damage = skillSet.LookUpDamage(skill, type , 1, dmg);

		//damage = damage + dmg;
		GameObject a;
		if(up)
		{
			for(int i = 0; i < v.Length; i++)
			{
				if(v[i].x == 0 && v[i].y == 0)
				{
					break;
				}
				a = gemHolder_scr.gems[XCoord+(int)v[i].x, YCoord+(int)v[i].y] ;
				if(a != null)
				{
					a.SendMessage("Mark",1);
				}
			}
			for(int i = 0; i < vs.Length; i++)
			{
				//Debug.Log (v[i]);
				if(vs[i].x == 0 && vs[i].y == 0)
				{
					break;
				}
				if(summon == null)
				{
				HitTile(vs[i],damage);
				}
				else
				{
					GameObject smObj =	Instantiate(summon, new Vector3(transform.position.x + vs[i].x, transform.position.y+ + vs[i].y, transform.position.z),Quaternion.identity) as GameObject;
					smObj.SendMessage("SetDamage",damage);
					Destroy(gemHolder_scr.gems[(int)transform.position.x + (int)vs[i].x, (int)transform.position.y+ + (int)vs[i].y] );
					gemHolder_scr.gems[(int)transform.position.x + (int)vs[i].x, (int)transform.position.y+ + (int)vs[i].y] = smObj;
				}

			}
		}

		v = skillSet.LoopUpVector(skill, type);
		vs = skillSet.LookUpDamageTile(skill, type);
		damage = skillSet.LookUpDamage(skill, type , 1, dmg);
		if(down)
		{
			for(int i = 0; i < v.Length; i++)
			{
				v[i] = Downize(v[i]);
				if(v[i].x == 0 && v[i].y == 0)
				{
					break;
				}
				a = gemHolder_scr.gems[XCoord+(int)v[i].x, YCoord+(int)v[i].y];
				if(a != null)
				{
					a.SendMessage("Mark",1);
				}
			}
			for(int i = 0; i < vs.Length; i++)
			{
				vs[i] = Downize(vs[i]);
				//Debug.Log (v[i]);
				if(vs[i].x == 0 && vs[i].y == 0)
				{
					break;
				}
				if(summon == null)
				{
					HitTile(vs[i],damage);
				}
				else
				{
					GameObject smObj =	Instantiate(summon, new Vector3(transform.position.x + vs[i].x, transform.position.y+ + vs[i].y, transform.position.z),Quaternion.identity)as GameObject;
					smObj.SendMessage("SetDamage",damage);
					Destroy(gemHolder_scr.gems[(int)transform.position.x + (int)vs[i].x, (int)transform.position.y+ + (int)vs[i].y] );
					gemHolder_scr.gems[(int)transform.position.x + (int)vs[i].x, (int)transform.position.y+ + (int)vs[i].y] = smObj;
				}
			}
		}
		v = skillSet.LoopUpVector(skill, type);
		vs = skillSet.LookUpDamageTile(skill, type);
		damage = skillSet.LookUpDamage(skill, type , 1, dmg);
		if(right)
		{
			for(int i = 0; i < v.Length; i++)
			{
				v[i] = Rightize(v[i]);
				if(v[i].x == 0 && v[i].y == 0)
				{
					break;
				}
				a = gemHolder_scr.gems[XCoord+(int)v[i].x, YCoord+(int)v[i].y] ;
				if(a != null)
				{
					a.SendMessage("Mark",1);
				}
			}
			for(int i = 0; i < vs.Length; i++)
			{
				vs[i] = Rightize(vs[i]);
				//Debug.Log (v[i]);
				if(vs[i].x == 0 && vs[i].y == 0)
				{
					break;
				}
				if(summon == null)
				{
					HitTile(vs[i],damage);
				}
				else
				{
					GameObject smObj =	Instantiate(summon, new Vector3(transform.position.x + vs[i].x, transform.position.y+ + vs[i].y, transform.position.z),Quaternion.identity)as GameObject;
					smObj.SendMessage("SetDamage",damage);
					Destroy(gemHolder_scr.gems[(int)transform.position.x + (int)vs[i].x, (int)transform.position.y+ + (int)vs[i].y] );
					gemHolder_scr.gems[(int)transform.position.x + (int)vs[i].x, (int)transform.position.y+ + (int)vs[i].y] = smObj;
				}
			}
		}

		v = skillSet.LoopUpVector(skill, type);
		vs = skillSet.LookUpDamageTile(skill, type);
		damage = skillSet.LookUpDamage(skill, type , 1, dmg);
		if(left)
		{
			for(int i = 0; i < v.Length; i++)
			{
				v[i] = Leftize(v[i]);
				if(v[i].x == 0 && v[i].y == 0)
				{
					break;
				}
				a = gemHolder_scr.gems[XCoord+(int)v[i].x, YCoord+(int)v[i].y] ;
				if(a != null)
				{
					a.SendMessage("Mark",1);
				}
			}
			for(int i = 0; i < vs.Length; i++)
			{
				vs[i] =  Leftize(vs[i]);
				//Debug.Log (v[i]);
				if(vs[i].x == 0 && vs[i].y == 0)
				{
					break;
				}
				if(summon == null)
				{
					HitTile(vs[i],damage);
				}
				else
				{
					GameObject smObj =	Instantiate(summon, new Vector3(transform.position.x + vs[i].x, transform.position.y+ + vs[i].y, transform.position.z),Quaternion.identity)as GameObject;
					smObj.SendMessage("SetDamage",damage);
					Destroy(gemHolder_scr.gems[(int)transform.position.x + (int)vs[i].x, (int)transform.position.y+ + (int)vs[i].y] );
					gemHolder_scr.gems[(int)transform.position.x + (int)vs[i].x, (int)transform.position.y+ + (int)vs[i].y] = smObj;
				}
			}
		}
		//return b;
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
		d.SendMessage("SetDamage",dmg);
	}

	void HitTile(Vector2 v, int dam)
	{
		GameObject d = Instantiate(damager, new Vector3(transform.position.x + v.x, transform.position.y + v.y, transform.position.z), Quaternion.identity) as GameObject;
		d.SendMessage("SetDamage",dam);
	}

	void OnDestroy()
	{
		//gemHolder_scr.characterList.Remove(gameObject);
	}

	void DisplayHpBar()
	{
		Debug.Log (hp/max_hp);
		hpSlider.value = (float)hp/max_hp;
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

}
