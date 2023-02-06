using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class PoolManager : MonoBehaviour
{
    public List<GameObject> poolPrefebs;
    private List<ObjectPool<GameObject>> poolEffectList=new List<ObjectPool<GameObject>>();

    private void OnEnable()
    {
       
        EventHandler.ParticalEffectEvent += ParticalEffectEvent;
    }

    private void OnDisable()
    {
        EventHandler.ParticalEffectEvent -= ParticalEffectEvent;
    }

    private void Start()
    { 
        CreatePool();
    }

    private void ParticalEffectEvent(ParticaleEffectType particaleEffectType, Vector3 pos)
    {
        //TODO:根据特效补全
      var objectpool=particaleEffectType switch
      {
          ParticaleEffectType.LeavesFalling1=>poolEffectList[0],
          ParticaleEffectType.LeavesFalling2=>poolEffectList[1],
          ParticaleEffectType.Rock=>poolEffectList[2],
          ParticaleEffectType.ReapableScenery=>poolEffectList[3],
          _=>null
      };
      var obj = objectpool.Get();
      obj.transform.position = pos;
      StartCoroutine(ReleaseRoutine(objectpool, obj));
    }
    IEnumerator ReleaseRoutine(ObjectPool<GameObject> pool, GameObject obj)
    {
        yield return new WaitForSeconds(1.5f);
        pool.Release(obj);
    }

    /// <summary>
    /// 生成对象池
    /// </summary>
    private void CreatePool()
    {
        foreach (var item in poolPrefebs)
        {
            var parent = new GameObject(item.name).transform;
            parent.SetParent(transform);
            var newPool = new ObjectPool<GameObject>(
                (() => Instantiate(item, parent)),
                e => e.SetActive(true),
                e => e.SetActive(false),
                e=>Destroy(e)
            );
            poolEffectList.Add(newPool);
        }
    }
}
