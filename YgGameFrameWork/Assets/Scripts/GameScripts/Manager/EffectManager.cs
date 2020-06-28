using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Spine.Unity;


public class EffectManager : UnitySingleton<EffectManager>
{
    const int POOL_LIMITED = 0;                                                                                 //缓存池大小

    private int id = 0;                                                                                         //特效ID编号
    private Dictionary<string, List<GameObject>> effectPoolDic = new Dictionary<string, List<GameObject>>();    //特效池    


    /// <summary>
    /// 特殊镜头展示特效
    /// </summary>
    private bool exhibition = false;
    private Queue<GameObject> exhibition_queue = new Queue<GameObject>();
    public bool Exhibition
    {
        get { return exhibition; }
        set
        {
            exhibition = value;
            if (!value)
            {
                GameObject tgo;
                while (exhibition_queue.Count > 0)
                {
                    tgo = exhibition_queue.Dequeue();
                    if (tgo != null)
                    {
                        //设置层级
                        Utils.ChangeLayer(tgo, Layers.EFFECT, true);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 创建特效
    /// </summary>
    /// <param name="evo"></param>
    /// <returns></returns>
    public BaseEffect Create(string sourceName, string aniName, bool loop, 
        //Vector3 startVector, Vector3 endVector, 
        Transform startTrans, string startAttachName,
        Transform endTrans, string endAttachName, 
        Transform target, float speed, 
        float lifeTime, float scale)
    {
        EffectVO evo = new EffectVO();
        evo.SourceName = sourceName;
        evo.AniName = aniName;
        evo.Loop = loop;
        //if (startVector != Vector3.zero)
        //{
        //    evo.isZero = false;
        //    evo.StartVector = startVector;
        //}
        //else
        //    evo.isZero = true;

        //if (endVector != Vector3.zero)
        //{
        //    evo.EndVector = endVector;
        //}

        //if (startTrans != null)
        //{
        //    evo.StartTrans = startTrans;
        //    evo.StartTranName = startAttachName;

        //    Spine.Bone bone = startTrans.GetSkeletonAnimation().skeleton.FindBone(startAttachName);
        //    evo.StartVector = bone.GetWorldPosition(startTrans.transform);
        //}

        //if (endTrans != null)
        //{
        //    evo.EndTrans = endTrans;
        //    evo.EndTranName = endAttachName;
        //    Spine.Bone bone = endTrans.GetSkeletonAnimation().skeleton.FindBone(endAttachName);
        //    evo.EndVector = bone.GetWorldPosition(endTrans.transform);
        //    if (evo.EndVector == null)
        //        Debug.LogWarning ("  -------------找不到挂点------------- ");

        //}
        evo.Speed = speed;
        evo.LifeTime = lifeTime;
        evo.Scale = scale;

        GameObject eftGo = new GameObject("tmp_effect");
        string resName = evo.SourceName;

        if(!GetInActive(resName, eftGo))
        {
            GameObject effectObject = ResourceManager.Instance.GetResourceInstantiate(resName, eftGo.transform, ResouceType.Effect);
            effectObject.name = resName;
            effectObject.tag = Tags.EFFECT;
            AddScript(evo.EffectType, effectObject);
        }

        eftGo.name = new StringBuilder(resName).Append("_").Append(id++).ToString();
        eftGo.transform.SetParent(transform, false);
        if (!effectPoolDic.ContainsKey(resName))
            effectPoolDic.Add(resName, new List<GameObject>()); 

        effectPoolDic[resName].Add(eftGo);

        Transform tf = eftGo.transform.GetChild(0);
        BaseEffect eb = tf.GetComponent<BaseEffect>(); 
        eb.Stop();
        eb.EffectInfo = evo;


        //如果是特殊镜头展示特效
        if (Exhibition)
        {
            exhibition_queue.Enqueue(eb.gameObject);
            //展示用层级
            Utils.ChangeLayer(eb.gameObject, Layers.EFFECT_EXHIBITION, true);
        }
        else
        {
            //设置层级
            //eb.SetSortLayer(startTrans.GetSortLayer() + 1, SpineActor.LayerSetType.EQUAL);
            //Utils.ChangeLayer(eb.gameObject, Layers.EFFECT, true);
        }

        return eb;
    }


    /// <summary>
    /// 添加特效脚本
    /// </summary>
    /// <param name="type"></param>
    /// <param name="go"></param>
    /// <returns></returns>
    private BaseEffect AddScript(int type, GameObject go)
    {
        switch (type)
        {
            case 1:
                return go.AddComponent<BaseEffect>();
            default:
                return go.AddComponent<BaseEffect>();
        }
    }

    /// <summary>
    /// 特效池里查找缓存对象
    /// </summary>
    /// <param name="key"></param>
    /// <param name="ngo"></param>
    /// <returns></returns>
    private bool GetInActive(string key, GameObject ngo)
    {
        if (!effectPoolDic.ContainsKey(key))
        {
            effectPoolDic.Add(key, new List<GameObject>());
            return false;
        }
        GameObject tmp_go = ResourceManager.Instance.GetResource(key, ResouceType.Effect);

        List<GameObject> glst = effectPoolDic[key];
        if (tmp_go == null)
        {
            for (int i = 0; i < glst.Count; i++)
            {
                GameObject.Destroy(glst[i]);
            }
            glst.Clear();
            return false;
        }
        GameObject fgo;
        for (int i = 0; i < glst.Count; i++)
        {
            fgo = glst[i];
            if (fgo != null && !fgo.activeSelf)
            {
                if (fgo.transform.childCount > 0)
                {
                    fgo.transform.GetChild(0).SetParent(ngo.transform, false);
                    glst.RemoveAt(i);
                    UnityEngine.Object.Destroy(fgo);
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 删除特效
    /// </summary>
    /// <param name="effect">特效的GameObject</param>
    public bool RemoveEffect(GameObject effect)
    {
        if (effect == null) return false;
        if (effect.transform.childCount == 0) return false;
        Transform sub_tf = effect.transform.GetChild(0);

        string key = sub_tf.name;
        if (!effectPoolDic.ContainsKey(key))
        {
            GameObject.Destroy(effect);
            return false;
        }

        if (effectPoolDic[key].Count > POOL_LIMITED)
        {
            effectPoolDic[key].Remove(effect);
            UnityEngine.Object.Destroy(effect);
            return true;
        }

        effect.SetActive(false);
        return true;
    }
}
