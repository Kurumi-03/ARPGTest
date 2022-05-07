using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightTool
{
    public static EffectConfig.EffectData ConverState(Global.FightState[] states){
        //Debug.Log("ConverState函数调用");
        //当状态为空时  返回一个空状态集
        if(states == null || states.Length == 0) return new EffectConfig.EffectData();
        EffectConfig.EffectData data = new EffectConfig.EffectData();
        //更新每一个状态的效果数据
        foreach(var state in states){
            //Debug.Log("state:" + state);
            EffectConfig.EffectData ed = EffectConfig.GetEffectData(state.ToString());
            //Debug.Log("传入的isblock值为:" + ed.isBlock);
            data.isBlock += ed.isBlock;
            data.aniPause += ed.aniPause;
            data.stateChange += ed.stateChange;
            data.limiteAtk += ed.limiteAtk;
            data.limiteMove += ed.limiteMove;
            data.limiteSKill += ed.limiteSKill;
        }
        //Debug.Log("isBlock的值:" + data.isBlock);
        return data;
    }
}
