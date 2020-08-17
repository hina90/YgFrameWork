using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectView
{
    void OnAwake();
    void OnUpdate();
    void OnDispose();
}
