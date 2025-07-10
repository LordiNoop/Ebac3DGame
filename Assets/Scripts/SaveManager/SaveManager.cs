using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;
using Itens;
using Cloth;

public class SaveManager : Singleton<SaveManager>
{
    [SerializeField] private SaveSetup _saveSetup;

    private string _path = Application.streamingAssetsPath + "/save.txt";

    public int lastLevel;

    public Action<SaveSetup> FileLoaded;

    public HealthBase healthBase;

    public Vector3 spawnPlayerPosition = new Vector3(423f, -6.8f, 39f);

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Invoke(nameof(Load), .1f);
    }

    public SaveSetup Setup
    {
        get {  return _saveSetup; }
    }

    private void CreateNewSave()
    {
        _saveSetup = new SaveSetup();
        _saveSetup.lastLevel = 0;
        _saveSetup.playerName = "Cesar";
        _saveSetup.playerPosition = spawnPlayerPosition;
        _saveSetup.life = 25;
    }

    #region SAVE
    [NaughtyAttributes.Button]
    private void Save()
    {
        string setupToJson = JsonUtility.ToJson(_saveSetup, true);
        Debug.Log(setupToJson);
        SaveFile(setupToJson);
    }

    public void SaveName(string text)
    {
        _saveSetup.playerName = text;
        Save();
    }

    public void SaveLastLevel(int level)
    {
        _saveSetup.lastLevel = level;
        _saveSetup.playerName = "Cesar";
        SaveItems();
        SaveStats();
        Save();
    }

    public void SaveLastCheckpoint()
    {
        _saveSetup.playerName = "Cesar";
        SaveItems();
        SaveStats();
        SavePosition();
        Save();
    }

    public void SaveItems()
    {
        _saveSetup.coins = Itens.ItemManager.Instance.GetItemByType(Itens.ItemType.COIN).soInt.value;
        _saveSetup.health = Itens.ItemManager.Instance.GetItemByType(Itens.ItemType.LIFE_PACK).soInt.value;
        Save();
    }

    public void SavePosition()
    {
        _saveSetup.playerPosition = CheckpointManager.Instance.GetPositionFromLastCheckPoint();
        Save();
    }

    public void SaveStats()
    {
        _saveSetup.life = healthBase.Life;
        _saveSetup.cloth = Cloth.ClothManager.Instance.GetSetupByType(ClothType.DEFAULT);
        Save();
    }
    #endregion
    private void SaveFile(string json)
    {
        Debug.Log(_path);
        File.WriteAllText(_path, json);
    }

    [NaughtyAttributes.Button]
    private void Load()
    {
        string fileLoaded = "";

        if (File.Exists(_path))
        {
            fileLoaded = File.ReadAllText(_path);
            _saveSetup = JsonUtility.FromJson<SaveSetup>(fileLoaded);
            lastLevel = _saveSetup.lastLevel;
            spawnPlayerPosition = _saveSetup.playerPosition;
        }
        else
        {
            CreateNewSave();
            Save();
        }


        FileLoaded.Invoke(_saveSetup);
    }

    [NaughtyAttributes.Button]
    private void SaveLevelOne()
    {
        SaveLastLevel(1);
    }
    
    [NaughtyAttributes.Button]
    private void SaveLevelFive()
    {
        SaveLastLevel(5);
    }
}

[System.Serializable]
public class SaveSetup
{
    public int lastLevel;
    public float coins;
    public float health;
    public float life;
    public ClothSetup cloth;
    public Vector3 playerPosition;

    public string playerName;
}
