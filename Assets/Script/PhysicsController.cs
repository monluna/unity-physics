using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhysicsController : MonoBehaviour
{
    public GameObject weight;
    public TMP_Text boardText;
    public Transform anchor1;
    public SpringObject weightSpringJoint;
    
    private Rigidbody weightRigidbody;
    private float initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        weightRigidbody = weight.GetComponent<Rigidbody>();
        initialPosition = anchor1.position.y; 
    }

    float getSpringLength() {
        return anchor1.position.y - initialPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float mass = weightRigidbody.mass;
        float anchorDist = getSpringLength();
        float dampening = -weightSpringJoint.dampeningCoefficient * weightRigidbody.velocity.y;
        float springCoefficient = weightSpringJoint.coefficient;
        float force = -anchorDist * springCoefficient;

        
        string weightText = mass.ToString();
        string springCoefficientText = springCoefficient.ToString();
        string anchorDistText = anchorDist.ToString();
        string forceText = force.ToString();
        string underwaterVolumeCM = (weightSpringJoint.underwaterVolume * 1000000).ToString();
        string archimedesForce = weightSpringJoint.archimedesForce.ToString();

        string text = $"Масса: {weightText} кг\nКоэффицент жесткости: {springCoefficientText}\ndx: {anchorDistText}\nF = -{springCoefficientText} * {anchorDistText} = {forceText} Н\n\nПогруженный объём: {underwaterVolumeCM} см3\nСила архимеда: {archimedesForce} Н";
        boardText.text = text;
    }
}
