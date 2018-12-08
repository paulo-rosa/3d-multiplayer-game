using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollision : MonoBehaviour {

    public LayerMask Layer;
    public BoxCollider _collider;
    private Vector3 _objectScale;
    #region Struct
    private Vector3 frontLeft;
    private Vector3 frontRight;
    private Vector3 backLeft;
    private Vector3 backRight;
    private Vector3 downMiddle;
    private Vector3 downBack;

    #endregion
    // Use this for initialization
    private void Start ()
    {
        _objectScale = _collider.transform.localScale;
        Debug.Log(_objectScale);
    }

    private void Update ()
    {
        GetPositions();
        GroundCollision();
        FrontCollision();
        BackCollision();
        HoleCollision();
    }



    private void GetPositions()
    {
        var xMargin = _collider.size.x * .47f;
        var zMargin = _collider.size.z * .47f;
        frontLeft = _collider.transform.localPosition +_collider.transform.TransformPoint(-xMargin, 0, 0);
        frontRight = _collider.transform.localPosition + _collider.transform.TransformPoint(xMargin, 0, 0);
        backLeft = _collider.transform.localPosition + _collider.transform.TransformPoint(-xMargin, 0, 0);
        backRight = _collider.transform.localPosition + _collider.transform.TransformPoint(xMargin, 0, 0);
        downMiddle = _collider.transform.localPosition + _collider.transform.TransformPoint(0, 0, 0);
        downBack = _collider.transform.localPosition + _collider.transform.TransformDirection(0, 0, -zMargin);
    }
    public bool GroundCollision()
    {
        Debug.DrawRay(downMiddle, transform.TransformDirection(Vector3.down), Color.red, 1f);
        if (Physics.Raycast(downMiddle, transform.TransformDirection(Vector3.down), (_objectScale.y * .6f) * _collider.size.y, Layer))
        {
            return true;
        }

        return false;
    }

    public bool FrontCollision()
    {
        Debug.DrawRay(frontLeft , transform.TransformDirection(Vector3.forward * (_objectScale.z * .62f) * _collider.size.z), Color.blue, 1f);
        Debug.DrawRay(frontRight, transform.TransformDirection(Vector3.forward * (_objectScale.z * .62f) * _collider.size.z), Color.green, 1f);

        if (Physics.Raycast(frontLeft, transform.TransformDirection(Vector3.forward), (_objectScale.z * .62f) * _collider.size.z, Layer))
        {
            return true;

        }

        if (Physics.Raycast(frontRight, transform.TransformDirection(Vector3.forward), (_objectScale.z * .62f) * _collider.size.z, Layer))
        {
            return true;

        }

        return false;
    }

    public bool BackCollision()
    {
        Debug.DrawRay(frontLeft, transform.TransformDirection(Vector3.forward * -(_objectScale.z * .52f) * _collider.size.z), Color.yellow, 1f);
        Debug.DrawRay(frontRight, transform.TransformDirection(Vector3.forward * -(_objectScale.z * .52f) * _collider.size.z), Color.yellow, 1f);

        if (Physics.Raycast(backLeft, transform.TransformDirection(Vector3.forward), -(_objectScale.y * .62f) * _collider.size.z, Layer))
        {
            return true;

        }

        if (Physics.Raycast(backRight, transform.TransformDirection(Vector3.forward), -(_objectScale.y * .62f) * _collider.size.z, Layer))
        {
            return true;
        }

        return false;
    }

    private void HoleCollision()
    {
        Debug.DrawRay(downBack, transform.TransformDirection(Vector3.down),Color.red, .2f);
    }

}
