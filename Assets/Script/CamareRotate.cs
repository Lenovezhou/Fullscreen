using UnityEngine;
using System.Collections;

public class CamareRotate : MonoBehaviour {

    [HideInInspector]
    public string Fullscreen_meterial_name;
    public float camerarotaionspeed,scalespeed;
    private float adda, addb,camera_lastrotate_x,camera_lastrotate_y ,cameraview,sun_cameraview;
    private Camera camera,sun_camera;
    private Touch oldtouch1, oldtouch2;
    private Vector3 camera_lastpos;
    private GameObject target_camera;
    private Material Fullscreen_meterial;
    private int i=0;    //  场景索引；
	void Awake () {
        target_camera = GameObject.FindWithTag("MainCamera");
        
        if (Fullscreen_meterial_name=="")
        {
            Debug.Log(Fullscreen_meterial_name);
            Fullscreen_meterial_name = "House";
        }
        Fullscreen_meterial = (Material)Resources.Load(Fullscreen_meterial_name);
        //  初始化旋转角度
        camera_lastpos = target_camera.transform.position;
        camera_lastrotate_x = target_camera.transform.eulerAngles.x;
        camera_lastrotate_y = target_camera.transform.eulerAngles.y;
        adda = target_camera.transform.eulerAngles.x;
        addb = target_camera.transform.eulerAngles.y;
        //修改全景图
        MeshRenderer newmaterial = this.transform.GetChild(0).GetComponent<MeshRenderer>();
        newmaterial.material = Fullscreen_meterial;

        camera = target_camera.GetComponent<Camera>();
        sun_camera = target_camera.transform.GetChild(0).GetComponent<Camera>();
        cameraview = camera.fieldOfView;
        sun_cameraview = sun_camera.fieldOfView;
	}


    //  Boutton:回位
    public void Cameback()
    {
        transform.rotation = Quaternion.Euler(camera_lastrotate_x,camera_lastrotate_y,0);
        adda = transform.eulerAngles.x;
        addb = transform.eulerAngles.y;
    }
	

    // 切换场景Button：先得到Resources下所有文件
    //public void ChangePanel() 
    //{
    //   Object[] assets= Resources.LoadAll("");
    //   i += 1;
    //   if (i<assets.Length)
    //   {
    //       Fullscreen_meterial_name = assets[i].name;
    //   }
    //   else if (i==assets.Length)
    //   {
    //       i = 0;
    //       Fullscreen_meterial_name = assets[i].name;
    //   }
    //   Fullscreen_meterial = (Material)Resources.Load(Fullscreen_meterial_name);
    //}
	void Update () {
	

#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
          
            float a = Input.GetAxis("Mouse X");
            float b = Input.GetAxis("Mouse Y");
            adda += a;
            addb += b;
            Vector3 newrotate = new Vector3(-addb,-adda,0)*camerarotaionspeed;
            target_camera.transform.rotation = Quaternion.Euler(newrotate);
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0) 
        {
           
            float scrollwheel = Input.GetAxis("Mouse ScrollWheel")*scalespeed;
           
            //  调整camera的field of view来实现缩放：
            if (cameraview >= 30 && cameraview <= 70)
            {
                cameraview -= scrollwheel;
                sun_cameraview -= scrollwheel;
                sun_camera.fieldOfView = sun_cameraview;
                camera.fieldOfView = cameraview;
            }
            else {
                cameraview = Mathf.Clamp(cameraview,30f,70f);
                sun_cameraview = Mathf.Clamp(cameraview,30f,70f);
            }

            //Vector3 scale = new Vector3(transform.localScale.x+scrollwheel,transform.localScale.y+scrollwheel,transform.localScale.z+scrollwheel);
            //transform.localScale = scale;
        }
#elif UNITY_ANDROID
        if (Input.touchCount==0)
        {
            return;
        }
        if (Input.touchCount==1)
        {
            adda += Input.GetTouch(0).deltaPosition.x;
            addb += Input.GetTouch(0).deltaPosition.y;
            Vector3 newrotation=new Vector3(-addb,-adda,0)*camerarotaionspeed;
            target_camera.transform.rotation = Quaternion.Euler(newrotation);
            //transform.Rotate(Vector3.up, Input.GetTouch(0).deltaPosition.x * camerarotaionspeed);
            //transform.Rotate(Vector3.right,Input.GetTouch(0).deltaPosition.y*camerarotaionspeed);
        }
        Touch newtouch1 = Input.GetTouch(0);
        Touch newtouch2 = Input.GetTouch(1);
        if (Input.GetTouch(1).phase==TouchPhase.Began)
        {
            oldtouch1 = newtouch1;
            oldtouch2 = newtouch2;
            return;
        }
        float oldDistance = Vector2.Distance(oldtouch1.position,oldtouch2.position);
        float newDistance = Vector2.Distance(newtouch1.position,newtouch2.position);
        float offset = oldDistance - newDistance;
        float scaleFort = offset / 100f;
       
        if (cameraview >= 30 && cameraview <= 70)
        {
            cameraview += scaleFort;
          
            sun_camera.fieldOfView = cameraview;
            camera.fieldOfView = cameraview;
        }
        else
        {
            cameraview = Mathf.Clamp(cameraview,30f,70f);
          //  sun_cameraview = Mathf.Clamp(sun_cameraview, 30f, 70f);
        }
       
#endif



    }
}
