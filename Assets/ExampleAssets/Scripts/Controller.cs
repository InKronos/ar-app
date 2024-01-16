using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private float helicopterSpeed = 0.5f;
    [SerializeField] private float rotorSpeedModifier = 10f;
    [SerializeField] private Transform rotorsTransform;

    private FixedJoystick joystick;
    private Rigidbody rigidbody;

    private bool rotate = false;

    public void toggleRotation() {
        rotate = !rotate;
    }
    private void OnEnable()
    {
        joystick = FindObjectOfType<FixedJoystick>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        if (rotate) rotorsTransform.Rotate(Vector3.up * 50f * rotorSpeedModifier);

        float xVal = joystick.Horizontal;
        float zVal = joystick.Vertical;

        Vector3 movement = new Vector3(xVal, 0, zVal);
        rigidbody.velocity = movement * helicopterSpeed;

        if(xVal !=0 && zVal !=0) 
        { 
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.Atan2(xVal, zVal)*Mathf.Rad2Deg, transform.eulerAngles.z);
        }
    }

}
