using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    GameObject target;

    void Start()
    {
        target = transform.parent.gameObject;
    }

    void Update()
    {
        transform.LookAt(transform.position);
    }
}
