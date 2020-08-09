using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseManager : BaseBeheviour
{
    public bool isOnUpdate = false;

    public abstract void Initialize();

    public abstract void OnUpdate(float deltaTime);

    public abstract void OnDispose();
}
