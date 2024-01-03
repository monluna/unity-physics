using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strech : MonoBehaviour
{
    public Transform anchor1;
    public Transform anchor2;
    public float offset = 0.2f;
    public float scaleCoefficient = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float centerPos = ( anchor1.position.y + anchor2.position.y ) / 2.0f;
        transform.position = new Vector3(transform.position.x, centerPos - offset, transform.position.z);
        
        float scaleY = Mathf.Abs(anchor1.position.y - anchor2.position.y) * scaleCoefficient;
        transform.localScale = new Vector3(transform.localScale.x, scaleY, transform.localScale.z);
    }

}
