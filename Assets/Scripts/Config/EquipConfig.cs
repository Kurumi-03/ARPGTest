using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipConfig : MonoBehaviour
{
    //掉落物信息
    public struct DropInfo{
        public float atk;//掉落物攻击力
    }
    //获取掉落物信息
    public static DropInfo getDropInfo(int dropId){
        DropInfo info = new DropInfo();
        return info;
    }
}
