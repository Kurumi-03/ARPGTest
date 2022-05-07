using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    //输入技能名   攻击者属性   被攻击者属性
    public static void skillLogic(string atkId , Attribute attacker , Attribute target){
        //当攻击者或目标hp小于0时  不执行
        if(attacker.hp < 0 || target.hp < 0) return;
        // Debug.Log("技能名:" + atkId);
        // Debug.Log("攻击者名字:" + attacker.cName);
        // Debug.Log("目标名字:" + target.cName);
        Global.FightInfo fightInfo = new Global.FightInfo();
        CaleDamage(atkId,attacker,target,ref fightInfo);
        CaleState(atkId,attacker,target,ref fightInfo);
        Ifight ifight = target as Ifight;
        ifight.BeHit(fightInfo);
        //可以再结算一次攻击者的状态
        // ifight = attacker as Ifight;
        // ifight.BeHit(fightInfo);
    }

    //结算状态
    static void CaleState(string atkId,Attribute attacker,Attribute target,ref Global.FightInfo info){
        SkillConfig.SKillData sKillData = SkillConfig.getSKillInfo(atkId);//获取技能信息
        info.fightStates = sKillData.fightStates;//被攻击者的状态调整为技能产生后的状态
        //Debug.Log("状态为:" + info.fightStates[0].ToString());
    }

    //结算伤害
    static void CaleDamage(string atkId,Attribute attacker,Attribute target,ref Global.FightInfo info){
        SkillConfig.SKillData sKillData = SkillConfig.getSKillInfo(atkId);
        //计算各种伤害
        //(由自身的攻击力 加上装备附加的攻击力)乘以伤害加成 加上 技能所带伤害  减去 目标防御力 即为所求
        float damage = (attacker.atk + EquipConfig.getDropInfo(GameDataConfig.weaponId).atk) * sKillData.damageAdd + sKillData.damage - target.def;
        //当不足以破防时  伤害为1
        if(damage <= 0){
            damage = 1;
        }
        info.damage = damage;
    }
}
