using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPivotFollowsObject : MonoBehaviour
{
    public Transform target;

    private void FixedUpdate()
    {
        Vector3 pos = transform.position;
        transform.position = Vector3.Lerp(pos, target.position, 0.25f);
    }
}
