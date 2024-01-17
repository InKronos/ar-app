using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PrefabCreator : MonoBehaviour
{
    [SerializeField] private GameObject helicopterPrefab;
    [SerializeField] private Vector3 prefabOffset;
    private ARTrackedImageManager arTrackedImageManager;

    private GameObject helicopter;

    private bool canSpawnHeli = false;

    private void OnEnable()
    {
            arTrackedImageManager = gameObject.GetComponent<ARTrackedImageManager>();
            arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (ARTrackedImage image in obj.added)
        {
                Debug.Log("Helic spwaned");
                helicopter = Instantiate(helicopterPrefab, image.transform);
                helicopter.transform.position += prefabOffset;
        }
            
    }
    


    
}