using UnityEngine;
using System.Collections;

public class gyroscope : MonoBehaviour {
  
    private const float lowPassFilterFactor = 0.2f; 
    private float adda,addb;
    private bool gyroBool;
    private Touch oldtouch1,oldtouch2;
    private Camera[] camera_arry;
	void Start () {
        adda = transform.eulerAngles.x;
        addb = transform.eulerAngles.y;
        //camera_arry=transform.GetComponentsInChildren<Camera>();


        //判断是否支持陀螺仪
        gyroBool = SystemInfo.supportsGyroscope;

        //设置设备陀螺仪的开启/关闭状态，使用陀螺仪功能必须设置为 true  
        Input.gyro.enabled = true;
        //获取设备重力加速度向量 
        Vector3 deviceGravity = Input.gyro.gravity;
        //设备的旋转速度，返回结果为x，y，z轴的旋转速度，单位为（弧度/秒）
        Vector3 rotationVelocity = Input.gyro.rotationRate;
        //获取更加精确的旋转         
        Vector3 rotationVelocity2 = Input.gyro.rotationRateUnbiased;
        //设置陀螺仪的更新检索时间，即隔 0.1秒更新一次         
        Input.gyro.updateInterval = 0.1f;
        //获取移除重力加速度后设备的加速度          
        Vector3 acceleration = Input.gyro.userAcceleration;   
		
	}
    
	void OnGUI()    
    {
        GUI.Label(new Rect(50, 100, 500, 20), "Label : " + Input.acceleration.x + "       " + Input.acceleration.y + "         " + Input.acceleration.z + "       " + Time.deltaTime);
        GUI.Label(new Rect(50, 80, 500, 20), "Label : " + Input.gyro.gravity + "       " + Input.gyro.rotationRate + "         " + Input.gyro.rotationRateUnbiased + "       " + Input.gyro.userAcceleration + "            " + Input.gyro.attitude);
        GUI.Label(new Rect(50,60,500,20),"Label:"+transform.eulerAngles.x+"        "+transform.eulerAngles.y);
        GUI.Label(new Rect(50, 60, 500, 20), "陀     螺    仪    " + gyroBool);

       
    }  
	void Update () {
        //if (transform.eulerAngles.y <= 80f && transform.eulerAngles.y >= -80)
        //{
        //    adda += Input.acceleration.x * 15; 
        //}
        //else {
        //   adda = Mathf.Clamp(adda,-80f,80f);
        //}
        adda += Input.acceleration.x * 15;
        addb -= Input.acceleration.y * 15;
       // transform.rotation = Quaternion.Euler(0,adda,0);
        transform.rotation = Quaternion.Slerp(transform.rotation,Input.gyro.attitude,lowPassFilterFactor);
        //#if UNITY_EDITOR
//        if (Input.GetMouseButton(0)) {
//            float a=Input.GetAxis("Mouse X");
//            float b=Input.GetAxis("Mouse Y");
//            adda+=a;
//            addb+=b;
//            transform.rotation=Quaternion.Euler(addb,adda,0);
//        }
//        if (Input.GetAxis("Mouse ScrollWheel")!=0) {
//
//            for (int i = 0; i <camera_arry.Length ; i++) {
//                if (camera_arry[i].fieldOfView<=70f&&camera_arry[i].fieldOfView>=50f) {
//                    camera_arry[i].fieldOfView-=Input.GetAxis("Mouse ScrollWheel");
//                }else{
//                    camera_arry[i].fieldOfView=Mathf.Clamp(camera_arry[i].fieldOfView,50f,70f);
//                }
//
//            }
//        }
		//#elif UNITY_ANDROID

       
        //Input.gyro.attitude 返回值为 Quaternion类型，即设备旋转欧拉角  
       
       // transform.rotation = Input.gyro.attitude;


        

		//#endif
	}
}



 
    