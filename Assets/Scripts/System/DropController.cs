using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropController : MonoBehaviour
{
    public struct DropData{
        public string id;//名称
        public int num;//数量
    }

    //掉落的静态方法  传入参数为怪物id和怪物位置
    public static void DoDrop(string monsId,Vector3 monsPos){
        Debug.Log("执行掉落方法");
        DropData dropData = new DropData();
        MonsDropConfig.Data data = MonsDropConfig.GetDropInfo(monsId);
        Debug.Log("data数据:" + data.exp);
        int rate = ToolRandom.rand_100();
        GameObject item;
        //在掉落概率为掉落一时
        if(rate < data.drop1Rate){
            dropData.id = data.drop3Id;
            //dropData.num = Random.Range(data.drop1MinNum,data.drop1MaxNum);//当需要掉落多个drop1的时候并将掉落位置散落开
            item = GameObject.Instantiate(Resources.Load<GameObject>("drop/" + data.drop1Id));
            item.name = data.drop1Id;
            item.transform.position = monsPos;
        }
        if(rate < data.drop2Rate){
            dropData.id = data.drop2Id;
            //dropData.num = Random.Range(data.drop2MinNum,data.drop2MaxNum);//当需要掉落多个drop1的时候并将掉落位置散落开
            item = GameObject.Instantiate(Resources.Load<GameObject>("drop/" + data.drop2Id));
            item.name = data.drop2Id;
            item.transform.position = monsPos;
        }
         if(rate < data.drop3Rate){
            dropData.id = data.drop3Id;
            //dropData.num = Random.Range(data.drop1MinNum,data.drop1MaxNum);//当需要掉落多个drop1的时候并将掉落位置散落开
            item = GameObject.Instantiate(Resources.Load<GameObject>("drop/" + data.drop3Id));
            item.name = data.drop3Id;
            item.transform.position = monsPos;
        }

        ExpController.Instance.AddExp(data.exp);
    }
}
