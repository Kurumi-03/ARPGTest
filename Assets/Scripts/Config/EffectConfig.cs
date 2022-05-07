using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//管理特殊状态
//管理设为int  是因为可能会收到多个状态的叠加 使效果发生叠加bool无法做到对应效果
public class EffectConfig
{   
    public struct EffectData{
        public int limiteMove;//限制移动
        public int limiteAtk;//限制攻击
        public int limiteSKill;//限制技能
        public int isBlock;//是否击退
        public int aniPause;//动画暂停
        public int stateChange;//状态是否切换
    }

    public static EffectData GetEffectData(string id){
        EffectData data = new EffectData();
        switch(id){
            case "Frozen":
                data.stateChange = 0;
                data.limiteMove = 1;
                data.limiteAtk = 1;
                data.limiteSKill = 1;
                data.aniPause = 1;
                data.isBlock = 0;
                break;
            case "Stop":
                data.stateChange = 0;
                data.limiteMove = 1;
                data.limiteAtk = 1;
                data.limiteSKill = 1;
                data.aniPause = 1;
                data.isBlock = 0;
                break;
            case "Block":
                data.isBlock = 1;
                break;
            case "AttackWait":
                data.stateChange = 1;
                break;
        }
        return data;
    }
}
