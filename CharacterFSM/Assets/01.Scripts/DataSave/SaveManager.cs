using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoSingleton<SaveManager>
{
    [SerializeField] private string filename;
    private GameData _gameData;
    private List<ISaveManager> _saveManagerList;
    private FileDataHandler _fileDataHandler;

    [SerializeField] private bool _isEncrypt;

   // [SerializeField] private SerializableDictionary<string, int> testDictionary;

    private void Start()
    {
        _fileDataHandler = new FileDataHandler(Application.persistentDataPath, filename, _isEncrypt);
        _saveManagerList = FindAllSaveManagers();

        LoadGame();

        //testDictionary = new SerializableDictionary<string, int>();
    }

    public void NewGame()
    {
        _gameData = new GameData();
    }

    public void LoadGame()
    {
        _gameData = _fileDataHandler.Load();
        if (_gameData == null)
        {
            Debug.Log("no save data found");
            NewGame();
        }

        foreach (var saveManager in _saveManagerList)
        {
            saveManager.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        foreach (var saveManager in _saveManagerList)
        {
            saveManager.SaveData(ref _gameData);
        }

        _fileDataHandler.Save(_gameData);
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        return FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>().ToList();      // true 는 꺼진 오브젝트도 가져올거냐 하는 것.
    }

    [ContextMenu("Delete save file")]
    public void DeleteSaveData()
    {
        // 나중에 뭐 해봐
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
