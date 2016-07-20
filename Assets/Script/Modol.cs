using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Modol : MonoBehaviour {
	
	private modolMSG modolmsg;
	private Text buttonname;
    private List<Button> bu;
    private Button selfbutton;
    private Image buttonimage;
    private bool IsOn;
	// Use this for initialization
	void Start () {
        bu = new List<Button>();
        bu.Add(this.GetComponent<Button>());
        selfbutton = GetComponent<Button>();
        selfbutton.onClick.AddListener(ChangeStates);
        buttonimage = GetComponent<Image>();
        bu = new List<Button>();
		buttonname = GetComponentInChildren<Text> ();
		modolmsg = new modolMSG ();
	}
    private List<GameObject> subObj = new List<GameObject>();

    public List<GameObject> SubObj
    {
        get { return subObj; }
        set { subObj = value; }
    }

    public void ChangeStates() {
        
            if (buttonimage.color == Color.white)
            {
                buttonimage.color = Color.gray;
                bu.Remove(this.gameObject.GetComponent<Button>());
                //      将之前的button的image的color都改为白色
                for (int i = 0; i < bu.Count; i++)
                {
                    bu[i].image.color = Color.white;
                }
                //  将改变颜色的button都记录下来
                bu.Add(this.gameObject.GetComponent<Button>());
            }
            else
            {
                buttonimage.color = Color.white;
            }

           // DataMsg.parentname = this.transform.GetChild(0).GetComponent<Text>().text;     // 记录当前被按下按钮的文字
            if (IsOn)
            {
                foreach (GameObject item in subObj)
                {
                    item.SetActive(false);
                }
                IsOn = false;

            }
            else
            {
                foreach (GameObject item in subObj)
                {
                    item.SetActive(true);
                }
                IsOn = true;

            }
            //  AddButton._instand.ChackActiveCount(AddButton._instand.image1);      //  展开子菜单时检查子button的数量决定
        
    }

	public void initModule(string names)
	{
		
		buttonname.text = names;
	}


	// Update is called once per frame
	void Update () {
	
	}
}

public class modolMSG {
    public static modolMSG modolmsg;
    private string name;
    public string Name 
    { 
		
        set{name=value;}

        get{return name;}

    }
	public List<string> _name = new List<string> ();
    private string englishname;
    public string Englishname 
    {
        set { englishname = value; }
        get { return englishname; }
    }

    public List<Sub> SubList;
  

}