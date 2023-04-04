using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPositioner : MonoBehaviour
{
    [SerializeField] Transform _referenceObject;
    [SerializeField] Vector3 _localOffset;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = _referenceObject.position+_localOffset;
       
    }
    private void OnEnable()
    {
        //this.transform.position = _referenceObject.position + _localOffset;
    }

    private void Update()
    {
        //Debug.Log("POS= " + _referenceObject.position);
    }
}
