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

    //public static event Action<string> OnCategoryMessage;     // 위에꺼가 좋다고....가독성..?

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

        OnLoadComplete?.Invoke();       // 로딩완료 인보크
    }

    private async Task LoadAsset()
    {
        //await Task.WhenAll(_assetList.loadingList.Select(x => x.LoadAssetAsync<GameObject>().Task));          // 아래꺼가 이거. 이게 훨씬 효율적임

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
                Debug.LogWarning($"{r.assetRef.Asset.name} 은 풀링아이템이 아닙니다.");
                continue;
            }

            OnDescMessage?.Invoke($"loading.. {r.assetRef.Asset.name}");
            await Task.Delay(1);        // UI 반영 시간
            PoolManager.Instance.CreatePool(r.assetRef.AssetGUID, prefab, r.count);
        }
    }
}
