using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTheShip : MonoBehaviour
{
    [SerializeField] private Transform target;
    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.position = targetPosition;
    }
}
