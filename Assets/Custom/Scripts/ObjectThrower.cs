using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
//TODO La pelota no se debera reiniciar con el tiempo sino que cuando haga contacto con los planos del suelo. 
//TODO Esta iniciara en reposo en la mesa y solo desde ahi podras agarrarla
public class ObjectThrower : InputBase
{

    private GameObject holdObject;
    float startTime, endTime, swipeDistance, swipeTime;
    private Vector2 startPosition;
    private Vector2 endPosition;

    public float minSwipeDistance;
    private float objectVelocity;
    private float objectSpeed;
    public float maxObjectSpeed;
    private Vector3 angle;

    private bool thrown, holding;
    private Vector3 newPosition;

    public float smooth = 0.75f;

    Rigidbody rigidbody;

    bool isPressed;


    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        getObject();
    }

    void getObject()
    {
        GameObject _holdObject = GameObject.FindGameObjectWithTag("Throwable");
        holdObject = _holdObject;
        rigidbody = holdObject.GetComponent<Rigidbody>();
        ResetHoldObject();
    }

    void ResetHoldObject()
    {
        angle = Vector3.zero;
        endPosition = Vector3.zero;
        startPosition = Vector3.zero;
        objectSpeed = 0;
        startTime = 0;
        endTime = 0;
        swipeDistance = 0;
        swipeTime = 0;
        thrown = holding = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
        holdObject.transform.position = transform.position;
        holdObject.transform.rotation = transform.rotation;
    }

    void CatchObject()
    {

        Vector3 touchPosition = Pointer.current.position.ReadValue();
        touchPosition.z = Camera.main.nearClipPlane * 5f;
        newPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        holdObject.transform.localPosition = Vector3.Lerp(holdObject.transform.localPosition, newPosition, 80f * Time.deltaTime);

    }

    void Update()
    {
        if (holding)
        {
            CatchObject();
        }

        if (thrown)
        {
            return;
        }

        Vector2 touchPosition = Pointer.current.position.ReadValue();

        if (isPressed == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 50f))
            {

                if (hit.transform.tag == holdObject.transform.tag)
                {
                    startTime = Time.time;
                    startPosition = touchPosition;
                    holding = true;
                }
            }



        }
        else if (isPressed == false)
        {

            endTime = Time.time;
            endPosition = touchPosition;
            swipeDistance = (endPosition - startPosition).magnitude;
            swipeTime = endTime - startTime;
            holding = false;
            rigidbody.isKinematic = false;
            if (swipeTime < 0.5f && swipeDistance > 10f)
            {
                //Lanzamos el Objeto
                CalculateSpeed();
                CalculateAngle();

                rigidbody.AddForce(new Vector3(angle.x * objectSpeed, angle.y * objectSpeed, angle.z * objectSpeed));
                rigidbody.useGravity = true;
                thrown = true;
                Invoke("ResetHoldObject", 4f);

            }
            else
            {

                ResetHoldObject();

            }
            return;
        }


    }


    private void CalculateAngle()
    {

        angle = Camera.main.ScreenToWorldPoint(
            new Vector3(endPosition.x,
                        endPosition.y,
                        Camera.main.nearClipPlane)
            );

    }
    void CalculateSpeed()
    {

        if (swipeTime > 0)
            objectVelocity = swipeDistance / (swipeDistance - swipeTime);

        objectSpeed = objectVelocity * 40;

        if (objectSpeed <= maxObjectSpeed)
        {

            objectSpeed = maxObjectSpeed;

        }

        swipeTime = 0f;

    }

    protected override void OnPress(Vector3 position) => isPressed = true;

    protected override void OnRelease() => isPressed = false;
}
