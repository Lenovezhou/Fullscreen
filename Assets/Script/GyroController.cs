#if UNITY_ANDROID && !UNITY_EDITOR
#define ANDROID
#endif

#if UNITY_IPHONE && !UNITY_EDITOR
#define IPHONE
#endif


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;


enum uistates { 

   
    left,
    right

} 

public class GyroController : MonoBehaviour 
{
    
	public GameObject imageprefab;
	public GameObject backgroungpanel,colosepanel,showpanel;
	public GameObject B_imageparent,C_imageparent;
	public Animator player_test;
	public GameObject backgroundmaterial,playermaterial;
	public float camerarotaionspeed,scalespeed;
	private float adda, addb,camera_lastrotate_x,camera_lastrotate_y ,cameraview,sun_cameraview;
	private Vector3 camera_lastpos;
	private Camera[] cameras;
	private bool gyroEnabled = true;
	private const float lowPassFilterFactor = 0.2f;
	private bool chose=false;	//是否手动
	private bool half_full=false;		//是否全屏
	private readonly Quaternion baseIdentity =  Quaternion.Euler(90, 0, 0);
	private Touch oldtouch1,oldtouch2;
	private Object[] backgrounds,modols,C_images,B_images;
	private string pointer_name;
	private int i=0,j=0;
	private List<GameObject> delete;

	
	private Quaternion cameraBase =  Quaternion.identity;
	private Quaternion calibration =  Quaternion.identity;
	private Quaternion baseOrientation =  Quaternion.Euler(90, 0, 0);
	private Quaternion baseOrientationRotationFix =  Quaternion.identity;
	private Quaternion referanceRotation = Quaternion.identity;
	private bool gyroBool ;
    private uistates leftstate;
    private Animator UImoves; 
	private float timer;

    void OnGUI()
    {
		if (!gyroBool) {
			GUI.Label (new Rect (50, 100, 500, 20), "当前设备不支持三轴陀螺仪！！！");
		}
       // GUI.Label(new Rect(50, 100, 500, 20), "Label : " + Input.gyro.attitude.x + "       " + Input.gyro.attitude.y + "         " + Input.gyro.attitude.z + "       " + Time.deltaTime);
        //GUI.Label(new Rect(50, 80, 500, 20), "Label : " + Input.gyro.gravity + "       " + Input.gyro.rotationRate + "         " + Input.gyro.rotationRateUnbiased + "       " + Input.gyro.userAcceleration);

    }  


	protected void Start () 
	{
		//当前设备是否支持三轴陀螺仪
		gyroBool = SystemInfo.supportsGyroscope;
		delete = new List<GameObject> ();
		backgroungpanel.SetActive (false);
		colosepanel.SetActive (false);
        leftstate = uistates.right;
		AttachGyro();
		cameras = transform.GetComponentsInChildren<Camera> ();
		backgrounds = Resources.LoadAll ("BackGround");
		modols = Resources.LoadAll ("Modl");
		C_images= Resources.LoadAll ("C_Picture");
		B_images= Resources.LoadAll ("B_Picture");
        UImoves = showpanel.GetComponentInChildren<Animator>();
	}
	//切换人物
	public void ChangeClose(){
		
		bool action = colosepanel.activeSelf;
		action = action == true ? false : true;
		colosepanel.SetActive (action);
		for (int i = 0; i < C_images.Length/2; i++) {
			GameObject image = Instantiate (imageprefab);
		//	Debug.Log (C_images.Length);
			delete.Add (image);
			image.GetComponent<Image> ().overrideSprite = Resources.Load ("C_Picture/"+i,typeof(Sprite))as Sprite;
			image.transform.SetParent (C_imageparent.transform);
			imagescontroller _imagescontroller=image.AddComponent<imagescontroller> ();
			_imagescontroller._name_int = i;
			imagescontroller.gyrocontroller = this;

			//pointer_name = imagescontroller._name;
		}
		if (!action) {
			for (int i = 0; i < delete.Count; i++) {
				Destroy (delete[i]);
			}
		}




//		j+=2;
//
//		if (j <= modols.Length - 1) {
//			player_test.SetBool("fiter",false);
//			playermaterial.GetComponent<MeshRenderer> ().material = Instantiate (modols [j])as Material;
//			playermaterial.transform.GetChild (0).GetComponent<MeshRenderer> ().material = Instantiate (modols [j+1]as Material);
//		} else {
//			j = 0;
//			player_test.SetBool ("fiter",true);
//			playermaterial.GetComponent<MeshRenderer> ().material = Instantiate (modols [j])as Material;
//			playermaterial.transform.GetChild (0).GetComponent<MeshRenderer> ().material = Instantiate (modols [j+1]as Material);
//		}

	}


	//点击切换人物调用的方法
	public void Click_change_close(int i){
        Material mm =Instantiate(modols[i]) as Material;
		playermaterial.GetComponent<MeshRenderer> ().material=mm ;
		playermaterial.transform.GetChild (0).GetComponent<MeshRenderer> ().material = Instantiate (modols [i+1]as Material);

		Debug.Log (mm.mainTexture.width+"width,hight"+mm.mainTexture.height);
		//截取实例出的material的名字：
//        string str1 = modols[i].name;
//        string str2 = modols[i + 1].name;
//        string[] wide = str1.Split(',');
//        string[] high = str2.Split(',');
//		float[] tempint_w = new float[wide.Length];
//		float[] tempint_h=new float[high.Length];
//        for (int q = 0; q < wide.Length; q++)
//        {
//            tempint_w[q] = System.Convert.ToInt32(wide[q]);
//            Debug.Log(System.Convert.ToInt32(wide[q]));
//        }
//        for (int u = 0; u < high.Length; u++)
//        {
//			Debug.Log(System.Convert.ToInt32(high[u]));
//            tempint_h[u] = System.Convert.ToInt32(high[u]);
//        }
		//float temp = tempint_w [1] / tempint_h [1];
		float temp=(float)mm.mainTexture.width/(float)mm.mainTexture.height;
		Vector3 playerplan_scale = new Vector3(-temp,1,-1);
        playermaterial.transform.localScale = playerplan_scale;

        //string strtemp1= wide.ToString();
        //string strtemp2 = high.ToString();
        //int a = System.Convert.ToInt32(strtemp1);
        //int b = System.Convert.ToInt32(strtemp2);
      
	}
	//点击切换背景调用的方法
	public void Click_change_back(int i){
		backgroundmaterial.GetComponent<MeshRenderer> ().material =Instantiate (backgrounds [i])as Material;
	}  

	//切换背景
	public void ChangeBackGround()
    {
		bool action = backgroungpanel.activeSelf;
		action = action == true ? false : true;
		backgroungpanel.SetActive (action);

		for (int i = 0; i < B_images.Length/2; i++)
        {
			GameObject image = Instantiate (imageprefab);
			delete.Add (image);
			image.GetComponent<Image> ().overrideSprite = Resources.Load ("B_Picture/"+i,typeof(Sprite))as Sprite;
			image.transform.SetParent (B_imageparent.transform);
			backgroundcontroller backcontorller=image.AddComponent<backgroundcontroller> ();
			backcontorller.int_name = i;
			backcontorller.gyrocontroller = this;

			//pointer_name = imagescontroller._name;
		}

		if (!action)
        {
			for (int i = 0; i < delete.Count; i++)
            {
				Destroy (delete[i]);
			}
		}
	}


	//全半屏
	public void Half_Full()
    {
		half_full=half_full==false?true:false;

	}

	//手自动
	public void Hand_Auto()
    {
		chose=chose==true?false:true;
		adda = transform.rotation.eulerAngles.y;
		addb = transform.rotation.eulerAngles.x;

	//	adda=transform.rotation.eulerAngles.y;
	//	addb = transform.rotation.eulerAngles.x;
	}

	//切换panel的具体方法：
	void ChangePanel(GameObject last,GameObject current)
    {
		if (current.activeSelf&&last)
        {
			
		}
	}


    public void ChangeAnimator(string name) 
    {
        UImoves.SetBool(name,true);
    }



	protected void Update()
	{
		//检测开启的panel只允许开启一个；
		if (backgroungpanel.activeSelf) {
			colosepanel.SetActive (false);
		} else if (colosepanel.activeSelf) {
			backgroungpanel.SetActive (false);
		}

      //检测菜单时间
		timer+=Time.deltaTime;
       




//#if ANDROID||IPHONE

        
//#endif

        //		#if UNITY_EDITOR
		if (Input.GetMouseButton(0)&&chose) 
		{
//			if (EventSystem.current.IsPointerOverGameObject()) 
//			{
				//Debug.Log ("点击在UI上"+EventSystem.current.gameObject.name);
			adda+=Input.GetAxis("Mouse X")*camerarotaionspeed;
			addb+=Input.GetAxis("Mouse Y")*camerarotaionspeed;
			Vector3 newrotation = new Vector3 (addb, adda, 0) ;
			transform.rotation=Quaternion.Euler(newrotation);

		//	}
		}




//		transform.rotation = Quaternion.Slerp (transform.rotation,
//			cameraBase * (ConvertRotation (referanceRotation * Input.gyro.attitude) * GetRotFix ()), lowPassFilterFactor);



		if (half_full) {
			transform.GetChild (0).gameObject.SetActive (false);
			transform.GetChild (1).gameObject.SetActive (false);
			transform.GetChild (2).gameObject.SetActive (true);
		} else if (!half_full) {
			transform.GetChild (0).gameObject.SetActive (true);
			transform.GetChild (1).gameObject.SetActive (true);
			transform.GetChild (2).gameObject.SetActive (false);
		}
//#if IPHONE||ANDROID
//		if (EventSystem.current.IsPointerOverGameObject (Input.GetTouch (0).fingerId)) 
//		{
//			Debug.Log ("点击在UI上");
//		}
//
//#else
		if (EventSystem.current.IsPointerOverGameObject ()) {
		//	Debug.Log ("点击在UI上");
		}
//#endif
	//		Debug.Log ("点击在UI上");
	else {
			//菜单（ShowPanel）开启方式：不受状态（chose，ispointerovergameobject()）控制
			if (leftstate == uistates.right&&timer>=5f&&timer<=45f)

			{
				//Debug.Log(leftstate.ToString());
				Debug.Log("right : Down");
				UImoves.SetBool("right",true);
				UImoves.SetBool("left", false);
				leftstate = uistates.left;
			}

			else if ((Input.touchCount>0&&Input.GetTouch(0).phase == TouchPhase.Began||Input.GetMouseButton(0)) && leftstate == uistates.left)
			{
				timer = 0f;
				Debug.Log("left : Down");

				UImoves.SetBool("right", false);
				UImoves.SetBool("left", true);
				leftstate = uistates.right;
			}



			//手动
			if (chose) {
				if (Input.touchCount == 0) {
					//chose = false;
					return;
				}
				if (Input.touchCount <= 1) {
					//if (!EventSystem.current.IsPointerOverGameObject (Input.GetTouch(0).fingerId)) {
					//chose = true;
					adda += Input.GetTouch (0).deltaPosition.x;
					addb += Input.GetTouch (0).deltaPosition.y;
					Vector3 newrotation = new Vector3 (-addb, adda, 0) * camerarotaionspeed;
					//Quaternion newquater = Quaternion.LookRotation (newrotation);
					transform.rotation = Quaternion.Euler (newrotation);
					//transform.rotation=Quaternion.Slerp(transform.rotation,newquater,0.2f);
					//transform.Rotate(Vector3.up, Input.GetTouch(0).deltaPosition.x * camerarotaionspeed);
					//transform.Rotate(Vector3.right,Input.GetTouch(0).deltaPosition.y*camerarotaionspeed);
				}
			
				if (Input.touchCount >= 2) {
				
			
					Touch newtouch1 = Input.GetTouch (0);
					Touch newtouch2 = Input.GetTouch (1);
					if (Input.GetTouch (1).phase == TouchPhase.Began) {
						oldtouch1 = newtouch1;
						oldtouch2 = newtouch2;
						return;
					}
					float oldDistance = Vector2.Distance (oldtouch1.position, oldtouch2.position);
					float newDistance = Vector2.Distance (newtouch1.position, newtouch2.position);
					float offset = oldDistance - newDistance;
					float scaleFort = offset / 100f;
					for (int i = 0; i < cameras.Length; i++) {
						if (cameraview >= 30 && cameraview <= 70) {
							cameraview += scaleFort;

							cameras [i].fieldOfView = cameraview;
						} else {
							cameraview = Mathf.Clamp (cameraview, 30f, 70f);

						}
					}
					oldtouch1 = newtouch1;
					oldtouch2 = newtouch2;
				}

			}
		}
			
		if (!chose)
		{
			transform.rotation = Quaternion.Slerp (transform.rotation,
				cameraBase * (ConvertRotation (referanceRotation * Input.gyro.attitude) * GetRotFix ()), lowPassFilterFactor);
		}
				
}





	private void AttachGyro()
	{
		Input.gyro.enabled=true;
		gyroEnabled = true;
		ResetBaseOrientation();
		UpdateCalibration(true);
		UpdateCameraBaseRotation(true);
		RecalculateReferenceRotation();
	}





	private void UpdateCalibration(bool onlyHorizontal)
	{
		if (onlyHorizontal)
		{
			var fw = (Input.gyro.attitude) * (-Vector3.forward);
			fw.z = 0;
			if (fw == Vector3.zero)
			{
				calibration = Quaternion.identity;
			}
			else
			{
				calibration = (Quaternion.FromToRotation(baseOrientationRotationFix * Vector3.up, fw));
			}
		}
		else
		{
			calibration = Input.gyro.attitude;
		}
	}

	private void UpdateCameraBaseRotation(bool onlyHorizontal)
	{
		if (onlyHorizontal)
		{
			var fw = transform.forward;
			fw.y = 0;
			if (fw == Vector3.zero)
			{
				cameraBase = Quaternion.identity;
			}
			else
			{
				cameraBase = Quaternion.FromToRotation(Vector3.forward, fw);
			}
		}
		else
		{
			cameraBase = transform.rotation;
		}
	}
	

	private static Quaternion ConvertRotation(Quaternion q)
	{
		return new Quaternion(q.x, q.y, -q.z, -q.w);	
	}
	

	private Quaternion GetRotFix()
	{

		return Quaternion.identity;

	}
	

	private void ResetBaseOrientation()
	{
		baseOrientationRotationFix = GetRotFix();
		baseOrientation = baseOrientationRotationFix * baseIdentity;
	}


	private void RecalculateReferenceRotation()
	{
		referanceRotation = Quaternion.Inverse(baseOrientation)*Quaternion.Inverse(calibration);
	}


}
