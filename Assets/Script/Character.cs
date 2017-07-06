using UnityEngine;
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

    //order display
    public GameObject order_prefab;
    private int score = 0;
    private int count = 0;
	public int max_hp = 5;
	public int hp, dmg, def;
	public List<int> charSkill = new List<int>();
	public List<int> charLvl = new List<int>();
	public List<int> charExp = new List<int>();
	private GameObject central;
	private Central central_scr;
	public int atkBonus;
	public int atkBonusTurn = 0;
	public int defBonus;
	public int defBonusTurn = 0;
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
    private OrderList orderList;

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
    private GameObject[] order_obj = new GameObject[3];
	public Vector3 hpBarScale = new Vector3(1,1,1);
    private int order_ff = -1;
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

    //Mock Data for Order
    //private Order o = new Order(new int[4] { 0,0,0,0 });
    //private Order p = new Order(new string[4] { "ItemA", "ItemA", "ItemA", "ItemA" });
    //private Order q = new Order(new string[4] { "ItemA", "ItemA", "ItemA", "ItemA" });

    private Order[] orders = new Order[3];


    // Use this for initialization
    void Start () {
       // orders = new Order[1] { o };
		Sync();
        //Get info from central
        orderList = GameObject.FindGameObjectWithTag("OrderList").GetComponent<OrderList>();
		skillSet = GameObject.Find("SkillSet").GetComponent<SkillSet>();
		hp = max_hp;
		InitUI();
		gemHolder_obj = GameObject.FindGameObjectWithTag ("GemHolder");
		gemHolder_scr = gemHolder_obj.GetComponent<GemHolder> ();
        //	DisplayHpBar();
        CreateOrder();
	}

    //Creating Order at the top so the player can follow it
    //Start by randomly assign ingredient to 4 spot of order
    //Then create an order object at according location and send an array of 4 numbers to it
    //This create a sprite at the location of the orders
    private void CreateOrder()
    {
        
        for (int i = 0; i < 3; i++)
        {
            orders[i] = GenerateOrder();
            GameObject g = Instantiate(order_prefab, new Vector3(5+4*i, 13, 0), Quaternion.identity);
            g.SendMessage("SetOrder", orders[i]);
            order_obj[i] = g;
        }
    }

    private Order GenerateOrder()
    {
        int i, j;
        int[] ing = new int[4];
        for (i = 0; i < 4; i++)
        {
            j = Random.Range(0, gemHolder_scr.max_element);
            ing[i] = j;
        }
        Order o = new Order(ing);
        return o;
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
			foreach(int exp in central_scr.char1Exp)
			{
				charExp.Add (exp);
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
			foreach(int exp in central_scr.char2Exp)
			{
				charExp.Add (exp);
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
		//h.SendMessage("FollowTarget",  transform);
		g.transform.localScale = hpBarScale;
		g.SendMessage("FollowTarget",transform);
		DisplayHpBar();

	}


	// Update is called once per frame
	void Update () {
	
	}

	public void ApplyDamage(int damage)
	{
		damage = damage-def - defBonus;
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
        count = 0;
        score = 0;
        order_ff = -1;
        //Check();
        bool b = false;
        CheckSurrounding(ref b);
        //CheckBonus();
        
        gemHolder_scr.FinishChar(b);
    }

    private bool CheckSurroundingForNull()
    {
        bool n = false;
        if( gemHolder_scr.gems[XCoord, YCoord + 1 ] == null ||
            gemHolder_scr.gems[XCoord, YCoord - 1 ] == null ||
            gemHolder_scr.gems[XCoord + 1, YCoord ] == null ||
            gemHolder_scr.gems[XCoord - 1, YCoord ] == null)
        {
            n = true;
        }
        return n;
    }

    private void CheckSurrounding(ref bool b)
    {
        //if the cook is at the outside perimeter, automatically fail
        if (CheckSurroundingForNull())
            return;
        //Get the tag of each adjacent
        //put it in an array
        int[] cook_in = new int[5];
        string[] tag = new string[5];
        tag[0] = gemHolder_scr.gems[XCoord, YCoord + 1].tag;
        tag[1] = gemHolder_scr.gems[XCoord, YCoord - 1].tag;
        tag[2] = gemHolder_scr.gems[XCoord + 1, YCoord].tag;
        tag[3] = gemHolder_scr.gems[XCoord - 1, YCoord].tag;
        cook_in = ConvertRecipe(tag);



        //Debug.Log(Compare(cook_in, ConvertRecipe(orders[0].GetOrder())));
        
        for (int j = 0; j < orders.Length; j++)
        {
            if(!b)
            if (Compare(cook_in, ConvertRecipe(orders[j].GetOrder())))
            {
               b = CookFood(tag, j);
            }
        }
        Debug.Log(tag[0]);
        
    }


    //Cook the food, the int i is for the position of the food cooked
    private bool CookFood(string[] t, int i)
    {
        Debug.Log("SHouting for COOK");
        //Compare position
         score = GetPositionScore(t, i);
        Debug.Log(score);

        //Get Freshness Score
        score += GetFreshnessScore();
        
        
        //Move Item
        //First, send all 4 Mark Message
        gemHolder_scr.gems[XCoord, YCoord + 1].SendMessage("Mark",20+i);
        gemHolder_scr.gems[XCoord, YCoord - 1].SendMessage("Mark", 20 + i);
        gemHolder_scr.gems[XCoord+1, YCoord ].SendMessage("Mark", 20 + i);
        gemHolder_scr.gems[XCoord-1, YCoord ].SendMessage("Mark", 20 + i);

        //FullFill Order
        order_ff = i;
        //Then call GemHolder DestroyedMarked
        //gemHolder_scr.DestroyMarkedTile(false);

        return true;
    }

    private int GetFreshnessScore()
    {
        int sum = 0;
        sum += GetFresh(gemHolder_scr.gems[XCoord, YCoord + 1]);
        sum += GetFresh(gemHolder_scr.gems[XCoord, YCoord - 1]);
        sum += GetFresh(gemHolder_scr.gems[XCoord + 1, YCoord ]);
        sum += GetFresh(gemHolder_scr.gems[XCoord - 1, YCoord]);
        return sum;
    }

    private int GetFresh(GameObject g)
    {
        int s = 0;
        s = (int)g.GetComponent<Gem>().status;
        return s;
    }

    private void FullFillOrder()
    {
        if (order_ff == -1)
            return;
        if (count == 2)
        {
            
            //Generate new Order
           // orders[order_ff] = GenerateOrder();
            order_obj[order_ff].SendMessage("SetScore", score);
            order_obj[order_ff].SendMessage("SetOrder", orders[order_ff]);
            gemHolder_scr.AddMoney(score);
        }
        else
        {
            count++;
        }
    }

    private int GetPositionScore(string[] t, int i)
    {
        int sum = 0;
        for(int j = 0; j <4; j++)
        {
            if(t[j] == orders[i].GetOrder()[j])
            {
                sum++;
            }
        }
        return 5*sum;
    }

    //Convert a system of tag into broken down ingredient needed
    private int[] ConvertRecipe(string[] tag)
    {
        int[] cook_in = new int[5];
        for (int i = 0; i < 4; i++)
        {
            if (tag[i] == "ItemA")
            {
                cook_in[0]++;
            }
            if (tag[i] == "ItemB")
            {
                cook_in[1]++;
            }
            if (tag[i] == "ItemC")
            {
                cook_in[2]++;
            }
            if (tag[i] == "ItemD")
            {
                cook_in[3]++;
            }
            if (tag[i] == "ItemE")
            {
                cook_in[4]++;
            }

            
        }
        return cook_in;
    }

    private bool Compare(int[] c_in, int[] c_out)
    {
        bool result = true;
        if (c_in[0] < c_out[0])
            return false;
        if (c_in[1] < c_out[1])
            return false;
        if (c_in[2] < c_out[2])
            return false;
        if (c_in[3] < c_out[3])
            return false;
        if (c_in[4] < c_out[4])
            return false;
        return result;
    }

	void CheckBonus()
	{
		atkBonusTurn --;
		if(atkBonusTurn <=0)
		{
			atkBonus = 0;
		}
		defBonusTurn --;
		if(defBonusTurn <=0)
		{
			defBonus = 0;
		}
	}

	void Check()
	{
		//Here there will be a list of all the skill number
		//and for each skill, check it


		//bool up, down, right, left
		for(int i =0; i < charSkill.Count; i++)
		{
			Debug.Log("CharSkill Count" + charSkill.Count + "Skill number" + i);
			int temp = CheckSkill(charSkill[i]);
			if(temp >0)
			{
				charExp[i]= charExp[i] + temp;
				if(CheckPowerUp(charExp[i]) != charLvl[i] )
				{
					Debug.Log("Level Up");
					LevelUp(i);
				}
			}
	  	}
		//CheckForNewSkill(type);
	}

	void LevelUp(int i)
	{
		charLvl[i] = CheckPowerUp(charExp[i]);

	}

	void CheckForNewSkill(int type)
	{
		if(type == 1)
		{
			if(charLvl[charSkill.IndexOf(1)] >=2 )
			{
				charSkill.Add (5);
				charLvl.Add (1);
				charExp.Add (0);
			}
		}
	}

	int CheckPowerUp(int exp)
	{
		if(exp < 1)
		{
			return 1;
		}
		else if (exp < 25)
		{
			return 2;
		}
		else if (exp < 50)
		{
			return 3;
		}
		else if (exp < 100)
		{
			return 4;
		}
		else
		{
			return 5;
		}
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
	int CheckSkill(int skill)
	{
		int use = 0;
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
		 damage = skillSet.LookUpDamage(skill, type , 1, dmg) + atkBonus;

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
					use ++;
			if(damage > 900)
			{
				atkBonus = damage - 900;
				atkBonusTurn = 3;
			}
			if(damage > 1000)
			{
				defBonus = damage - 1000;
			}

		}

		v = skillSet.LoopUpVector(skill, type);
		vs = skillSet.LookUpDamageTile(skill, type);
		damage = skillSet.LookUpDamage(skill, type , 1, dmg)+ atkBonus;
		if(down && damage < 900)
		{
					use ++;
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
		damage = skillSet.LookUpDamage(skill, type , 1, dmg)+ atkBonus;
		if(right &&  damage < 900)
		{
			use++;
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
		damage = skillSet.LookUpDamage(skill, type , 1, dmg)+ atkBonus;
		if(left&& damage < 900)
		{
			use++;
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
		return use;
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
