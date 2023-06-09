using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItems : MonoBehaviour
{
    private Item currentItem;

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
        if (currentItem == null)
        {
            currentItem = ItemManager.instance.GetRandomItem();
            print("Picked up " + currentItem.GetType().FullName);
        }
    }

    public void UseItem(InputAction.CallbackContext context)
    {
        if (context.performed && currentItem != null)
        {
            currentItem.Use(gameObject);
            currentItem = null; //unequip item
        }
    }
}
