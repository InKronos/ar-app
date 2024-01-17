using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.XR.CoreUtils;
public class BuildingPlacmentManager : MonoBehaviour
{
    
    [SerializeField]public GameObject SpawnableBuilding;
    [SerializeField]private XROrigin xROrigin;
    [SerializeField]private ARRaycastManager raycastManager;

    [SerializeField]private ARPlaneManager planeManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private bool canPlaceBuilding = true;

    private void Update() {
        if(Input.touchCount > 0 && canPlaceBuilding)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began)
            {
                raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon);

                bool collision = raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon);

                if(collision)
                {   
                    Debug.Log("Building placed");
                    GameObject building = Instantiate(SpawnableBuilding, hits[0].pose.position, hits[0].pose.rotation);
                    building.transform.localScale= new Vector3((float)0.01, (float)0.01, (float)0.01);
                    foreach(ARPlane plane in planeManager.trackables)
                    {
                        plane.gameObject.SetActive(false);
                    }

                    planeManager.enabled = false; 
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
    }
}
