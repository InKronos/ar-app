using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnchancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(requiredComponent: typeof(ARRaycastManager),
    requiredComponent2: typeof(ARPlaneManager))]

public class PlaceObject : MonoBehaviour
{   
    [SerializeField] 
    private GameObject prefab;
    private Camera arCam;

    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    GameObject obj;

    private void Awake() {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
        // arCam = GameObject.Find("MainCamera").GetComponent<Camera>();
    }

    private void OnEnable() {
        EnchancedTouch.TouchSimulation.Enable();
        EnchancedTouch.EnhancedTouchSupport.Enable();
        EnchancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable() {
        EnchancedTouch.TouchSimulation.Disable();
        EnchancedTouch.EnhancedTouchSupport.Disable();
        EnchancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void FingerDown(EnchancedTouch.Finger finger) {
        if (finger.index != 0 ) return;

        if (aRRaycastManager.Raycast(finger.currentTouch.screenPosition,
            hits, TrackableType.PlaneWithinPolygon)) {
            foreach(ARRaycastHit hit in hits) {
                
                if (obj == null) {
                    Pose pose = hit.pose;
                    pose.position.y += 0.5f;
                    pose.rotation.y += 90.0f;

                    obj = Instantiate(prefab, pose.position, pose.rotation);
                    Debug.Log("Heli placed");

                    if (aRPlaneManager.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp) {
                        Vector3 position = obj.transform.position;
                        Vector3 cameraPosition = Camera.main.transform.position;
                        Vector3 direction = cameraPosition - position;
                        Vector3 targetRotationEuler = Quaternion.LookRotation(direction).eulerAngles;
                        Vector3 scaledEuler = Vector3.Scale(targetRotationEuler, obj.transform.up.normalized);
                        Quaternion targetRotation = Quaternion.Euler(scaledEuler);
                        obj.transform.rotation = obj.transform.rotation * targetRotation;
                    }
                }
            }
        }
        
        // Something fishy
        RaycastHit h;
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        if (Input.GetTouch(0).phase == TouchPhase.Began) {
            if (Physics.Raycast(ray, out h)) {
                if (h.collider.gameObject.tag == "helic") {
                    h.collider.gameObject.GetComponent<Controller>().toggleRotation();
                }
            }
        }
    }
}
