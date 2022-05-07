using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpConfig : MonoBehaviour
{
    public struct ExpData{
        public int lv;//等级
        public int targetExp;//下一级所需经验
    }

    public static ExpData GetExpInfo(int level){
        ExpData expData = new ExpData();
        switch(level){
            case 2:
            expData.targetExp = 25;
            break;
            case 3:
            expData.targetExp = 50;
            break;
            case 4:
            expData.targetExp = 75;
            break;
            case 5:
            expData.targetExp = 100;
            break;
        }
        return expData;
    }
}
