using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour
{

    public virtual void Use(GameObject player) { }

    private void OnEnable() { } //this is here only to show the checkbox for the items
}
