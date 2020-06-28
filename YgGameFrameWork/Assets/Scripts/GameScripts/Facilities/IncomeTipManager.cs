using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeTipManager : Singleton<IncomeTipManager>
{
    private Transform tipContainer;
    private readonly string incomeTipPrefab = "IncomeTip";
    private GameObjectPool incomeTipPool;

    public IncomeTipManager()
    {
        tipContainer = CSceneManager.Instance.GetSceneObjByName("TipsContainer").transform;
        incomeTipPool = new GameObjectPool(tipContainer, incomeTipPrefab);
    }

    public void ShowIncomeTip(int value, Vector3 pos)
    {
        GameObject tipObj = incomeTipPool.GetPool();
        tipObj.GetOrAddComponent<IncomeTipItem>().SetValue(value, pos);
    }

    public void Recycle(GameObject obj)
    {
        incomeTipPool.Recycle(obj);
    }
}
