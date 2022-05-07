using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InputController : MonoBehaviour
{
    public static InputController _instance;
    public Vector2 movement;//获取到x轴，z轴变化输入
    public Vector3 cameraValue;//获取到鼠标的x，y位置和滑轮数值
    public bool isAttack;//传递是否能攻击的判断
    public bool isBlock;//格挡的判定
    private int chargeFrame;//按下键盘时到蓄力开始的帧数计时
    public bool isCharge;//判断是否在蓄力
    

    private void Awake() {
        _instance = this;
    }

    private void Update() {
        //后续可通过修改movement的值来修改input的方式
        movement.Set(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        cameraValue.Set(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"),Input.GetAxis("Mouse ScrollWheel"));
        if(Input.GetButtonDown("Fire1")){
            isAttack = true;
            StartCoroutine(AtkWait());//在下一帧将判断设回
        }
        //格挡技能的释放   不能与普攻同时释放  所以放在else里
        else if(Input.GetMouseButtonDown(1)){
            isBlock = true;
            StartCoroutine(BlockWait());
        }
        //蓄力技能的释放
        if(Input.GetKey(KeyCode.Q)){
            chargeFrame++;
            //按下超过3帧开始蓄力
            if(chargeFrame > 3){
                isCharge = true;
            }
        }
        //松开键盘时
        else{
            isCharge = false;
        }
        //打开UI界面
        if(Input.GetKeyDown(KeyCode.B)){
            UIController.Instancee.OpenUI("EquipUI");
        }
    }
    //在下一帧将攻击设回false
    IEnumerator AtkWait(){
        yield return 0;
        isAttack = false;
    }

    IEnumerator BlockWait(){
        yield return 0;
        isBlock = false;
    }
}
