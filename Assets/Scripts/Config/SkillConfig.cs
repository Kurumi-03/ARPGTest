using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillConfig : MonoBehaviour
{
    public struct SKillData{
        public string id;
        public float damageAdd;//伤害加成  能叠加伤害
        public float damage;//伤害值
        public int level;//技能等级
        public string txName;//特效名
        public Global.FightState[] fightStates;//技能状态
    }

    //静态方法获取技能信息
    public static SKillData getSKillInfo(string id){
        SKillData sKillData = new SKillData();
        switch(id){
            //三连击的第一段
            case "atk1":
            sKillData.damageAdd = 1;
            sKillData.damage = 0;
            sKillData.fightStates = new Global.FightState[1];
            sKillData.fightStates[0] = Global.FightState.Block;//产生击退
            break;
            //三连击的第二段
            case "atk2":
            sKillData.damageAdd = 1.1f;
            sKillData.damage = 0;
            break;
            //三连击的第三段
            case "atk3":
            sKillData.damageAdd = 1.5f;
            sKillData.damage = 0;
            sKillData.fightStates = new Global.FightState[1];
            sKillData.fightStates[0] = Global.FightState.Frozen;//产生冰冻
            break;
            //蓄力攻击
            case "skill1001":
            sKillData.damageAdd = 1.5f;
            sKillData.damage = 500;
            sKillData.fightStates = new Global.FightState[2];
            sKillData.fightStates[0] = Global.FightState.Block;//产生击退
            sKillData.fightStates[1] = Global.FightState.AttackWait;//产生硬直
            break;
            case "skill1002":
            sKillData.damageAdd = 0;
            sKillData.damage = 0;
            sKillData.fightStates = new Global.FightState[1];
            sKillData.fightStates[0] = Global.FightState.AttackWait;//产生硬直
            break;
            case "skill1003":
            sKillData.damageAdd = 1;
            sKillData.damage = 10;
            sKillData.fightStates = new Global.FightState[1];
            sKillData.fightStates[0] = Global.FightState.Stop;//产生禁止释放技能
            break;
        }
        return sKillData;
    }
}
