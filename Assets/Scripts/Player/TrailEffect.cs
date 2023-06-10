using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailEffect : MonoBehaviour
{
    TrailRenderer boostTrail;
    [SerializeField] PlayerBoost playerBoost;

    [SerializeField] float lerpDeadzone;

    [SerializeField] float maxAlpha;
    [SerializeField] float alphaLerpTime;
    float currentAlpha;
    float targetAlpha;
    Color trailColor;

    [SerializeField] float minLength;
    [SerializeField] float maxLength;
    [SerializeField] float lengthLerpTime;
    float currentLength;
    float targetLength;

    private void Awake()
    {
        boostTrail = GetComponent<TrailRenderer>();
        trailColor = boostTrail.material.color;
    }

    private void OnEnable()
    {
        playerBoost.BoostInitiated += NewBoost;
        playerBoost.BoostFinished += EndBoost;
    }
    private void OnDisable()
    {
        playerBoost.BoostInitiated -= NewBoost;
        playerBoost.BoostFinished -= EndBoost;
    }

    private void Update()
    {
        if (boostTrail.enabled)
        {
            if (!Approximate(currentAlpha, targetAlpha, lerpDeadzone))
            {
                currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, alphaLerpTime * Time.deltaTime);
            }
            else
                currentAlpha = targetAlpha;

            if (!Approximate(currentLength, targetLength, lerpDeadzone))
            {
                currentLength = Mathf.Lerp(currentLength, targetLength, lengthLerpTime * Time.deltaTime);
            }
            else
                currentLength = targetLength;

            SetAlpha(currentAlpha);
            boostTrail.time = currentLength;

            if (currentAlpha == 0)
            {
                targetLength = 0f;
                currentLength = targetLength;
                boostTrail.time = currentLength;

                boostTrail.enabled = false;
            }
        }
    }

    void SetAlpha(float alpha)
    {
        trailColor.a = alpha;
        boostTrail.material.color = trailColor;
    }

    private void NewBoost(float percentBoost)
    {
        targetLength = Mathf.Lerp(minLength, maxLength, percentBoost);
        targetAlpha = maxAlpha;

        if (!boostTrail.enabled)
        {
            currentLength = targetLength;
            currentAlpha = targetAlpha;
            boostTrail.enabled = true;
        }
    }

    private void EndBoost()
    {
        targetAlpha = 0f;
    }

    static bool Approximate(float a, float b, float distance)
    {
        return a < b + distance && a > b - distance;
    }
}
