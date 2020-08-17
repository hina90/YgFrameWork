using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData
{
    public NPCData(int npcid)
    {
        this.npcid = npcid;
    }
    public int npcid = 0;
    public uint roleid = 0;
    public uint index = 0;
    public Vector3 position;
    public ushort level = 0;            //等级
    public NpcFSM fsm = null;          //角色状态机

    public override string ToString()
    {
        return string.Format("npcid:{0}  fsm:{1}", npcid, fsm);
    }
}
