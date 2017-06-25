using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gem : MonoBehaviour {
	//Set up for changing sprite color
	public Sprite[] color_sprite = new Sprite[5];
	private SpriteRenderer spriteRenderer;
	UISlider hpSlider;
	public int color;
	bool release = true;
	bool ready = false;
	bool detonate = false;
	bool battleMarked = false;
    bool cookedMarked = false;
	public GameObject timer;
	public GameObject damager;
	public bool heart =false;
	public bool isTotem;
	public int totemLives=3;
    private Vector3 dest = new Vector3(0, 0, 0);

	//if gems is a character
	public bool isChar;
	public bool isSpecial;
	public bool unready;
	bool match = false;
	public int damage = 0;
    private SpriteRenderer BG;


	//Set up for click and drag
	private Vector3 offset;
	private Vector3 screenPoint;

	//The original coordinate
	public int curX;
	public int curY;

	//mark whether it's up for destroy
	private bool marked = false;				
	//value when matched
	public int value;
    public int spoil;

	//Gem Holder cache
	private GameObject gemHolder_obj;
	private GemHolder gemHolder_scr;

    //The Status of the gems(food)
    public enum Status { Fresh = 5, Good = 3, Okay = 2, Bad = 1, Rat = 0 };
    public Status status;

    //Currently Coordinate, get by its local position
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

	public void NewTurn()
	{

		if(unready)
		{
			CreateGem();
			unready = false;
		}
		if(isTotem)
		{
			DealDamage(XCoord,YCoord+1);
			DealDamage(XCoord,YCoord-1);
			DealDamage(XCoord+1,YCoord);
			DealDamage(XCoord-1,YCoord);
			totemLives--;
			if(totemLives == 0)
			{
				Destroy(gameObject);
			}
		}
        else
        {
            ChangeStatus();
        }
	}

    private void ChangeStatus()
    {
        if (isChar)
            return;
        if (status == Status.Fresh)
        {
            status = Status.Good;
        }
        else if (status == Status.Good)
        {
            status = Status.Okay;
        }
        else if (status == Status.Okay)
        {
            status = Status.Bad;
        }
        else if (status == Status.Bad)
        {
            status = Status.Rat;
            gemHolder_scr.rat_flag = true;
        }
        Debug.Log(status);
        ChangeStatusSprite();
    }

    private void ChangeStatusSprite()
    {
        if (status == Status.Fresh)
        {
            BG.color = Color.white;
        }
        if (status == Status.Good)
        {
            BG.color = Color.green;
        }
        if (status == Status.Okay)
        {
            BG.color = Color.yellow;
        }
        if (status == Status.Bad)
        {
            BG.color = Color.magenta;
        }
        if (status == Status.Rat)
        {
            BG.color = Color.red;
        }
    } 

	void Start () 
	{
        if (!isChar)
        {
            BG = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        }
		timer = GameObject.FindGameObjectWithTag("Timer");
		hpSlider = timer.GetComponent<UISlider>();
			transform.localScale = new Vector3(0.9f,0.9f,0.9f);

		gemHolder_obj = GameObject.FindGameObjectWithTag ("GemHolder");
		gemHolder_scr = gemHolder_obj.GetComponent<GemHolder> ();
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		if(!isSpecial && !unready )
		{
			CreateGem();
		}
		else
		{
			transform.parent = GameObject.FindGameObjectWithTag ("GemHolder").transform;
			curX = XCoord;
			curY = YCoord;
		}

		if(unready)
		{
			transform.tag = "Unready";
		}

	}



	// Update is called once per frame
	void Update () {
		if(match)
		{
            if (cookedMarked)
            {
                transform.position = Vector3.MoveTowards(transform.position, dest, 10 * Time.deltaTime);
                if (Vector3.Distance(transform.position, dest) < 0.1f)
                {
                    match = false;
                    gemHolder_scr.GemSignal();
                    GameObject.FindGameObjectWithTag("Char").SendMessage("FullFillOrder");
                    Destroy(gameObject);
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, dest, 10 * Time.deltaTime);
                if (Vector3.Distance(transform.position, dest) < 0.1f)
                {
                    match = false;
                    gemHolder_scr.GemSignal();
                    Destroy(gameObject);
                }
            }
		}
	}

	public void CreateGem()
	{
		//int color = Random.Range (0, gemHolder_scr.max_element);
		if (isChar) {
			//transform.tag = "Char";
		} else {
			Init_color ();
		}
		transform.parent = GameObject.FindGameObjectWithTag ("GemHolder").transform;
		curX = XCoord;
		curY = YCoord;
		if(transform.tag == "Heart")
			heart =true;
        status = Status.Fresh;
	}

	/// <summary>
	/// Initialize color
	/// </summary>
	void Init_color()
	{
		//transform.tag = "Unready";
		int color_num = gemHolder_scr.max_element;
		color = Random.Range(0, color_num);
		if (color == 0)
		{
			transform.tag = "ItemA";
			//spriteRenderer.sprite  = color_sprite[color];
		}
		if (color == 1)
		{
			transform.tag = "ItemB";
			//spriteRenderer.sprite  = color_sprite[color];
		}
		if (color == 2)
		{
			transform.tag = "ItemC";
			//spriteRenderer.sprite  = color_sprite[color];
		}
		if (color == 3)
		{
			transform.tag = "ItemD";
			//spriteRenderer.sprite  = color_sprite[color];
		}
		if(color== 4)
		{
			transform.tag = "ItemE";
			//spriteRenderer.sprite  = color_sprite[color];
		}
		spriteRenderer.sprite = color_sprite[color];
		//transform.tag = "Unready";
	}

	void OnMouseDown()
	{
		if (isChar || unready)
			return;

	//	Debug.Log ("CheckStatus");
	//	Debug.Log ((int)gemHolder_scr.status);
		if ((int)gemHolder_scr.status == 0) {
			hpSlider.value = 100;
			timer.GetComponent<UIFollowTarget>().enabled = true;
			timer.SendMessage("FollowTarget", transform);
			Debug.Log ("Called Mouse Down");
			offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
			Invoke ("ForceMouseUp", 5);
			release = false;
		}
	}
	
	void OnMouseDrag()
	{
		//Debug.Log ((int)gemHolder_scr.status);
		if (isChar || unready)
			return;
		if (release == false &&(int)gemHolder_scr.status == 0 ) {
			hpSlider.value = hpSlider.value-Time.deltaTime/5; 
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
			if (curPosition.x >= gemHolder_scr.GridWidth+5 - 0.5f || curPosition.x <= 4.5f) {
				curPosition.x = curX;
			}

			if (curPosition.y >= gemHolder_scr.GridHeight+5 - 0.5f || curPosition.y <= 4.5f) {
				curPosition.y = curY;
			}


//			Debug.Log (XCoord);
			transform.position = curPosition;
			//Debug.Log (XCoord);
			//Debug.Log (curX);
			if ((XCoord != curX || YCoord != curY))
			{
				if( gemHolder_scr.gems [XCoord, YCoord] != null)
				{
					gemHolder_scr.SwapGem (gameObject, gemHolder_scr.gems [XCoord, YCoord]);
				}
				else
				{
					gemHolder_scr.SwapGem (gameObject, new Vector2(XCoord,YCoord));
				}
			}
		}
	}



	//Register the current coordinate
	public void Register()
	{
		curX = XCoord;
		curY = YCoord;
	}

	void ForceMouseUp()
	{
	
		if (release == false) {

			Release ();
		}
	}

	void OnMouseUp()
	{
		if (isChar && unready)
			return;
		if (release == false &&(int)gemHolder_scr.status == 0 ) {
			Release ();
		}
	}

	public void Release()
	{

		timer.GetComponent<UIFollowTarget>().enabled = false;
		timer.transform.position = new Vector3(-875,877,0);
		if ((int)gemHolder_scr.status == 0)
		{
			//Debug.Log ("RELEASE");
		release = true;
		transform.localPosition = new Vector2 (curX, curY);
		gemHolder_scr.NextAction ();
	}

	}

	/// <summary>
	/// Mark the specified value.
	/// if mark is 1 that's meant battle marked.
    /// if mark is 20, that's meant cook marked
	/// </summary>
	/// <param name="value">the length of the match, e.g. match 3 or 4</param>
	void Mark(int val)
	{

		if(val == 1)
		{
			battleMarked = true;
		}
        if(val > 19)
        {
            cookedMarked = true;
            dest = new Vector3(5 + 4 * (val - 20), 13, 0);
        }
		marked = true;
		value = val;
	}

	public bool isMarked()
	{
		return marked;
	}

    public void DestroyBad()
    {
        if(status == Status.Rat)
        {
            gemHolder_scr.no_bad = false;
            MatchAnimation(2);
        }
    }

	public void DestroyMarked()
	{
		//Error checking make sure that the character does not get killed in matching
		if (isChar&&unready)
			return;

		if (marked) {
			/*if(t)
			{
				if(transform.CompareTag("Heart"))
				{
					gemHolder_scr.AddHeart(1);
				}
				else if(transform.CompareTag("Dark") || transform.CompareTag("Mine"))
				{
					//Do Nothing
				}
				else
				{
					gemHolder_scr.AddCombo(1);
				}
			}*/

			//Destroy(gameObject);
			if(battleMarked)
			{
			
				MatchAnimation(2);
			}
			else
			{
                Debug.Log("MatchHere");
				MatchAnimation(0);
			}

		}
	}

	/// <summary>
	/// Make animations as the gems get destroyed. Based on type, do different animation
	/// 0: just go to wherever;
	/// 1: break animation
	/// 2: Attack animation
	/// </summary>
	/// <param name="type">Type.</param>
	void MatchAnimation(int dam)
	{
        gemHolder_scr.gems[XCoord, YCoord] = null;
		detonate = true;
		/*if(transform.CompareTag("Dark") || transform.CompareTag ("Mine"))
		{
			DealDamage(XCoord,YCoord+1);
			DealDamage(XCoord,YCoord-1);
			DealDamage(XCoord+1,YCoord);
			DealDamage(XCoord-1,YCoord);

		}*/
		transform.localScale = new Vector3(0.5f,0.5f,0.5f);
		if(dam== 0)
		{
			match = true;
		}
		else if(dam == 1)
		{
			//damage animation
			Invoke("DoDestroy",1);
		}
		else if(dam == 2)
		{
		    
			//Just burn
			Invoke("DoDestroy",1);

            gemHolder_scr.AddMoney(-gemHolder_scr.cost);
		}
		//yield return new WaitForSeconds (1f);
		//gemHolder_scr.CreateNewGem(XCoord, YCoord);
		//Destroy (gameObject);
	}

	void DoDestroy()
	{
		gemHolder_scr.GemSignal();
		Destroy(gameObject);
	}

/*	IEnumerator DamageAnimation()
	{
		detonate = true;
		if(transform.CompareTag("Dark"))
		{
			DealDamage(XCoord,YCoord+1);
			DealDamage(XCoord,YCoord-1);
			DealDamage(XCoord+1,YCoord);
			DealDamage(XCoord-1,YCoord);
			
		}
		transform.localScale = new Vector3(0.5f,0.5f,0.5f);
		yield return new WaitForSeconds (1f);
		//gemHolder_scr.CreateNewGem(XCoord, YCoord);
		Destroy (gameObject);
	}
*/

	void DealDamage(int x,int y)
	{
		GameObject d = Instantiate(damager, new Vector3( x, y, transform.position.z), Quaternion.identity) as GameObject;
		if(transform.CompareTag("Dark"))
		{
		d.SendMessage("SetDamage",1);
		}
		if(transform.CompareTag("Mine"))
		{
			//Debug.Log ("Hmm");
			d.SendMessage("SetDamage", damage);
		}
		if(transform.CompareTag("Totem"))
		{
			d.SendMessage("SetDamage", damage);
		}
	}

	public void SetDamage(int d)
	{
		damage = d;
	}

	public void ApplyDamage(int damage)
	{

		//Debug.Log ("ApplyDamage");
		if (isChar ) {
			if(transform.CompareTag("Char"))
			gameObject.GetComponent<Character> ().ApplyDamage (damage);
		} else {
			if (heart) {
				gemHolder_scr.ApplyDamage(1);
				heart = false;
				//Lose level hp
				//StartCoroutine("DamageAnimation");
				MatchAnimation(1);
			}
			else
			{
				if(detonate == true)
				{

				}
				else
				{
				//StartCoroutine("DamageAnimation");
					MatchAnimation(1);
				}
			}
		}
	}
}
