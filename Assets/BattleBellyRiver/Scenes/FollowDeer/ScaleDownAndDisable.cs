using System.Collections;
using UnityEngine;

public class ScaleDownAndDisable : MonoBehaviour
{
    public float SecondsToScaleDown = 5f;  
    private bool isScaling = false;
/*
    private void Start()
    {
        //StartScaling();
    }*/

    IEnumerator ScaleDownCoroutine()
    {
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;

        float elapsedTime = 0f;

        while (elapsedTime < SecondsToScaleDown)
        {
            // Gradually interpolate the scale towards the targetScale
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / SecondsToScaleDown);

            yield return new WaitForSeconds(Time.deltaTime);

            elapsedTime += Time.deltaTime;
        }

        // Ensure the final scale is exactly Vector3.zero
        transform.localScale = targetScale;

        // Disable the GameObject when the scale is Vector3.zero
        gameObject.SetActive(false);
        isScaling = false;  
    }

    public void StartScaling()
    {
        if (!isScaling)
        {
            StartCoroutine(ScaleDownCoroutine());
            isScaling = true;
        }
    }
}

