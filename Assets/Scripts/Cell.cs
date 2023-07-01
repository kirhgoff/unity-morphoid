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

    public GameObject gameObject { get; set; }

    public void InstantiatePrefab()
    {
        GameObject prefab = Resources.Load<GameObject>(prefabName);
        gameObject = GameObject.Instantiate(prefab) as GameObject;
        
        gameObject.name = "Cell#" + id.ToString();
        gameObject.transform.localScale = initialSize;
        gameObject.transform.position = Vector3.zero;
    }

    public void UpdateSize()
    {
        float factor = currentAge / lifespan;

        Vector3 newSize = Vector3.Lerp(initialSize, finalSize, factor);
        gameObject.transform.localScale = newSize;
    }
}
