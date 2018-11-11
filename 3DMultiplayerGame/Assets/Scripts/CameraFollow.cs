using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    public float distance = 5.0f;
    public float height = 3.0f;
    public float damping = 5.0f;
    public bool smoothRotation = true;
    public bool followBehind = true;
    public float rotationDamping = 10.0f;
    public float lookRotation;

    private float minHeight;
    private CarBehaviour _carBehaviour;

    private void Start()
    {
        
    }

    private void Update()
    {
        //if (_carBehaviour.GetState() != PlayerStates.ALIVE)
        //    return;

        if (Car.transform == null)
        {
            return;
        }
        else
        {
            target = Car.transform;
        }
        
        Vector3 wantedPosition;

        followBehind = true;

        if (Input.GetKey(KeyCode.Q))
        {
            followBehind = false;
        }

        if (followBehind)
        {
            wantedPosition = target.TransformPoint(0, height, -distance);
        }
        else
            wantedPosition = target.TransformPoint(0, height, distance);
        if(minHeight == 0) { minHeight = height; }

        if(wantedPosition.y < minHeight)
        {
            wantedPosition = new Vector3(wantedPosition.x, minHeight, wantedPosition.z);
        }

        transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);

        if (smoothRotation)
        {
            //new Vector3(lookTotation, 0, 0)
            Quaternion wantedRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
            
        }
        else transform.LookAt(target, Vector3.up);

        transform.eulerAngles = new Vector3(lookRotation, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}