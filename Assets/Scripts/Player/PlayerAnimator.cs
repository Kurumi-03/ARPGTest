using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    InputController input;

    public static PlayerAnimator _instance;//单例模式
    public static PlayerAnimator Instance{
        get{
            if(_instance == null){
                _instance = FindObjectOfType<PlayerAnimator>();
            }
            return _instance;
        }
    }

    private void Awake() {
        _instance = this;
        animator = transform.GetChild(0).GetComponent<Animator>();
        input = FindObjectOfType<InputController>();
    }

    //需要将人物的控制核心放在同一个控制脚本中
    public void FuncUpdate() {
        //因为动画的切换条件是h和v大于0  所以需要获取输入值的绝对值
        animator.SetFloat("horizontal" , Mathf.Abs(input.movement.x));
        animator.SetFloat("vertical" , Mathf.Abs(input.movement.y));
        //避免trigger一直调用时连招无法正常使用  需要每一帧都将攻击trigger设回去
        animator.ResetTrigger("atk");
        //设置连招的时间判定    将当前状态的运行时间到1f  重复赋值给anitime
        animator.SetFloat("AniTime",Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime,1f));
        //蓄力动作
        animator.SetBool("charge",input.isCharge);
    }

    //攻击动作的显示
    public void Atk(){
        animator.SetTrigger("atk");
    }
    public void Bolck(){
        animator.SetTrigger("block");
    }
}
