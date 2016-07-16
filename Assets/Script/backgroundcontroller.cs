using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class backgroundcontroller : MonoBehaviour,IPointerClickHandler
{
	public GyroController gyrocontroller;
	public int int_name;// Use this for initialization
	void Start () {
	
	}
	public void OnPointerClick (PointerEventData eventData){
		gyrocontroller.Click_change_back (int_name);
		gyrocontroller.ChangeBackGround ();
	}
	// Update is called once per frame
	void Update () {
	
	}
}
