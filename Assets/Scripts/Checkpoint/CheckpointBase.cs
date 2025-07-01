using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBase : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public int key = 01;

    private bool _checkpointActive = false;
    private string _checkpointKey = "CheckpointKey";

    private void OnTriggerEnter(Collider other)
    {
        if (!_checkpointActive && other.transform.tag == "Player")
        {
            CheckCheckpoint();
        }
    }

    private void Awake()
    {
        TurnItOff();
    }

    private void CheckCheckpoint()
    {
        TurnItOn();
        SaveCheckpoint();
    }

    [NaughtyAttributes.Button]
    private void TurnItOn()
    {
        meshRenderer.material.SetColor("_EmissionColor", Color.white);
    }

    [NaughtyAttributes.Button]
    private void TurnItOff()
    {
        meshRenderer.material.SetColor("_EmissionColor", Color.black);
    }

    private void SaveCheckpoint()
    {
        /*if (PlayerPrefs.GetInt(_checkpointKey, 0) > key)
            PlayerPrefs.SetInt(_checkpointKey, key);*/

        CheckpointManager.Instance.SaveCheckpoint(key);

        _checkpointActive = true;
    }
}
