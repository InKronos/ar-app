using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.XR.CoreUtils;
using EnchancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(requiredComponent: typeof(ARRaycastManager),
    requiredComponent2: typeof(ARPlaneManager))]

public class BuildingPlacmentManager : MonoBehaviour
{
    
    [SerializeField]public GameObject SpawnableBuilding;
    [SerializeField]private XROrigin xROrigin;
    [SerializeField]private ARRaycastManager aRRaycastManager;

    [SerializeField]private ARPlaneManager aRPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    GameObject obj;


    private bool canPlaceBuilding = true;

        private void Awake() {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
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
                Pose pose = hit.pose;
                pose.rotation.y += 90.0f;

                obj = Instantiate(SpawnableBuilding, pose.position, pose.rotation);
                obj.transform.localScale= new Vector3((float)0.01, (float)0.01, (float)0.01);
                Debug.Log("Building placed");

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

    public bool isButtonPressed(){
        if(EventSystem.current.currentSelectedGameObject?.GetComponent<Button>() == null)
        {
            return false;
        }
        else{
            return true;
        }
    }

    public void SwitchBuldings(GameObject building){
        SpawnableBuilding = building;
    }

    public void StopPlacingBuildings(){
        canPlaceBuilding = false;

        // foreach(ARPlane plane in aRPlaneManager.trackables)
        // {
        //     plane.gameObject.SetActive(false);
        // }
        aRPlaneManager.enabled = false;
    }
}
