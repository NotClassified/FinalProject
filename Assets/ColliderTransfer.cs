using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class ColliderTransfer : MonoBehaviour
{
    //BoxCollider2D[] colliders;
    PolygonCollider2D colliderInstance;

    private void Start()
    {
        //colliders = transform.GetChild(0).GetComponents<BoxCollider2D>();

        //for (int i = 1; i < transform.childCount; i++)
        //{
        //    var nextChild = transform.GetChild(i).gameObject;

        //    var collider0 = nextChild.AddComponent<BoxCollider2D>();
        //    collider0.size = colliders[0].size;
        //    collider0.offset = colliders[0].offset;

        //    var collider1 = nextChild.AddComponent<BoxCollider2D>();
        //    collider1.size = colliders[1].size;
        //    collider1.offset = colliders[1].offset;
        //}


    }
}
