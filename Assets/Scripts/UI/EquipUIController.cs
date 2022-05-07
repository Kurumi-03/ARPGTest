using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipUIController : MonoBehaviour
{
    public Transform uiItemList;//道具列表
    public GameObject equipInfo;//装备信息
    public GameObject equipCell;//装备格
    string selectedItemID;//已被装备的道具

    //在每次唤醒装备栏时都更新一次道具显示
    public void OnEnable() {
        RefreshItem();
    }

    public void OnDisable() {
        
    }

    //点击道具后  显示装备名和信息菜单
    public void ClickItem(string name){
        selectedItemID = name;
        equipInfo.transform.Find("name").GetComponent<Text>().text = name;//将显示的装备
        //此处可加描述信息
        equipInfo.SetActive(true);
    }

    //关闭装备面板
    public void CancelEquip(){
        selectedItemID = "";
        equipInfo.SetActive(false);
    }

    //装备道具
    public void WearEquip(){
        GameDataController.ChangeEquip(selectedItemID);
        equipCell.transform.GetChild(0).GetComponent<Text>().text = selectedItemID;
        selectedItemID = "";
        equipInfo.SetActive(false);
        RefreshItem();//装备后刷新
    }

    //刷新显示
    public void RefreshItem(){
        selectedItemID = "";//重置已经选择道具
        for(int i = 0;i < uiItemList.childCount;i++){
            //此时代表i号格子有道具
            if(i < GameDataController.itemList.Count){
                uiItemList.GetChild(i).GetChild(0).GetComponent<Text>().text = GameDataController.itemList[i].id;
                uiItemList.GetChild(i).GetChild(1).GetComponent<Text>().text = GameDataController.itemList[i].num.ToString();
                uiItemList.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();//避免有其他监听影响
                int index = i;
                uiItemList.GetChild(i).GetComponent<Button>().onClick.AddListener(
                    delegate(){
                        ClickItem(GameDataController.itemList[index].id);
                    }
                ); 
            }
            else{
                uiItemList.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                uiItemList.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
                uiItemList.GetChild(i).GetChild(1).GetComponent<Text>().text = "";
            }
        }
    }
}
