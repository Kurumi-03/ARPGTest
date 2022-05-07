using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpController : MonoBehaviour
{
    //较完整的单例模式
    static ExpController instance;
    public static ExpController Instance{
        set{
            instance = value;
        }
        get{
            if(instance == null){
                instance = FindObjectOfType<ExpController>();
            }
            return instance;
        }
    }

    public int level = 1;//初始等级
    public int exp;//初始经验值

    public void AddExp(int exp){
        StartCoroutine(IaddExp(exp));
    }

    public IEnumerator IaddExp(int aExp){
        exp += aExp;
        //检测是否升级  即当前经验值是否大于下一级所需经验
        if(exp > ExpConfig.GetExpInfo(level + 1).targetExp){
            exp -= ExpConfig.GetExpInfo(level + 1).targetExp;//将经验值减去
            level++;
            yield return new WaitForSeconds(1);//1s后执行后续
            //可在此处执行升级的特效显示
            //递归调用  防止一次性获取的经验值超过升一级所需经验  能够连续升级
            StartCoroutine(IaddExp(0));
        }
        else{
            //未升级情况
        }
        Debug.Log("Level:" + level);
        Debug.Log("exp:" + exp);
    }
}
