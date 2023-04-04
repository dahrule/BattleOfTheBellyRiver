using UnityEngine;
using System.Collections;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] Transform Player;

    private void Awake()
    {
        if(Player==null) Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        if (Player != null)
        {
            StartCoroutine(UpdateRoutine());
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
