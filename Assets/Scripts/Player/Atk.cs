using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk : MonoBehaviour
{
    public BoxCollider weapon;
    public static bool canAtk;
    AtkTrigger trigger;

    private void Awake() {
        trigger = weapon.GetComponent<AtkTrigger>();
    }
    public void AtkTriggerStart(string name){
        trigger.SKillName = name;
        weapon.enabled = true;
        canAtk = true;
    }
    public void AtkTriggerEnd(){
        trigger.SKillName = "";
        weapon.enabled = false;
        canAtk = false;
    }

    public void tx(GameObject tx){
        var instance = Instantiate(tx,this.transform.position,Quaternion.identity);
        instance.transform.rotation = PlayerController._instance.transform.rotation;//设置特效的朝向为主角当前朝向
        Destroy(instance,3);//3s后销毁
    }
}
