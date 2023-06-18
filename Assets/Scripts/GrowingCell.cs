using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingCell: MonoBehaviour
{
    private Cell cell;

    void Start()
    {
        cell = new Cell();
        cell.id = Guid.NewGuid();
        cell.prefabName = "Cube"; // Make sure this prefab is in your Resources folder
        cell.lifespan = 10.0f;
        cell.currentAge = 0.0f;
        cell.initialSize = Vector3.one * 0.1f; // start at 10% of the final size
        cell.finalSize = Vector3.one; // end at 100% of the final size

        cell.InstantiatePrefab();
    }

    // Update is called once per frame
    void Update()
    {
        cell.currentAge += Time.deltaTime;
        if (cell.currentAge <= cell.lifespan)
        {
            cell.UpdateSize();
        }
    }
}
