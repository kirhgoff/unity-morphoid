using System;
using System.Collections.Generic;
using UnityEngine;

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

        // TODO: move this part of code to body plan
        foreach (var bodyPart in bodyPlan.bodyParts)
        {
            if (currentTime < bodyPart.relativeTimeToAppear * bodyPlan.lifespan || 
                cells.ContainsKey(bodyPart.cell.id)
            ) {
                continue;
            }

            AddPart(bodyPart);
        }

        ProgressCells();
    }

    void ProgressCells()
    {
        foreach (var cell in cells.Values)
        {
            if (cell.currentAge <= cell.lifespan)
            {
                cell.currentAge += Time.deltaTime;
                cell.UpdateSize();
            }
        }
    }

    void AddPart(BodyPart bodyPart)
    {            
        Cell parentCell = cells[bodyPart.parentCellId];

        // TODO: add trasform to the cell and use it here
        bodyPart.cell.gameObject.transform.Tap(child => {
            parentCell.gameObject.transform.Tap(parent => {
                child.position = parent.position + parentCell.gameObject.transform.forward;
                child.rotation = parent.rotation;
            });
        });

        bodyPart.cell.InstantiatePrefab();
        
        cells.Add(bodyPart.cell.id, bodyPart.cell);
    }
}
