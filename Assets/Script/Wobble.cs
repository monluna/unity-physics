using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour
{
    Renderer rend;
    Vector3 lastPos;
    Vector3 pos;
    Vector3 velocity;
    Vector3 lastRot;  
    Vector3 angularVelocity;
    public float MaxWobble = 0.03f;
    public float WobbleSpeed = 1f;
    public float Recovery = 1f;
    public float fillAmount = 0;
    public Mesh mesh;
    float wobbleAmountX;
    float wobbleAmountZ;
    float wobbleAmountToAddX;
    float wobbleAmountToAddZ;
    float pulse;
    float time = 0.5f;
    
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    private void Update()
    {
        time += Time.deltaTime;
        // velocity
        velocity = (lastPos - transform.position) / Time.deltaTime;
        angularVelocity = transform.rotation.eulerAngles - lastRot;
        
        // decrease wobble over time
        wobbleAmountToAddX = Mathf.Clamp((velocity.x + angularVelocity.z) * MaxWobble, -MaxWobble, MaxWobble);
        wobbleAmountToAddZ = Mathf.Clamp((velocity.z + angularVelocity.x) * MaxWobble, -MaxWobble, MaxWobble);

        // make a sine wave of the decreasing wobble
        pulse = 2 * Mathf.PI * WobbleSpeed;
        wobbleAmountX = wobbleAmountToAddX * Mathf.Sin(pulse * time);
        wobbleAmountZ = wobbleAmountToAddZ * Mathf.Sin(pulse * time);

        // send it to the shader
        rend.material.SetFloat("_WobbleX", wobbleAmountX);
        rend.material.SetFloat("_WobbleZ", wobbleAmountZ);

        // add clamped velocity to wobble
        wobbleAmountToAddX += Mathf.Clamp((velocity.x + (angularVelocity.z * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);
        wobbleAmountToAddZ += Mathf.Clamp((velocity.z + (angularVelocity.x * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);

        Vector3 worldPos = transform.TransformPoint(new Vector3(mesh.bounds.center.x, mesh.bounds.center.y, mesh.bounds.center.z));
        pos = worldPos - transform.position - new Vector3(0, fillAmount, 0);
        rend.material.SetVector("_FillAmount", pos);

        
        // keep last position
        lastPos = transform.position;
        lastRot = transform.rotation.eulerAngles;
    }



}