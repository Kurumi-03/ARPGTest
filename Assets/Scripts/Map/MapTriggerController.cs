using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapTriggerController : MonoBehaviour
{
    public bool isDelete;//判断是否在触发后删除
    public GameObject[] hideObject;//触发后需要隐藏的物体
    public GameObject[] showObject;//触发后需要显示的物体
    public string eventName;
    public string triggerTag;//只针对一种tag进行触发检测

    public static event UnityAction<string> MapEvent; 

    private void OnTriggerEnter(Collider collider) {
        if(collider.tag == triggerTag){
            //当有要发送的事件时  发送事件
            if(eventName != "" && MapEvent != null){
                MapEvent(eventName);
            }
            //将需要隐藏的物体隐藏
            for(int i = 0;i < hideObject.Length;i++){
                hideObject[i].SetActive(false);
            }
            //显示物体
            for(int i = 0;i < showObject.Length;i++){
                showObject[i].SetActive(true);
            }
            //在触发后删除需要删除的物体
            if(isDelete){
                Destroy(gameObject);
            }
        }
    }
}
