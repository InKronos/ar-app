using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Bomb : MonoBehaviour
{
    public float esplosionRadius = 5f;
    private ARPlaneManager arPlaneManager;

    void OnCollisionEnter(Collision collision) {
        if (IsCollisionWithARPlane(collision)) {
            Explode();
        }
    }

    bool IsCollisionWithARPlane(Collision collision) {
        foreach (ContactPoint contact in collision.contacts) {
            ARPlane arPlane = GetARPlaneFromPosition(contact.point);
            if (arPlane != null) {
                return true;
            }
        }
        return false;
    }

    ARPlane GetARPlaneFromPosition(Vector3 position) {
        if (arPlaneManager == null) {
            arPlaneManager = FindObjectOfType<ARPlaneManager>();
        }

        // if (arPlaneManager != null) {
        //     List<ARRaycastHit> hits = new List<ARRaycastHit>();
        //     if (arPlaneManager.Raycast(new Ray(position, Vector3.down), hits))
        //     {
        //         return hits[0].plane;
        //     }
        // }

        return null;
    }

    void Explode() {
        Debug.Log("BOOM");
    }
}
