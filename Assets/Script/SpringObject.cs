using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class SpringObject : MonoBehaviour
{
    public float coefficient = 20f;
    public float dampeningCoefficient = 0.03f;
    public float liquidDensity = 1000f;
    public float liquidDampening = 0.1f;
    public float mass = 0.1f;
    // [HideInInspector]
    public float cylinderRadius = 0.02f;
    public GameObject waterMesh;
    public GameObject springMesh;
    public bool useGravity = true;
    public bool useSpring = true;


    [HideInInspector]
    public float underwaterVolume = 0f;
    [HideInInspector]
    public float archimedesForce = 0f;

    private float startPosition;
    private float cylinderDiameter;
    private Vector2 initialScale;
    private Vector2 initialBounds;
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

        waterMeshRenderer = waterMesh.GetComponent<MeshRenderer>();
        waterMeshMaterial = waterMesh.GetComponent<Renderer>().material;
        waterMeshLowerY = waterMeshRenderer.bounds.min.y;
        waterMeshDiameter = waterMeshRenderer.bounds.size.x;
        initialWaterHeight = getWaterY();
        initialScale = new Vector2(transform.localScale.x, transform.localScale.z);
        initialBounds = new Vector2(meshRenderer.bounds.size.x, meshRenderer.bounds.size.z);
    }

    void FixedUpdate()
    {
        if (rb.mass != mass) rb.mass = mass;
        if (rb.useGravity != useGravity) rb.useGravity = useGravity;
        if (springMesh.activeSelf != useSpring) springMesh.SetActive(useSpring);

        cylinderDiameter = cylinderRadius * 2f;
        transform.localScale = new Vector3(
            initialScale.x * cylinderDiameter / initialBounds.x,
            transform.localScale.y,
            initialScale.y * cylinderDiameter / initialBounds.y
        );

        Vector3 direction = new Vector3();
        if (useSpring) {
            float distanceDifference = transform.position.y - startPosition;
            float hooke = -coefficient * distanceDifference;
            float dampening = -dampeningCoefficient * rb.velocity.y;
            direction = Vector3.up * (hooke + dampening);
        }

        float waterY = getWaterY();
        float underwaterHeight = waterY - meshRenderer.bounds.min.y;
        if (underwaterHeight > 0)
        {
            underwaterVolume = Mathf.Pow(cylinderDiameter / 2f, 2) * underwaterHeight;
            float displacedHeight = underwaterVolume / Mathf.Pow(waterMeshDiameter / 2f, 2);
            setWaterY(displacedHeight);

            // archimedes force
            archimedesForce = liquidDensity * underwaterVolume * 9.81f;
            float liquidDrag = 0.5f * liquidDensity * rb.velocity.y * rb.velocity.y * (3.1415f * Mathf.Pow(cylinderDiameter / 2f, 2)) * liquidDampening;
            direction.y += archimedesForce - liquidDrag;
        }
        else
        {
            archimedesForce = 0f;
            underwaterVolume = 0f;
        }

        rb.AddForce(direction);
    }

    void setWaterY(float displacedHeight)
    {
        float totalHeight = initialWaterHeight + displacedHeight;
        float fill = (totalHeight - waterMeshLowerY) / (waterMeshRenderer.bounds.size.y * waterMesh.transform.localScale.y);
        waterMeshMaterial.SetFloat("_Fill", fill);
    }

    float getWaterY()
    {
        float waterMeshFill = waterMeshMaterial.GetFloat("_Fill");
        return waterMeshLowerY + waterMeshRenderer.bounds.size.y * waterMesh.transform.localScale.y * waterMeshFill;
    }

    public void setPropertyValue(string propertyName, object value)
    {
        this.GetType().GetField(propertyName).SetValue(this, value);
    }

}
