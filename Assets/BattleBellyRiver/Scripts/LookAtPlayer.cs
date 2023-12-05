using UnityEngine;
using System.Collections;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] Transform Player;

    private void Awake()
    {
        if(Player==null) Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        if (Player != null)
        {
            StartCoroutine(UpdateRoutine());
        }     
    }

    private void OnDisable()
    {
        if (Player != null)
        {
            StopCoroutine(UpdateRoutine());
        }
    }

    IEnumerator UpdateRoutine()
    {
        WaitForSeconds wait = new(0.2f);
        while (true)
        {
            transform.LookAt(Player);
            yield return wait;
        }  
    }

}
