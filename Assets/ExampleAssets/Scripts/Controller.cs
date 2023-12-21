using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private float rotorSpeedModifier = 10f;
    [SerializeField] private Transform rotorsTransform;

    private bool rotate = false;

    public void toggleRotation() {
        rotate = !rotate;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (rotate) rotorsTransform.Rotate(Vector3.up * 50f * rotorSpeedModifier);
    }

}
