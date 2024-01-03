using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringObject : MonoBehaviour
{
    public float coefficient = 20f;
    public float dampeningCoefficient = 0.03f;
    public float liquidDensity = 1000f;
    public float liquidDampening = 0.1f;
    public GameObject waterMesh;

    [HideInInspector]
    public float underwaterVolume = 0f;
    [HideInInspector]
    public float archimedesForce = 0f;

    private float startPosition;
    private float cylinderDiameter;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;

    private MeshRenderer waterMeshRenderer;
    private float waterMeshLowerY;
    private float waterMeshDiameter;
    private float initialWaterHeight;
    private Material waterMeshMaterial;
    

    void Start()
    {
        startPosition = transform.position.y;
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        cylinderDiameter = meshRenderer.bounds.size.x;
        
        waterMeshRenderer = waterMesh.GetComponent<MeshRenderer>();
        waterMeshMaterial = waterMesh.GetComponent<Renderer>().material;
        waterMeshLowerY = waterMeshRenderer.bounds.min.y;
        waterMeshDiameter = waterMeshRenderer.bounds.size.x;
        initialWaterHeight = getWaterY();
    }

    void FixedUpdate()
    {
        float distanceDifference = transform.position.y - startPosition;
        float hooke = -coefficient * distanceDifference;
        float dampening = -dampeningCoefficient * rb.velocity.y;
        Vector3 direction = Vector3.up * (hooke + dampening);

        float waterY = getWaterY();
        float underwaterHeight = waterY - meshRenderer.bounds.min.y;
        if (underwaterHeight > 0) {
            underwaterVolume = Mathf.Pow(cylinderDiameter / 2f, 2) * underwaterHeight;
            float displacedHeight = underwaterVolume / Mathf.Pow(waterMeshDiameter / 2f, 2);
            setWaterY(displacedHeight);
            
            // archimedes force
            archimedesForce = liquidDensity * underwaterVolume * 9.81f;
            direction.y += (archimedesForce - liquidDampening * rb.velocity.y);
        } else {
            archimedesForce = 0f;
            underwaterVolume = 0f;
        }

        rb.AddForce(direction);
    }

    void setWaterY(float displacedHeight) {
        float totalHeight = initialWaterHeight + displacedHeight;
        float fill = (totalHeight - waterMeshLowerY) / (waterMeshRenderer.bounds.size.y * waterMesh.transform.localScale.y);
        waterMeshMaterial.SetFloat("_Fill", fill);
    }

    float getWaterY() {
        float waterMeshFill = waterMeshMaterial.GetFloat("_Fill");
        return waterMeshLowerY + waterMeshRenderer.bounds.size.y * waterMesh.transform.localScale.y * waterMeshFill;
    }
}
