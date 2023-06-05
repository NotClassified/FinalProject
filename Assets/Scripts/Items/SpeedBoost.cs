using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : Item
{
    [SerializeField] float extraAcceleration;
    [SerializeField] float duration;

    public override void Use(GameObject player) => StartCoroutine(BoostRoutine(player));

    IEnumerator BoostRoutine(GameObject player)
    {
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        movement.AddAcceleration(extraAcceleration);
        print("boost activated");
        yield return new WaitForSeconds(duration);
        print("boost deactivated");
        movement.ResetAcceleration();
    }
}
