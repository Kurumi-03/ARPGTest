using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataConfig : MonoBehaviour
{
    public struct CreatMonsterData{
        public string monsterName;
        public float minNum;//最少怪物数
        public float maxNum;//最大怪物数
    }

    public static int weaponId;//武器id

    public static List<CreatMonsterData> GetCreatMonster(string map){
        CreatMonsterData mData;
        List<CreatMonsterData> dataList = new List<CreatMonsterData>();
        switch(map){
            case "ToWeld":
                mData.monsterName = "mons1001";
                mData.minNum = 5;
                mData.maxNum = 10;
                dataList.Add(mData);
                break;
            default:
                mData.monsterName = "mons1001";
                mData.minNum = 5;
                mData.maxNum = 10;
                dataList.Add(mData);
                break; 
        }
        return dataList;
    }
}
