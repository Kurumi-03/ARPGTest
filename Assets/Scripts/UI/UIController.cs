using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    static UIController instance;
    public static UIController Instancee{
        set{
            instance = value;
        }
        get{
            if(instance == null){
                instance = FindObjectOfType<UIController>();
            }
            return instance;
        }
    }

    public Dictionary<string,GameObject> module = new Dictionary<string, GameObject>();
    public GameObject lastOpen;//记录上一个打开的ui界面

    private void Awake() {
        //查找UI界面的所有按钮
        Transform canvas = GameObject.Find("Canvas").transform;
        foreach(Transform tr in canvas){
            module.Add(tr.name,tr.gameObject);
        }
    }

    public void OpenUI(string uiName){
        //当打开的UI是现在已经打开的UI时 锁定鼠标  使当前UI关闭
        if(lastOpen != null && lastOpen.name == uiName && lastOpen.activeSelf){
            //鼠标不可以且不可见
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            module[uiName].SetActive(false);
            lastOpen = null;
        }
        else{
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //当上一个打开的ui不是即将打开的ui时  将上一个UI关闭
            if(lastOpen != null) lastOpen.SetActive(false);
            module[uiName].SetActive(true);
            lastOpen = module[uiName];//将现在打开的ui设为上一个打开
        }
    }
}
