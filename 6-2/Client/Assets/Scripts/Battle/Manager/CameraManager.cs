
using UnityEngine;

public class CameraManager : UpdateGame
{
    static public CameraManager Instance;

    private new Camera camera;
    public float speed = 1;
    private Vector3 PreMouseMPos;
    // 缩放系数  
    public float distance = .5f;
    // 左右滑动移动速度  
    public float XSpeed = 10;
    // 左右滑动移动速度  
    public float YSpeed = 30;
    // 缩放限制系数  
    public float minDistance = -20;
    public float maxDistance = 80;
    //跟随速度
    public float follwSpeed = 10;
    // 记录上一次手机触摸位置判断用户是在做放大还是缩小手势  
    private Vector2 oldPosition1 = new Vector2(0, 0);
    private Vector2 oldPosition2 = new Vector2(0, 0);
    private Transform target;
    // 函数返回真为放大，返回假为缩小  
    bool isEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {
        // 函数传入上一次触摸两点的位置与本次触摸两点的位置计算出用户的手势  
        float leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
        float leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));

        if (leng1 < leng2)
        {
            // 放大手势  
            return true;
        }
        else
        {
            // 缩小手势  
            return false;
        }
    }

    private void Awake()
    {
        Instance = this;
        camera = GetComponentInChildren<Camera>();
    }
    protected override void OnUpdate()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer
            || Application.platform == RuntimePlatform.WindowsEditor
            )
        {
            camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel");
            //鼠标中键按下，上下左右移动相机  
            if (Input.GetMouseButton(1))
            {
                if (target != null)
                {
                    target = null;
                    return;
                }
                if (PreMouseMPos.x <= 0)
                {
                    PreMouseMPos = new Vector3(Input.mousePosition.x, 0.0f, Input.mousePosition.y);
                }
                else
                {
                    Vector3 CurMouseMPos = new Vector3(Input.mousePosition.x, 0.0f, Input.mousePosition.y);
                    Vector3 offset = CurMouseMPos - PreMouseMPos;
                    offset = -offset * 0.01f;//0.1这个数字的大小可以调节速度  
                    transform.Translate(offset);
                    PreMouseMPos = CurMouseMPos;
                }
            }
            else
            {
                PreMouseMPos = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }
        else
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidUpdate();
        }
    }
    void AndroidUpdate()
    {
        float x = 0;
        float y = 0;
        float orthographicSize = camera.orthographicSize;
        // 判断触摸数量为单点触摸  
        if (Input.touchCount == 1)
        {
            // 触摸类型为移动触摸  
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (target != null)
                {
                    target = null;
                    return;
                }
                //根据触摸点计算X与Y位置  
                x = Input.GetAxis("Mouse X") * XSpeed * Time.deltaTime;
                y = Input.GetAxis("Mouse Y") * YSpeed * Time.deltaTime;
            }
            transform.position += new Vector3(-x, 0, -y);
        }
        else
        // 判断触摸数量为多点触摸  
        if (Input.touchCount > 1)
        {
            // 前两只手指触摸类型都为移动触摸  
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                // 计算出当前两点触摸点的位置  
                var tempPosition1 = Input.GetTouch(0).position;
                var tempPosition2 = Input.GetTouch(1).position;

                // 函数返回真为放大，返回假为缩小  
                if (isEnlarge(oldPosition1, oldPosition2, tempPosition1, tempPosition2))
                {
                    orthographicSize -= distance;
                }
                else
                {
                    orthographicSize += distance;
                }
                // 备份上一次触摸点的位置，用于对比  
                oldPosition1 = tempPosition1;
                oldPosition2 = tempPosition2;

            }
            orthographicSize = Mathf.Clamp(orthographicSize, minDistance, maxDistance);
            camera.orthographicSize = orthographicSize;
        }
    }
    void LateUpdate()
    {
        FollowTarget();
    }
    void FollowTarget()
    {
        if (target == null) return;
        Vector3 point = transform.position;
        float y = point.y;
        point = Vector3.Lerp(point, target.position, follwSpeed * Time.deltaTime);
        point.y = y;
        transform.position = point;
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}