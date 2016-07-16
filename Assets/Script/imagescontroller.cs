using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class imagescontroller : MonoBehaviour,IPointerClickHandler
{
	
	public static GyroController gyrocontroller;

	public int _name_int;  
	// Use this for initialization
	void Start () {
	
	}

	//人物图片是成对出现的
	public void OnPointerClick (PointerEventData eventData){

			gyrocontroller.Click_change_close (_name_int*2);
			gyrocontroller.ChangeClose ();



	}
	// Update is called once per frame
	void Update () {
	
	}
}
