using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//玩家和怪物公共使用的属性类
public class Attribute : MonoBehaviour
{
    public float hp;//生命值
    public string cName;//名字
    public float atk;//攻击力
    public float def;//防御力
    protected float hpMax = 10;
    protected float moveSpeed = 1;//移动速度
    protected float atkWaitTime = 0.3f;//硬直时间
    private int txDeath;//死亡特效序号

}
