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
    public ushort count;        // � ����ų�
}

[CreateAssetMenu(menuName ="SO/AddressableAssets")]
public class AssetLoaderSO : ScriptableObject
{
    public int TotalCount => loadingList.Count + poolingList.Count;

    public List<AssetReference> loadingList;        // ������Ʈ Ǯ�� ���� �ε��� �ϸ� �Ǵ� �ֵ�
    public List<PoolingItem> poolingList;           // ������Ʈ Ǯ���� �ؾ��ϴ� �ֵ�

    private Dictionary<string, AssetReference> _nameDictrionary;
    private Dictionary<string, AssetReference> _guidDictionary;

    private void OnEnable()
    {
        _nameDictrionary = new();
        _guidDictionary = new();
    }

    // �ε��� �Ϸ�Ǹ� ��ųʸ��� �־��ش�. ������ �ٽ� ���� �� �� �ְ� 
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
