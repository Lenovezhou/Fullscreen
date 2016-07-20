using UnityEngine;
using System.Collections;

public class UImanager : MonoBehaviour {
    public AddButtons addbuttons;
    public DataMsg datamsg;
	// Use this for initialization
	void Start () {
	
	}

    public void ChangePanel(string s) 
    {
        addbuttons.a = true;
        datamsg.NowModuleName = s;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
