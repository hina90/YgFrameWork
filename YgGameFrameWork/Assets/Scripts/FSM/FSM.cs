using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    void Update()
    {
        FSMUpdate();
    }

    void FixedUpdate()
    {
        FSMFixedUpdate();
    }

    public virtual void Initialize(BaseActor actor) { }
    public virtual void FSMUpdate() { }
    public virtual void FSMFixedUpdate() { }
}
