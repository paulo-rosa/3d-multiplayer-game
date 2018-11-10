using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class CarAntiRollBar: MonoBehaviour
    {
        public WheelCollider WheelL;
        public WheelCollider WheelR;
        public float AntiRoll = 2000.0f;
        public  Rigidbody carRigidBody;

        private void FixedUpdate()
        {
            var hit = new WheelHit();
            var travelL = 1.0f;
            var travelR = 1.0f;

            var groundedL = WheelL.GetGroundHit(out hit);
            if (groundedL)
                travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;

            var groundedR = WheelR.GetGroundHit(out hit);
            if (groundedR)
                travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;

            var antiRollForce = (travelL - travelR) * AntiRoll;

            if (groundedL)
                carRigidBody.AddForceAtPosition(WheelL.transform.up * -antiRollForce,
                       WheelL.transform.position);

            if (groundedR)
                carRigidBody.AddForceAtPosition(WheelR.transform.up * antiRollForce,
                       WheelR.transform.position);
        }

    }
}
