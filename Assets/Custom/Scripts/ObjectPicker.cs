using UnityEngine;
 
public class ObjectPicker : MonoBehaviour
{
    public GameObject obj;

    public bool isCatched = false;

    void Update()
    {
        if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began && isCatched == false) { //for pc = if(Input.GetButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position); //for pc = Input.mousePosition
            RaycastHit hit;
 
            if (Physics.Raycast (ray, out hit, 100f)) {
                if (hit.transform.tag == "Throwable") {
                    
                    obj = hit.transform.gameObject;
                    PickUp(obj);
                    isCatched = true;
                }
            }
        }

        
    }

    void PickUp(GameObject gameObject){

        gameObject.transform.position = Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 0.1f, Camera.main.nearClipPlane * 7.5f));
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.Euler (0f, 0F, 0f);
        

    }
    
}