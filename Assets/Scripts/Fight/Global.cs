using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//技能和普攻产生的效果和信息
public class Global : MonoBehaviour
{
    //攻击后产生的状态
    public enum FightState{
        AttackWait,//硬直
        Frozen,//冰冻
        Block,//击退
        Stop//禁言  不能使用技能
    }

    //攻击信息
    public struct FightInfo{
        public float damage;//伤害值
        public bool crit;//暴击
        public FightState[] fightStates; 
    }

    //状态的具体效果
    public struct FightStateEffect{
        public bool isMove;//是否能移动
        public bool isAttack;//是否能攻击
        public bool isSKill;//是否能使用技能
        public bool isBlock;//是否击退
    }

}
