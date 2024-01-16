using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManual : MonoBehaviour
{
    [SerializeField]
    private Transform bookHolder;
    [SerializeField]
    private Transform characterControllerTransform;
    private BoxCollider boxCollider;
    private bool showModal = false;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;

            if (boxCollider.Raycast(ray, out hit, 100.0f))
            {
                showModal = true;
            }
        }

        if (showModal) {
            transform.position = Vector3.Lerp(transform.position, bookHolder.position, 0.2f);


            Debug.Log(characterControllerTransform.eulerAngles.y);
            transform.localRotation = Quaternion.Lerp(
                transform.localRotation, Quaternion.Euler(-90f, characterControllerTransform.eulerAngles.y + 180f, 0f), 0.2f
            ); 
        }
    }
}
