using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectPlacer : InputBase
{
    [SerializeField]
    [Tooltip("Este sera el objeto que se va a instanciar en el plano")]
    GameObject prefab;

    GameObject spawnedPrefab;

    bool isPressed;

    ARRaycastManager aRRaycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    protected override void Awake()
    {
        base.Awake();
        aRRaycastManager = GetComponent<ARRaycastManager>();  
    }

    void Update()
    {
        if (Pointer.current == null || isPressed == false){
            return;
        }

        Vector2 touchPosition = Pointer.current.position.ReadValue();

        if(aRRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon)){

            var hitPose = hits[0].pose;

            if(spawnedPrefab == null){

                spawnedPrefab = Instantiate(prefab, hitPose.position, hitPose.rotation);
                
            }
            else{

                spawnedPrefab.transform.position = hitPose.position;
                spawnedPrefab.transform.rotation = hitPose.rotation;

            }

        }
    }

    protected override void OnPress(Vector3 position) => isPressed = true;

    protected override void OnRelease() => isPressed = false;
}
