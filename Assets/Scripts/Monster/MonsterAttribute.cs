using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterAttribute : Attribute
{
    public string monsterName;
    protected float attackCD = 2;//攻击CD
    protected float attackWaitTime = 0.3f;//攻击硬直时间
    protected float attackRate = 65;//攻击概率
    protected float attackRunRate = 45;//追击概率
    protected float waitTime = 0.3f;//等待时间
    protected float attackWaitFTime = 1;//攻击前摇
    protected float attackTime = 0.5f;//攻击生效时间
    protected float attackWaitBTime = 1;//攻击后摇
    protected float eyeSight = 20;//视觉范围  视觉范围为120度
    protected float earSight = 13;//听觉范围
    protected float attackLength = 8;//攻击范围
    protected float loseTargetLength = 25;//失去目标距离
    protected bool isLookToPlayer;//是否看向玩家
    protected GameObject bullet;//子弹
    protected Vector3 attackPos = new Vector3(5 , 0 , 1);//子弹生成位置
    protected float monSpeed;
}
