using UnityEngine;
using System.Collections;

public class SkillSet : MonoBehaviour {



	public Vector2[] LoopUpVector(int skill, int type)
	{
		Vector2[] set = new Vector2[10];
		//Normal Mage
		if(type == 0)
		{
			if(skill == 0)
			{

				set[0] = new Vector2(1,1);
				set[1] = new Vector2(-1,1);
				return set;
			}
		}
		return set;
	}

	public string[] LookUpElement(int skill, int type)
	{
		string[] s = new string[10];

		if(type == 0)
		{
			if(skill == 0)
			{
				s[0] = "Fire";
				s[1] = "Tree";
			}
			
		}
		return s;
	}

	public int LookUpDamage(int skill, int type, int level)
	{
		int damage = 0;
		if(type == 0)
		{
			if(skill == 0)
			{
				damage = 4 + 2 * level;
			}
		}

		return damage;
	}

	public Vector2[] LookUpDamageTile(int skill, int type)
	{
		Vector2[] set = new Vector2[20];
		if(type == 0)
		{
			if(skill == 0)
			{
				set[0]  = new Vector2(0,2);
			}
		}
		return set;
	}
}
