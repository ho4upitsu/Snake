using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Food : MonoBehaviour
{
    [SerializeField] public BoxCollider2D spawnArea;
    private void Start()
    {
        RandomPosition();
    }

    private void RandomPosition()
    {
        Bounds bounds = this.spawnArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector2(Mathf.Round(x), Mathf.Round(y));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
            RandomPosition();
    }
}
