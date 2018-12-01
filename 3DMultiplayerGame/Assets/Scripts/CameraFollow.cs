using UnityEngine;
using System.Collections;


public class CameraFollow : MonoBehaviour
{
    public GameObject _target;
    public float distance = 12.0f;
    public float height = 17;
    public float damping = 5.0f;
    public bool smoothRotation = true;
    public bool followBehind = true;
    public float rotationDamping = 10.0f;
    public float lookRotation;
    public LayerMask layer;

    private float _originalDamping = 5f;
    private float _originalRotationDampig = 10f;
    private Camera _camera;
    private Transform target;
    private float minHeight = 17f;
    private CarBehaviour _carBehaviour;
    public bool _hasTarget = false;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        CameraTransition();
    }

    private void Update()
    {
        if (!_hasTarget)
            return;

        if (_carBehaviour.GetState() != PlayerStates.ALIVE)
        {
            return;
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
        {
            wantedPosition = target.TransformPoint(0, height, distance);
        }

        if (wantedPosition.y < minHeight)
        {
            wantedPosition = new Vector3(wantedPosition.x, minHeight, wantedPosition.z);
        }

        transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);

        if (smoothRotation)
        {
            Quaternion wantedRotation = Quaternion.LookRotation(target.forward, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);

        }
        else transform.LookAt(target, Vector3.up);

        transform.eulerAngles = new Vector3(lookRotation, transform.eulerAngles.y, transform.eulerAngles.z);
        Collision();
        
    }

    public void SetTheTarget(GameObject obj)
    {
        _target = obj;
        _carBehaviour = _target.GetComponent<CarBehaviour>();
    }

    private void CameraTransition()
    {
        Debug.Log("Chamada");
        StartCoroutine(CameraTransitionCoroutine());
    }

    private IEnumerator CameraTransitionCoroutine()
    {
        yield return new WaitForSeconds(3f);
        damping = _originalDamping;
        rotationDamping = _originalRotationDampig;
    }

    private void Collision()
    {
        RaycastHit hitInfo = new RaycastHit();
        Debug.DrawLine(target.position, transform.position);

        if (Physics.Linecast(target.position, transform.position, out hitInfo, layer))
        {
            transform.position = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
        }
    }

    //public void SetPosition(Vector3 pos)
    //{
    //    transform.position = pos;
    //}

    public void SetTarget(Transform tar)
    {
        target = tar;
        _hasTarget = true;
    }
}