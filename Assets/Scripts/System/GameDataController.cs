using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataController : MonoBehaviour
{
    public class ItemData{
        public string id;
        public int num;
        public ItemData(string iid,int inum){
            id = iid;
            num = inum;
        }
    }
    public static string nowMap;
    //多个道具的使用
    public static List<ItemData> itemList = new List<ItemData>(30);//此处参数为道具背包的最大容量
    public static string weaponId = "";//装备武器id

    //参数为拾取的我物体id 和 数量
    public static void AddItem(string itemName,int num){
        bool found = false;
        //在道具列表中查看是否已经拥有该道具
        for(int i = 0;i < itemList.Count;i++){
            //当已有道具时  只增加数量
            if(itemList[i].id == itemName){
                itemList[i].num++;
                found = true;
                break;
            }
        }
        if(!found){
            //在道具栏增加新道具
            ItemData itemData = new ItemData(itemName,num);
            itemList.Add(itemData);
        }
    }

    //移除道具
    public static void RemoveItem(string id){
        bool found = false;
        for(int i = 0;i < itemList.Count;i++){
            if(itemList[i].id == id){
                found = true;
                itemList[i].num--;
                if(itemList[i].num <= 0){
                    itemList.RemoveAt(i);//将该道具从列表中删除
                }
                break;
            }
        }
        //当未找到要删除道具时
        if(!found){
            Debug.LogError("试图删除一个不存在的道具");
            return;
        }
    }

    public static void ChangeEquip(string equipId){
        //已经有装备时  将当前装备道具转回道具栏
        if(weaponId != ""){
            AddItem(weaponId,1);
        }
        RemoveItem(equipId);//将道具栏的要装备道具移除
        weaponId = equipId;
    }
}
