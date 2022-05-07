using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;//DoTween插件使用

public class MonsterController : MonsterAttribute , Ifight
{
    public enum State{
        NoneTarget,//丢失目标
        LocatedTarget,//锁定目标
        Death,//死亡
        Wait,//等待
        AttackRun,//追击
        Attack,//攻击决策
        AttackWait,//攻击前摇
        AttackStart,//攻击开始
        Attacking,//攻击中
        AttackEnd,//攻击结束
        Blocking,//击退中
        Stop//硬直
    }

    //protected float hp = 10;
    private Image hpUI;//血量UI的控制
    protected Animator animator;
    private GameObject attackColider;//攻击碰撞体
    protected CharacterController controller;

    //当前属性的值(临时变量)   可在初始化时使用属性类的定义进行赋值
    protected float cattackCD = 2;//当前攻击CD
    protected float cattackWaitTime = 0.3f;//当前硬直时间
    protected float cwaitTime = 1;//当前等待时间
    protected float cattackWaitFTime = 1;//当前攻击前摇时间
    protected float cattackWaitBTime = 1;//当前攻击后摇时间

    public Transform target;//目标位置的获取
    GameObject imgState;//头上的状态图标
    public State state;

    EffectConfig.EffectData eData;
    float blockSpeed = 15;//击退速度
    float blockAddSpeed;//击退加速度
    float frozenTime = 5;//冰冻时间
    float apartFTime = -1;//解除冰冻时间

    private void Awake() {
        Binding();
        //cattackWaitBTime = this.attackWaitBTime;
    }   

    private void Update() {
        //检测状态  无状态返回
        if(!CheckState()) return;
        //没有目标时  搜索目标
        if(state == State.NoneTarget){
            TryToFindTarget();
        }
        //锁定目标后 执行锁定目标的行为
        else if(state == State.LocatedTarget){
            LocatedTarget();
        }
        //追击时
        else if(state == State.AttackRun ){
            AttackRun();
        }
        //攻击
        else if(state == State.Attack){
            Attack();
        }
        //攻击前摇
        else if(state == State.AttackWait){
            AttackWait();
        }
        //攻击开始
        else if(state == State.AttackStart){
            AttackStart();
        }
        //攻击中
        else if(state == State.Attacking){
            Attacking();
        }
        //攻击后摇
        else if(state == State.AttackEnd){
            AttackEnd();
        }
        //等待
        else if(state == State.Wait){
            Wait();
        }
        //击退
        else if(state == State.Blocking){
            Block();
        }
        //Debug.Log(state.ToString());//打印当前状态
        //Debug.Log("怪物是否击退:" + eData.isBlock);
        Debug.Log("怪物hp:" + hp);
        Debug.Log("怪物id:" + monsterName);
    }

    //初始化  将需要获取的组件赋值
    public void Binding(){
        state = State.NoneTarget;
        this.monsterName = "mons1001";
        this.monSpeed = 4.5f;
        this.hp = 200;
        this.atk = 50;
        controller = GetComponent<CharacterController>();
        bullet = Resources.Load<GameObject>("tx/Tx1001");
        animator = GetComponent<Animator>();
        //避免当前未能获取到组件
        if(animator == null){
            animator = transform.GetChild(0).GetComponent<Animator>();
        }
    }

    //检测是否处于硬直状态，硬直时不执行任何操作
    bool CheckState(){
        //判断是否死亡
        if(state == State.Death){
            return false;
        }
        //判断是否暂停动画
        if(eData.aniPause > 0){
            animator.speed = 0;//暂停动画播放
        }
        else{
            animator.speed = 1;
        }
        //当前实际节点处于要解除冰冻时
        if(apartFTime != -1 && Time.time > apartFTime){
            //可以在此处加上解除冰冻动画
            eData.aniPause -= 1;
            eData.limiteAtk -= 1;
            eData.limiteMove -= 1;
            eData.limiteSKill -= 1;
        }
        //处于击退
        if(eData.isBlock > 0){
            state = State.Blocking;
        }
        //处于硬直
        if(cattackWaitTime >= 0){
            cattackWaitTime -= Time.deltaTime;
            return false;
        }
        else{
            //处于硬直状态时
            if(state == State.Stop){
                state = State.NoneTarget;
            }
        }
        return true;
    }

    //尝试找到目标
    void TryToFindTarget(){
        //获得玩家与怪物之间的距离
        float distance = Vector3.Distance(transform.position,PlayerController._instance.transform.position);
        //当距离小于怪物的听觉范围 执行找到目标
        if(distance <= earSight){
            FindTarget();
        }
        else{
            //当距离小于视觉距离 且 怪物与玩家位置夹角小于怪物视野的一半时   即  判定为在视野范围内
            if(distance <= eyeSight && ToolMethod.GetEular(PlayerController._instance.transform.position,transform) < (GameConfig.eyeSightEular / 2)){
                FindTarget();
            }
            //否则视为脱离怪物视线范围 即解除目标锁定
            else{
                ReleaseTarget();
            }   
        }
    }

    //发现目标
    void FindTarget(){
        state = State.LocatedTarget;//锁定目标
        target = PlayerController._instance.transform;//将目标设为主角
        //头上出现反应图标
        // if(!!imgState){
        //     imgState.SetActive(true);
        //     Invoke("关闭图标", 0.4f);
        // }
    }

    //解除目标锁定
    void ReleaseTarget(){
        state = State.NoneTarget;//将状态设为无目标
        target = null;
    }

    //锁定目标并判断是否追击
    void LocatedTarget(){
        int random = Random.Range(0,100);//设定随机数来决定概率
        //随机数小于追击概率  则判定为追击
        if(random <= attackRunRate){
            //调用插件的转向方法   使怪物在1.3s时间转向到玩家所在位置(但还是处在怪物所在高度)
            transform.DOLookAt(new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z) , 1.3f);
            state = State.AttackRun;
        }
        else{
            state = State.Wait;
        }
    }

    //追击行为
    void AttackRun(){
        //检测是否可以攻击
        if(CheckAttack()){
            state = State.Attack;//可以时进入攻击状态
            animator.SetBool("walking",false);//攻击时停止移动动画
            return;
        }
        //检测是否失去目标
        if(CheckLose()){
            state = State.NoneTarget;//进入失去目标状态
            animator.SetBool("walking",false);
            return;
        }
        //转向到玩家方向并移动到玩家位置
        transform.LookAt(new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z));
        //controller.Move(-Vector3.up * Time.deltaTime * moveSpeed);//模拟重力效果
        controller.Move(transform.forward * Time.deltaTime * monSpeed);
        animator.SetBool("walking",true);
    }

    //检测是否可以攻击
    bool CheckAttack(){
        float distance = Vector3.Distance(transform.position,PlayerController._instance.transform.position);
        //Debug.Log("是否可攻击" + distance);
        //当玩家与怪物的距离小于攻击距离时
        if(distance <= attackLength){
            return true;
        }
        return false;
    }

    //检测是否失去目标
    bool CheckLose(){
        float distance = Vector3.Distance(transform.position,PlayerController._instance.transform.position);
        //当玩家与怪物的距离大于失去目标距离时
        if(distance > loseTargetLength){
            return true;
        }
        return false;
    }

    //执行攻击行为
    void Attack(){
        //可拓展技能的概率
        int random = Random.Range(0,100);
        //小于攻击概率即开始攻击 且不限制攻击时
        if(random < attackRate && eData.limiteAtk == 0){
            animator.SetBool("beforeAtk",true);
            //在0.6s时间内转向到玩家所在方位
            transform.DOLookAt(new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z) , 0.6f);
            state = State.AttackWait;//状态切换为攻击前摇
        }
        //大于时处于发呆状态  不进行攻击
        else{
            state = State.Wait;
        }
    }

    //攻击前摇
    void AttackWait(){
        cattackWaitFTime -= Time.deltaTime;
        if(cattackWaitFTime <= 0){
            animator.SetTrigger("atk");//执行攻击动画  没有前摇动作
            state = State.AttackStart;//状态切换为攻击开始
            cattackWaitFTime = attackWaitFTime;//将前摇时间重置
        }

    }

    //开始攻击
    void AttackStart(){
        state = State.Attacking;//切换为攻击中
        //若在当前受击  切换为攻击前摇状态 并重置攻击CD
        // cattackCD -= Time.deltaTime;
        // if(cattackCD <= 0){
        // }
        StartCoroutine(Atk());
    }

    //开始执行攻击的相关步骤
    public virtual IEnumerator Atk(){
        yield return 0;//等待0帧  即直接开始下面步骤
        //转向到当前玩家方向
        transform.LookAt(new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z));
        Vector3 dir = transform.forward;//记录当前正方向
        yield return new WaitForSeconds(attackTime);//等待法球生成时间后执行后续
        //最好不要直接生成子弹  制作一个工厂类来制造子弹
        GameObject bullets = Instantiate(bullet);//生成子弹
        //属性中有生成位置 可改写
        bullets.transform.position = transform.position + transform.forward;//子弹生成位置为怪物位置前方
        bullets.transform.LookAt(bullets.transform.position + new Vector3(dir.x,0,dir.z));//子弹的朝向设为子弹当前位置叫上怪物的朝向方位
        bullets.GetComponent<Rigidbody>().velocity = bullets.transform.forward * 5;//子弹速度  可在配置表中优化
        state = State.AttackEnd;//切换到后摇状态
    }

    //攻击中  攻击中可以设置无敌
    void Attacking(){

    }

    //攻击后摇
    void AttackEnd(){
        cattackWaitBTime -= Time.deltaTime;
        if(cattackWaitBTime <= 0){
            animator.SetBool("beforeAtk",false);
            state = State.NoneTarget;//切换状态为没有目标
            cattackWaitBTime = attackWaitBTime;
        }
    }

    //等待
    void Wait(){
        //发呆时是否看向主角
        if(isLookToPlayer){
            //转向至主角位置
            transform.LookAt(new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z));
        }
        cwaitTime -= Time.deltaTime;
        if(cwaitTime <= 0){
            state = State.NoneTarget;//切换为无目标状态
            cwaitTime = waitTime;
        }
    }

    //击退
    void Block(){
        //还处于击退中
        if(blockSpeed > 2f){
            blockAddSpeed += Time.deltaTime * 20;
            blockSpeed -= blockAddSpeed;
            //使用角色控制器  向怪物的后方移动
            controller.Move(this.transform.forward * -blockSpeed *Time.deltaTime);
            return;
        }
        else{
            //
            state = State.NoneTarget;
            eData.isBlock = 0;
            blockSpeed = 15;
            blockAddSpeed = 0;
        }
    }

    //实现Ifight接口的方法来更新怪物状态
    public void BeHit(Global.FightInfo info){
        //Debug.Log("函数Behit调用");
        //当前hp为0时  直接返回
        if(this.hp == 0) return;
        this.hp -= info.damage;//伤害结算
        eData = FightTool.ConverState(info.fightStates);
        //当伤害结算完毕后怪物还未死亡时
        if(!CheckDie()){
            foreach(var fstate in info.fightStates){
                if(fstate == Global.FightState.Frozen){
                    apartFTime = Time.time + frozenTime;//记录解除冰冻的实际点
                }
            }
        }
    }

    //检测万物是否存活
    bool CheckDie(){
        if(this.hp <= 0){
            state = State.Death;
            StartCoroutine(Die());
            return true;
        }
        return false;
    }

    IEnumerator Die(){
        yield return new WaitForSeconds(2);
        //掉落方法
        Drop();
        Destroy(gameObject);
        Debug.Log("执行destroy方法");
    }

    public void Drop(){
        Debug.Log("传入的怪物id:" + monsterName);
        DropController.DoDrop(monsterName,this.transform.position);//传入怪物参数
    }
}
