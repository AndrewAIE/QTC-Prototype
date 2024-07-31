using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENEMY : MonoBehaviour
{
    public void ResetEnemy()
    {
        transform.position = new Vector3(Random.Range(-10, 11), 1, Random.Range(-10, 11));
    }
}
