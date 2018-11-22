using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public Transform player;
    private Vector3 from = new Vector3(0, 0, -18);
    private readonly float maxSpeed = 0.2f;

    // Use this for initialization
    void Start()
    {
        if (this.player == null)
        {
            Player player = FindObjectOfType<Player>();
            this.player = player.gameObject.transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = (player.position + from) - transform.position;
        float speed = dir.magnitude;
        if (speed > maxSpeed) speed = maxSpeed;
        transform.position += dir.normalized * speed;
        //transform.rotation = Quaternion.LookRotation(player.position - transform.position);
    }
}
