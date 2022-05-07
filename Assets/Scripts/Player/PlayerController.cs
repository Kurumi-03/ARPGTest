using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerAttribute
{
    public static PlayerController _instance;
    private CharacterController controller;
    private PlayerAnimator playerAni;
    private bool canAttack = true;//是否能攻击
    private bool canBlock = true;//是否能格挡
    private float blockTime = 0;//记录按下格挡键的时间长度
    private float blockBounceTime = 0.5f;//格挡判定时间
    //public float movespeed = 4.5f;

    private float skillAdd = 0;//蓄力值(计算伤害使用)
    private float skillAddMin = 1;//最小蓄力值
    private float skillAddMax = 3;//最大蓄力值
    private int currentSkill = 0;//当前释放技能
    private bool isSkilled = true;//技能中是否可以并发释放技能
    private bool isAttack = true;//技能中是否可以攻击
    private bool isMove = true;//技能中是否可以移动
    public float SkillLastTime = 3.5f;//技能持续时间

    InputController input;

    private void Awake() {
        _instance = this;
        controller = GetComponent<CharacterController>();
        input = FindObjectOfType<InputController>();
        playerAni = FindObjectOfType<PlayerAnimator>();
        this.moveSpeed = 4.5f;
        this.hp = 200;
        this.atk = 20;
        this.cName = "玩家";
    }

    private void Update() {
        playerAni.FuncUpdate();//需要在atk方法之前
        Atk();
        Move(); 
        Skill();
    }

    //移动
    private void Move(){
        //释放技能时不能移动
        if(!isMove) return;
        //y设为-1是因为使用了角色控制器没有重力, 模拟重力效果
        Vector3 dir = transform.TransformDirection(new Vector3( input.movement.x , -1 , input.movement.y));
        controller.Move(dir * moveSpeed * Time.deltaTime);
    }

    //攻击
    private void Atk(){
        //释放技能不能攻击状态时
        if(!isAttack) return;
        //判断是否能攻击 （鼠标左键的点击和处于能攻击的状态时）
        if(input.isAttack && canAttack){
            PlayerAnimator._instance.Atk();//执行攻击动画的播放
        } 
        if(input.isBlock && canBlock){
            blockTime = Time.time;
            canBlock = false;//不能使用格挡
            PlayerAnimator._instance.Bolck();
            StartCoroutine(BolckWait());//在0.5秒后才能继续格挡
        }  

    }

    //技能
    private void Skill(){
        //判断是否可以并发释放技能  且当前技能是否为1001(蓄力攻击的代号)
        if(!isSkilled && currentSkill != 1001) return;
        //按下超过3帧时开始技能
        if(input.isCharge){
            Skill1();
        }
        //松开按键 且蓄力值不为0时
        else if(skillAdd > 0){
            SkillUse();
        }
    }

    //技能开始
    private void Skill1(){
        currentSkill = 1001;
        isAttack = false;
        isMove = false;
        isSkilled = false;
        skillAdd += Time.deltaTime;
        skillAdd = Mathf.Clamp(skillAdd,skillAddMin,skillAddMax);//规定蓄力值
    }

    //技能释放
    private void SkillUse(){
        if(skillAdd <= 0) return;
        skillAdd = 0;
        StartCoroutine(SkillLast());
    }

    //技能持续一定时间后设回
    IEnumerator SkillLast(){
        yield return new WaitForSeconds(SkillLastTime);//等待3.5s时间
        //将技能数值还原
        isMove = true;
        isAttack = true;
        isSkilled = true;
        currentSkill = 0;
    }
    
    IEnumerator BolckWait(){
        yield return new WaitForSeconds(0.5f);//等待0.5s
        canBlock = true;
    }

    //直接使用了player的碰撞体进行碰撞检测  所以运行效果不太好
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "MonsAtkTrigger"){
            //Debug.Log("碰撞");
            GetHit(other.gameObject);  
        }
        //与掉落物直接的碰撞检测   即拾取
        else if(other.tag == "DropItem"){
            GameDataController.AddItem(other.name,1);
            Destroy(other.gameObject);
        }
    }

    //处理格挡事件
    public void GetHit(GameObject ob){
        //Debug.Log("执行函数GetHit");
        float timer = Time.time;//碰撞时间
        //当碰撞时间和按下按键时间小于一定间隔时
        if(timer - blockTime <= blockBounceTime){
            Destroy(ob);
            Debug.Log("格挡成功");
        }
    }
}
