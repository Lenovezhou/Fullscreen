using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AddButtons : MonoBehaviour {
    [HideInInspector]
    public bool a;
    public GameObject ProductButton, ProductButton1;
    public Transform[] parent;
    private List<GameObject> GameObjectPool;
    private DataMsg DataMsg;
    private List<string> PathURL;
	// Use this for initialization
	void Start () {
        GameObjectPool = new List<GameObject>();
        PathURL=new List<string>();
        DataMsg=GetComponent<DataMsg>();
	  string path1 =
#if UNITY_EDITOR
 "file://" + Application.dataPath + "/StreamingAssets/";
#elif UNITY_ANDROID
    "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IOS
       "file:///"+ Application.streamingAssetsPath + "/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
 "file://" + Application.dataPath + "/StreamingAssets/";
#else
        string.Empty;
#endif
 PathURL.Add(path1);
	}


    
	
	
	// Update is called once per frame
	void Update () {
        if (a)
        {
            StartCoroutine(LoadXml(PathURL[0] + "Data.xml", DataMsg.NowModuleName));
            a = false;
        }
	}


    IEnumerator LoadXml(string path, string modNume)
    {
        GameObjectPool.Clear();
        WWW m_SaveWWW = new WWW(path);
        yield return m_SaveWWW;
        DataMsg.moduleList = testxml.ReadInfo(m_SaveWWW.text, modNume);
        LoadModuleButton(DataMsg.moduleList);
    }

    void LoadModuleButton(List<modolMSG> data)
    {

        for (int i = 0; i < data.Count; i++)//  SaveInfo temp in data)
        {
            GameObject button = Instantiate(ProductButton);
            //button.transform.SetParent(DataMsg.parent[0]);
            button.transform.parent =parent[0];
            button.transform.localScale = Vector3.one;
            Modol module = button.AddComponent<Modol>();    // 为每个按钮添加脚本
			//modolMSG msg = new modolMSG ();
			//module.initModule (msg._name[i]);
			// GameObjectPool.Add(button);         // 保存后用于切换panel时删除实例物体
            for (int j = 0; j < data[i].SubList.Count; j++)
            {
                GameObject subbutton = Instantiate(ProductButton1);//ProductButton：  Button 的预制体
                subbutton.SetActive(false);
                subbutton.transform.SetParent(parent[0]);
                subbutton.transform.localScale = Vector3.one;
                SubManager sub = subbutton.AddComponent<SubManager>();
                //sub.initModule(data[i].SubList[j]);
                Debug.Log(module.SubObj + "module");
                module.SubObj.Add(subbutton);
               
                GameObjectPool.Add(subbutton);      // 保存后用于切换panel时删除实例物体
            }
        }

        //   ChackActiveCount(image1);       //  是否显示向下箭头



    }
}
