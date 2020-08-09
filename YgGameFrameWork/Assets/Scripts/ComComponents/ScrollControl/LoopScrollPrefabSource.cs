namespace UnityEngine.UI
{
    [System.Serializable]   //可被序列化
    public class LoopScrollPrefabSource     //Scroll预制体信息
    {
        public string prefabName;           //预制体名字
        public int poolSize = 5;            //缓存池大小
        private ResouceType resouceType = ResouceType.PrefabItem;
        private bool inited = false;
        public virtual GameObject GetObject()
        {
            if (!inited)
            {
                SG.CachePoolManager.Instance.InitPool(prefabName, poolSize, ResouceType.PrefabItem);       //设置缓存池
                inited = true;
            }
            return SG.CachePoolManager.Instance.GetObjectFromPool(prefabName);
        }

        public virtual void ReturnObject(Transform go)
        {
            go.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);         //预制体删除后的回调信息
            SG.CachePoolManager.Instance.ReturnObjectToPool(go.gameObject);                     //将预制体归到池中
        }
    }
}
