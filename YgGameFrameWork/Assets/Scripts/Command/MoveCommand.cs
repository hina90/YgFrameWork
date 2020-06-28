using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 移动命令
/// </summary>
public class MoveCommand : ICommand
{
    private BaseActor _moveActor;
    private Vector3 _movePosition;

    public MoveCommand(BaseActor actor, Vector3 position)
    {
        _moveActor = actor;
        _movePosition = position;
    }
    /// <summary>
    /// 执行移动命令
    /// </summary>
    public void Execute()
    {
        //_moveActor.Move(_movePosition);
    }
}
