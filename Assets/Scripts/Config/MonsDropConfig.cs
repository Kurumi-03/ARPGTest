using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsDropConfig : MonoBehaviour
{
    public struct Data{
        public int exp;//掉落经验值
        public int money;//掉落金钱
        //掉落物1
        public string drop1Id;//掉落物id
        public float drop1Rate;//掉落概率
        public int drop1MaxNum;//掉落物最大数量
        public int drop1MinNum;//掉落物最小数量
        //掉落物2
        public string drop2Id;//掉落物id
        public float drop2Rate;//掉落概率
        public int drop2MaxNum;//掉落物最大数量
        public int drop2MinNum;//掉落物最小数量
        //掉落物3
        public string drop3Id;//掉落物id
        public float drop3Rate;//掉落概率
        public int drop3MaxNum;//掉落物最大数量
        public int drop3MinNum;//掉落物最小数量
    }

    public static Data GetDropInfo(string monsId){
        Data data = new Data();
        switch(monsId){
            case "mons1001":
                data.exp = 25;
                data.money = 100;
                data.drop1Id = "item1001";  data.drop1Rate = 50;  data.drop1MinNum = 1;    data.drop1MaxNum = 1;
                data.drop2Id = "item1002";  data.drop2Rate = 30;  data.drop2MinNum = 1;    data.drop2MaxNum = 1;
                data.drop3Id = "item1003";  data.drop3Rate = 10;  data.drop3MinNum = 1;    data.drop3MaxNum = 1;
            break;
        }
        return data;
    }
}
