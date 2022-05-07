using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCreator : MonoBehaviour
{
    private GameObject[] creator;//刷怪点
    private GameObject[] monsterPrefabs;//怪物的预制体
    private string lastMap = "";

    private void Awake() {
        //初始时并不刷怪只获取到刷怪点后就关闭
        if(creator == null){
            creator = GameObject.FindGameObjectsWithTag("SpawnPoint");//根据tag获取到刷怪点的引用
        }
        gameObject.SetActive(false);
    }

    //被调用时
    private void OnEnable() {
        //若当前地图是上一张地图则不变
        if(lastMap == GameDataController.nowMap){
            return;
        }
        //如果不是  将当前地图赋值为上一张地图
        lastMap = GameDataController.nowMap;
        //获取到怪物的数据
        List<GameDataConfig.CreatMonsterData> mData = GameDataConfig.GetCreatMonster(lastMap);
        //加载怪物
        monsterPrefabs = new GameObject[mData.Count];
        for(int i = 0;i < mData.Count; i++){
            //直接从resources文件夹中加载预制体
            monsterPrefabs[i] = Resources.Load<GameObject>("monster/" + mData[i].monsterName);
           
        }
        StartCoroutine(CreateMonster());
    }

    public IEnumerator CreateMonster(){
        yield return 0;
        for(int i = 0;i < creator.Length;i++){
            yield return 5;//每5帧调用一次
            Instantiate(monsterPrefabs[Random.Range(0,monsterPrefabs.Length)],creator[i].transform.position,Quaternion.identity);
            
        }
    }

    
}
