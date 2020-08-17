using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectView : BaseBeheviour, IObjectView
{
    public GameObject gameObject;
    public ViewObject viewObject;

    public virtual void OnAwake()
    {
    }

    public virtual void OnStart()
    {
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnDispose()
    {
        if (viewObject != null)
        {
            Destroy(viewObject);
        }
    }
}
