using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static T Tap<T>(this T obj, Action<T> action)
    {
        action(obj);
        return obj;
    }
}

public class BodyPart
{
    public float relativeTimeToAppear; // The relative time when this part should appear
    public Guid parentId; // The ID of the parent cell that this part grows from
    public Cell cell; // The new cell to add
}

public class BodyPlan
{
    public List<BodyPart> bodyParts = new List<BodyPart>();
    public float lifespan; // lifespan of the organism
}

public class Organism
{
    public Dictionary<Guid, Cell> cells = new Dictionary<Guid, Cell>();
    public BodyPlan bodyPlan;
    private float currentTime;

    public Organism(BodyPlan bodyPlan)
    {
        this.bodyPlan = bodyPlan;
        currentTime = 0f;
    }

    public void Update()
    {
        currentTime += Time.deltaTime;

        foreach (var bodyPart in bodyPlan.bodyParts)
        {
            if (currentTime >= bodyPart.relativeTimeToAppear * bodyPlan.lifespan && !cells.ContainsKey(bodyPart.cell.id))
            {
                Cell parentCell = cells[bodyPart.parentId];

                // TODO: make a delegate?
                bodyPart.cell.gameObject.transform.Tap(child => {
                    parentCell.gameObject.transform.Tap(parent => {
                        child.position = parent.position + parentCell.gameObject.transform.forward;
                        child.rotation = parent.rotation;
                    });
                });

                // Instantiate the new cell
                bodyPart.cell.InstantiatePrefab();

                // Add the new cell to the cells dictionary
                cells.Add(bodyPart.cell.id, bodyPart.cell);
            }
        }

        // Update all cells
        foreach (var cell in cells.Values)
        {
            if (cell.currentAge <= cell.lifespan)
            {
                cell.currentAge += Time.deltaTime;
                cell.UpdateSize();
            }
        }
    }
}
