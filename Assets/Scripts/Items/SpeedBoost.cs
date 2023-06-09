using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : Item
{
    [SerializeField] float extraAcceleration;
    [SerializeField] float duration;

    public override void Use(GameObject player)
    {
        base.Use(player);
        StartCoroutine(BoostRoutine(player));
    }

    IEnumerator BoostRoutine(GameObject player)
    {
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        movement.AddAcceleration(extraAcceleration);
        yield return new WaitForSeconds(duration);
        movement.ResetAcceleration();
    }
}
