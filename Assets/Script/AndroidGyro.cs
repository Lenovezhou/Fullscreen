using UnityEngine;
using System.Collections;

public class AndroidGyro : MonoBehaviour {
      public float speed = 10.0F;

      private bool isrotation=false, istranslate=false;
      private Vector3 dir;
      private bool gyroBool;
	// Use this for initialization
	void Start () 
    {
        gyroBool = SystemInfo.supportsGyroscope;
	}
	
	// Update is called once per frame
	
	

    public void Canrotation()
    {
        isrotation = isrotation == true ? false : true;
    }

    public void cantranslate()
    {
        istranslate = istranslate == true ? false : true;
    }


    void OnGUI()
    {
        if (!gyroBool)
        {
             GUI.Label(new Rect(50, 80, 500, 40), "当前设备不支持三轴陀螺仪！！！");
        }
      

        GUI.Label(new Rect(50, 100, 500, 40), "Label : " + Input.acceleration.x + "       " + Input.acceleration.y + "         " + Input.acceleration.z);
        //GUI.Label(new Rect(50, 80, 500, 20), "Label : " + Input.gyro.gravity + "       " + Input.gyro.rotationRate + "         " + Input.gyro.rotationRateUnbiased + "       " + Input.gyro.userAcceleration);

    }  



    void Update()
    {
        if (isrotation)
        {
            dir = Vector3.zero;
            dir.z = -Input.acceleration.y;
            dir.x = Input.acceleration.z;
            if (dir.sqrMagnitude > 1)
                dir.Normalize();

            dir *= Time.deltaTime;
            transform.Translate(dir * speed);
        }
       
        if (istranslate)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(dir.x,dir.z,dir.y));
        }
    }
    

}
