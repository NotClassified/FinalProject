using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    [SerializeField] float buttonHeldDuration;
    float buttonHeldTimer;

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
        currentItem = itemObj.GetComponent<Item>();
        print("picked up: " + currentItem.GetType());
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && currentItem != null)
        {
            buttonHeldTimer += Time.deltaTime;
            if (buttonHeldTimer >= buttonHeldDuration)
            {
                currentItem.Use();
                //reset
                currentItem = null;
                buttonHeldTimer = 0;
            }
        }
        else
        {
            buttonHeldTimer = 0;
        }
    }
}
