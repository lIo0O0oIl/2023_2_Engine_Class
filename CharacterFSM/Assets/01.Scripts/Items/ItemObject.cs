using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private ItemDataSO _itemData;

#if UNITY_EDITOR
    // 편의성을 위한 기능
    private void OnValidate()
    {
        if (_itemData == null) return;
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _spriteRenderer.sprite = _itemData.itemIcon;
        gameObject.name = $"ItemObject[{_itemData.itemName}]";
    }
#endif

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 아이템 드랍용
    public void SetUpItem(ItemDataSO itemData, Vector2 velocity)
    {
        _itemData = itemData;
        _rigidbody.velocity = velocity;
        _spriteRenderer.sprite = itemData.itemIcon;
    }

    public void PickUpItem()
    {
        // 여기에 인벤토리에 더하는 부분이 들어가야 한다.
        Inventory.Instance.AddItem(_itemData);
        Destroy(gameObject);
    }
}
