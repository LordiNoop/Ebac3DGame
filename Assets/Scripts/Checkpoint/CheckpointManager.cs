using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;

public class CheckpointManager : Singleton<CheckpointManager>
{
    public int lastCheckPointKey = 0;

    public List<CheckpointBase> checkpoints;

    public bool HasCheckpoint()
    {
        return lastCheckPointKey > 0;
    }

    public void SaveCheckpoint(int i)
    {
        if (i > lastCheckPointKey)
        {
            lastCheckPointKey = i;
        }
    }

    public Vector3 GetPositionFromLastCheckPoint()
    {
        var checkpoint = checkpoints.Find(i => i.key == lastCheckPointKey);
        var checkpointPos = checkpoint.transform.position + Vector3.back * 4;
        return checkpointPos;
    }
}
