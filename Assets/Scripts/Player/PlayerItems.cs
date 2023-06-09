using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItems : MonoBehaviour, IContainsInput
{
    public static event System.Action<Item> ItemChange;
    public static event System.Action<Item> ItemUsed;

    private Item _currentItem;
    private Item CurrentItem
    {
        get => _currentItem;
        set
        {
            _currentItem = value;

            if (ItemChange != null)
                ItemChange(_currentItem);
        }
    }

    private void OnEnable()
    {
        PlayerCollision.ItemPickedup += PickupItem;
    }
    private void OnDisable()
    {
        PlayerCollision.ItemPickedup -= PickupItem;

    }

    void PickupItem(GameObject itemObj)
    {
        if (CurrentItem == null)
        {
            CurrentItem = ItemManager.instance.GetRandomItem();
        }
    }

    public void UseItem(InputAction.CallbackContext context)
    {
        if (context.performed && CurrentItem != null)
        {
            CurrentItem.Use(gameObject);

            ItemUsed(CurrentItem); //notify
            CurrentItem = null; //unequip item
        }
    }
}
