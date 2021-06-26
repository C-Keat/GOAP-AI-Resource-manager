using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMarker : MonoBehaviour
{
    public float lifeTime = 1.0f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
