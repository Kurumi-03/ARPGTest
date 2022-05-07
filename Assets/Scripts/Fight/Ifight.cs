using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用来规范不同物体的状态结算
public interface Ifight
{
    void BeHit(Global.FightInfo fightInfo);
}
