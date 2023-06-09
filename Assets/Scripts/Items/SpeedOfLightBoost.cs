using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SpeedOfLightBoost : Item
{
    [SerializeField] ContactFilter2D filter;
    [SerializeField] float boostDelay;
    [Header("Target Position Offset")]
    [SerializeField] float minOffsetOfTargetPos;
    [SerializeField] float maxOffsetOfTargetPos;
    float offsetOfTargetPos;
    [SerializeField] float maxDistance;

    [Header("Line Renderer")]
    [SerializeField] float playerLineStartOffset;
    [SerializeField] float depthLayer;
    LineRenderer lineRend;

    private void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
        lineRend.enabled = false;
    }

    public override void Use(GameObject player)
    {
        base.Use(player);
        StartCoroutine(AimTargetPosition(player));
    }

    IEnumerator AimTargetPosition(GameObject player)
    {
        RaycastHit2D[] results = new RaycastHit2D[1];
        Vector3 lineStart;
        Vector3 lineEnd = player.transform.position;

        lineRend.enabled = true;
        float delayTimer = 0;
        while (delayTimer < boostDelay)
        {
            Physics2D.Raycast(player.transform.position, player.transform.up, filter, results);

            lineStart = player.transform.position + (player.transform.up * playerLineStartOffset);

            float distanceToTargetPoint = Vector2.Distance(lineStart, results[0].point);
            offsetOfTargetPos = Mathf.Lerp(minOffsetOfTargetPos, maxOffsetOfTargetPos, distanceToTargetPoint / maxDistance);
            lineEnd = results[0].point + offsetOfTargetPos * -(Vector2)player.transform.up;
            
            lineStart.z = depthLayer;
            lineEnd.z = depthLayer;

            lineRend.SetPosition(0, lineStart);
            lineRend.SetPosition(1, lineEnd);

            delayTimer += Time.deltaTime;
            yield return null;
        }
        lineRend.enabled = false;


        player.transform.position = lineEnd;
    }
}
