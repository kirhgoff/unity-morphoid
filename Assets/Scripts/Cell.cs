using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    // Cell properties
    public Guid id { get; set; }
    public string prefabName { get; set; }
    public float lifespan { get; set; }
    public float currentAge { get; set; }

    public Vector3 initialSize { get; set; }
    public Vector3 finalSize { get; set; }

    public GameObject cellObject { get; set; }

    public void InstantiatePrefab()
    {
        GameObject prefab = Resources.Load<GameObject>(prefabName);
        cellObject = GameObject.Instantiate(prefab);

        cellObject.transform.localScale = initialSize;
        cellObject.transform.position = Vector3.zero;
    }

    public void UpdateSize()
    {
        float factor = currentAge / lifespan;

        Vector3 newSize = Vector3.Lerp(initialSize, finalSize, factor);
        cellObject.transform.localScale = newSize;
    }
}
