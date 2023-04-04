using System.Collections;
using UnityEngine;
using System;


public class Timer: MonoBehaviour
{

    public Action OnCountUpStop;

    IEnumerator _countdown;

    public Timer(float stopTime, Action OnComplete)
    {
        _countdown = CountDownRoutine(stopTime);
        OnCountUpStop += OnComplete;
    }

    IEnumerator CountDownRoutine(float stopTime)
    {//<summary>
     //This is a timer that ends at stoptime.
        /// </summary>

        float startTime = Time.time;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (Time.time - startTime > stopTime)
                break;
        }
        OnCountUpStop?.Invoke();
    }

    public void StartCountDown()
    {
        StartCoroutine(_countdown);
    }

    public void Stop()
    {
        StopCoroutine(_countdown);
    }
}
