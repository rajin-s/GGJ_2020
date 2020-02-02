using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float FollowSpeed = 2f;
    [SerializeField] Transform playerA;
    [SerializeField] Transform playerB;

    private void Update()
    {

        Vector3 newPosition = (playerA.transform.position + playerB.transform.position) / 2;
        newPosition.z = -10;
        transform.position = Vector3.Slerp(transform.position, newPosition, FollowSpeed * Time.deltaTime);
    }
}
