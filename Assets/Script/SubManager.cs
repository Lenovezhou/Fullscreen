using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}


	// Update is called once per frame
	void Update () {
	
	}
}
public class Sub {
    private string name;
    public string Name
    {

        set { name = value; }

        get { return name; }
    }

    private string englishname;
    public string ENglishname
    {
        set { englishname = value; }
        get { return englishname; }
    }
    public List<Product> ProductList;
}
