using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Collections;

public class testxml : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame

    public static List<modolMSG> ReadInfo(string path, string Name)
    {
        List<modolMSG> Info = new List<modolMSG>();
        System.IO.StringReader stringReader = new System.IO.StringReader(path);
        stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        Debug.Log(Xmlroot[Name].ChildNodes.Count);
        ///只要调用这个方法，不管读取哪个节点都将电话号码赋值给  uimanager._instance.cellphonenumber
        ///
      //  UImanager._instance.cellphonenumber = Xmlroot["G"].Attributes["cellphonenumber"].Value;

        foreach (XmlNode item in Xmlroot[Name].ChildNodes)
        {
            modolMSG modolmsg = new modolMSG();
            modolmsg.Englishname = item.Attributes["englishname"].Value;
            modolmsg.Name = item.Attributes["name"].Value;
			modolmsg._name.Add (modolmsg.Name);
            modolmsg.SubList = new List<Sub>();
            foreach (XmlNode item1 in item.ChildNodes)
            {
                Sub sub = new Sub();
                sub.Name = item1.Attributes["name"].Value;
                sub.ENglishname = item1.Attributes["englishname"].Value;
                modolmsg.SubList.Add(sub);
                sub.ProductList = new List<Product>();
                foreach (XmlNode item2 in item1.ChildNodes)
                {
                    Product prod = new Product();
                    //prod.Name = item2.Attributes["name"].Value;
                    //prod.ModleURL = item2.Attributes["modle"].Value;
                    //prod.TextureURL = item2.Attributes["texture"].Value;
                    //prod.Description = item2.Attributes["description"].Value;
                    sub.ProductList.Add(prod);
                }
            }
            Info.Add(modolmsg);
        }
        return Info;
    }
    
}
