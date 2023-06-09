using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedOfLightBoost : Item
{
    [SerializeField] ContactFilter2D filter;
    [SerializeField] float minOffsetOfTargetPos;
    [SerializeField] float maxOffsetOfTargetPos;
    float offsetOfTargetPos;
    [SerializeField] float maxVelocity;

    public override void Use(GameObject player)
    {
        base.Use(player);

        RaycastHit2D[] results = new RaycastHit2D[1];
        Physics2D.Raycast(player.transform.position, player.transform.up, filter, results);


        float playerVelocity = player.GetComponent<Rigidbody2D>().velocity.magnitude;
        offsetOfTargetPos = Mathf.Lerp(minOffsetOfTargetPos, maxOffsetOfTargetPos, playerVelocity / maxVelocity);

        player.transform.position = results[0].point + offsetOfTargetPos * -(Vector2)player.transform.up;
    }
}
