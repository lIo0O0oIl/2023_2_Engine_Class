using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(SaveManager))]
public class LoadItemDatabaseEditor : Editor
{
    private SaveManager _saveManager;
    private string _soFilename = "ItemDB";

    private void OnEnable()
    {
        _saveManager = (SaveManager)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate item DB"))
        {
            CreateItemDBAsset();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    private void CreateItemDBAsset()
    {
        List<ItemDataSO> loadedList = new List<ItemDataSO>();
        string[] assetIDArray = AssetDatabase.FindAssets("", new[] { "Assets/08.SO/Items" });

        foreach (string assetID in assetIDArray)
        {
            Debug.Log(assetID);
            string assetPath = AssetDatabase.GUIDToAssetPath(assetID);
            ItemDataSO item = AssetDatabase.LoadAssetAtPath<ItemDataSO>(assetPath);

            if (item != null)
            {
                loadedList.Add(item);
            }
        }
            Debug.Log(loadedList.Count);

        string dbPath = $"Assets/08.SO/{_soFilename}.asset";
        ItemDatabaseSO itemDB = AssetDatabase.LoadAssetAtPath<ItemDatabaseSO>(dbPath);

        if (itemDB == null)
        {
            // 새로 SO 만들기
            itemDB = ScriptableObject.CreateInstance<ItemDatabaseSO>();
            itemDB.itemList = loadedList;
            string realPath = AssetDatabase.GenerateUniqueAssetPath(dbPath);        // 절대 경로가져와줌
            AssetDatabase.CreateAsset(itemDB, realPath);
            Debug.Log($"item db created at {dbPath}");
        }
        else
        {
            // 데이터만 고치면 됨.
            itemDB.itemList = loadedList;
            EditorUtility.SetDirty(itemDB);
        }
    }
}
