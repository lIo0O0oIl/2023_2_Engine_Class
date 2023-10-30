using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public enum AssetName
{
    LineTrail = 0,
    AudioEffect = 1,
}

[Serializable]
public struct PoolingItem
{
    public AssetReference assetRef;
    public ushort count;        // 몇개 만들거냐
}

[CreateAssetMenu(menuName ="SO/AddressableAssets")]
public class AssetLoaderSO : ScriptableObject
{
    public int TotalCount => loadingList.Count + poolingList.Count;

    public List<AssetReference> loadingList;        // 오브젝트 풀링 없이 로딩만 하면 되는 애들
    public List<PoolingItem> poolingList;           // 오브젝트 풀링을 해야하는 애들

    private Dictionary<string, AssetReference> _nameDictrionary;
    private Dictionary<string, AssetReference> _guidDictionary;

    private void OnEnable()
    {
        _nameDictrionary = new();
        _guidDictionary = new();
    }

    // 로딩이 완료되면 딕셔너리에 넣어준다. 언제든 다시 꺼내 쓸 수 있게 
    public void LoadingComplete(AssetReference reference, string name)
    {
        _guidDictionary.Add(reference.AssetGUID, reference);
        _nameDictrionary.Add(name, reference);
    }

    public UnityEngine.Object GetAssetByGUID(string guid)
    {
        if (_guidDictionary.TryGetValue(guid, out AssetReference value))
        {
            return value.Asset;
        }
        return null;
    }

    public UnityEngine.Object GetAssetByName(string name)
    {
        if(_nameDictrionary.TryGetValue(name, out AssetReference value))
        {
            return value.Asset;
        }
        return null;
    }

}
