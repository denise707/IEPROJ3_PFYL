using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    public static SaveData data;
    public static bool isLoad = false;

    private void Awake()
    {
        data = new SaveData();
        SaveLoad.OnLoadGame += LoadData;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void DeleteData()
    {
        SaveLoad.DeleteSaveData();
    }

    public static void SaveData()
    {
        var saveData = data;

        SaveLoad.Save(saveData);
    }

    private void LoadData(SaveData _data)
    {
        data = _data;
    }

    public static void TryLoadData()
    {
        SaveLoad.Load();
    }

    public static void SetIsLoad(bool _isLoad)
    {
        isLoad = _isLoad;
    }
}
