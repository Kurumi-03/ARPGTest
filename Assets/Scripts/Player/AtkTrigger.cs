using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//挂载在武器上的脚本
public class AtkTrigger : MonoBehaviour
{
    public enum RoleType{
        Player,
        Monster
    }

    private string skillName;
    public string SKillName{
        set{
            skillName = value;
        }
    }

    public RoleType roleType;
    public Attribute attribute;//玩家属性

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Monster"){
            Debug.Log("攻击到怪物");
            //将玩家属性和怪物属性传入
            FightManager.skillLogic(skillName,attribute,other.gameObject.GetComponent<Attribute>());
        }
    }
}
