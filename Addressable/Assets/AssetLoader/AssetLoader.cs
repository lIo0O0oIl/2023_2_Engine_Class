using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class AssetLoader : MonoBehaviour
{
    [SerializeField] private AssetLoaderSO _assetList;

    public AssetLoaderSO Assets => _assetList;
    public delegate void InvokeMessage(string msg);
    public delegate void Notify();

    public static event InvokeMessage OnCategoryMessage;
    public static event InvokeMessage OnDescMessage;
    public static event Notify OnLoadComplete;

    //public static event Action<string> OnCategoryMessage;     // �������� ���ٰ�....������..?

    private int _loadedCount = 0;

    public static AssetLoader Instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;

    }

    private async void Start()
    {
        var totalCount = _assetList.TotalCount;
        OnCategoryMessage?.Invoke($"Loading {totalCount} Assets");
        await LoadAsset();

        PoolManager.Instance = new PoolManager(transform);

        OnLoadComplete?.Invoke();       // �ε��Ϸ� �κ�ũ
    }

    private async Task LoadAsset()
    {
        //await Task.WhenAll(_assetList.loadingList.Select(x => x.LoadAssetAsync<GameObject>().Task));          // �Ʒ����� �̰�. �̰� �ξ� ȿ������

        foreach (var r in _assetList.loadingList)
        {
            var asset = await r.LoadAssetAsync<GameObject>().Task;
            OnDescMessage?.Invoke($"loading.. {asset.name}");
            _assetList.LoadingComplete(r, asset.name);
        }

        foreach (var r in _assetList.poolingList)
        {
            var asset = await r.assetRef.LoadAssetAsync<GameObject>().Task;
            OnDescMessage?.Invoke($"loading.. {asset.name}");
            _assetList.LoadingComplete(r.assetRef, asset.name);
        }
    }

    public async Task MakePooling()
    {
        foreach(var r in _assetList.poolingList)
        {
            var prefab = (r.assetRef.Asset as GameObject).GetComponent<PoolableMono>();
            if (prefab == null)
            {
                Debug.LogWarning($"{r.assetRef.Asset.name} �� Ǯ���������� �ƴմϴ�.");
                continue;
            }

            OnDescMessage?.Invoke($"loading.. {r.assetRef.Asset.name}");
            await Task.Delay(1);        // UI �ݿ� �ð�
            PoolManager.Instance.CreatePool(r.assetRef.AssetGUID, prefab, r.count);
        }
    }
}
