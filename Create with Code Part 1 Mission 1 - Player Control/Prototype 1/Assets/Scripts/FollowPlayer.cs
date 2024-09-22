using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject Player;
    public Vector3 Offset = new Vector3(0, 7, -10);

    void LateUpdate()
    {
        transform.position = Player.transform.position + Offset;
    }
}
