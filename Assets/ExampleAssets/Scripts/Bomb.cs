using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;

public class Bomb : MonoBehaviour
{   

    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject explosion;
    
    // private ARPlaneManager arPlaneManager;
    GameObject bomb;
    GameObject exp;
    Vector3 b;

    // void Start()
    // {
    //     arPlaneManager = FindObjectOfType<ARPlaneManager>();
    // }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "building") {
                Explode();
                Destroy(collision.gameObject);
            }
        //exp = Instantiate(explosion, bomb.transform.position, Quaternion.Euler(0f, 0f, 0f));
        Explode();
    }

    // void Update() {

    //     foreach (ARPlane arPlane in arPlaneManager.trackables) {
    //         if (CheckCollisionWithPlane(arPlane)) {
    //             Explode();
    //         }
    //     }
    // }

    // bool CheckCollisionWithPlane(ARPlane arPlane) {
    //     Collider objectCollider = GetComponent<Collider>();

    //     if (objectCollider != null) {
    //         Vector3 planePosition = arPlane.transform.position;
    //         Vector3 planeExtents = new Vector3(arPlane.extents.x, 0.001f, arPlane.extents.y);

    //         return objectCollider.bounds.Intersects(new Bounds(planePosition, planeExtents));
    //     }

    //     return false;
    // }

    void Explode() {
        Debug.Log("BOOM");
        Destroy(gameObject);
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(0.25f);
        exp = Instantiate(explosion, b, Quaternion.Euler(0f, 0f, 0f));
    }

    public void dropBomb() {
        GameObject helic = GameObject.FindGameObjectWithTag("helic");
        Vector3 p = helic.transform.position;
        p.y -= 0.07f;
        bomb = Instantiate(prefab, p, Quaternion.Euler(180f, 0f, 0f));
        bomb.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        b= bomb.transform.position;
        b.y -= 0.4f;
        StartCoroutine(Spawn());
        print("Bomb dropped");
    }
}
