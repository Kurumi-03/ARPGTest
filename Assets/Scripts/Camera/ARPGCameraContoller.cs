using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPGCameraContoller : MonoBehaviour
{
    public Transform target;//摄像机追踪目标引用
    public float targetHeight = 1.8f;//目标上边距
    public float targetSide = -0.1f;//目标侧边距
    public float distance = 4;//目标距离
    public float minDistance = 2.2f;//最小距离
    public float maxDistance = 8;//最大距离
    public float xSpeed = 250;//水平旋转速度
    public float ySpeed = 125;//垂直旋转速度
    public float yLimiteMax = 72;//垂直旋转角度最大限制
    public float yLimiteMin = -10;//垂直旋转角度最小限制
    public float zoomRate = 80;//缩放倍率
    //临时变量
    public float x = 20;
    public float y = 0;

    InputController input;
    private void Awake() {
        input = FindObjectOfType<InputController>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        Cursor.lockState = CursorLockMode.Locked;//锁定鼠标
        Cursor.visible = true;//鼠标不可见
    }


    private void Update() {
        
    }

    private void LateUpdate() {
        x += input.cameraValue.x * xSpeed * Time.deltaTime;
        y -= input.cameraValue.y * ySpeed * Time.deltaTime;//避免造成鼠标与镜头的移动相反

        //根据鼠标滑轮来计算当前摄像机与目标的位置  -=是因为滑轮向后是负数但效果是加大距离
        distance -= (input.cameraValue.z * Time.deltaTime) * zoomRate * Mathf.Abs(distance);
        //避免距离超过最大值或最小值
        distance = Mathf.Clamp(distance , minDistance , maxDistance);

        y = ClampAngle(y,yLimiteMin,yLimiteMax);//限制并修正y轴角度
        Quaternion angel = Quaternion.Euler(y,x,0);//因为鼠标的y坐标对应的是x轴旋转
        transform.rotation = angel;

        //控制角色的旋转方向为摄像机当前朝向
        if(input.movement.x != 0 || input.movement.y != 0){
            target.transform.rotation = Quaternion.Euler(0,x,0);//角色不需要x轴上的旋转
        }

        //控制摄像机的跟随   将偏移量乘以旋转再乘以距离即 摄像机与角色之间的距离
        Vector3 position = target.position - ((angel * new Vector3(targetSide,0,1) * distance - new Vector3(0,targetHeight,0)));
        transform.position = position;
    }

    //修正旋转角度  当角度大于360或小于-360时，将角度修正
    private float ClampAngle(float angle,float min,float max){
        if(angle > 360){
            angle -= 360;
        }
        if(angle < -360){
            angle += 360;
        }
        return Mathf.Clamp(angle,min,max);
    }
}


