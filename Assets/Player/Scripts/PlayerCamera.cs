using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject Player;
    public Vector3 Offset;



    private void Update()
    {
        transform.position = Player.transform.position + Offset;
        transform.forward = Player.transform.position - transform.position;
    }



}
