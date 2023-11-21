using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FidController : MonoBehaviour
{
    public virtual void RotateController(float rotSpeed, float moveSpeed) {
        if (moveSpeed > 0.05f)
        {
            return;
        }
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius += rotSpeed;
        if (sphereCollider.radius > 4)
            sphereCollider.radius = 4;
        else if (sphereCollider.radius < -4)
            sphereCollider.radius = -4;
    }
}
