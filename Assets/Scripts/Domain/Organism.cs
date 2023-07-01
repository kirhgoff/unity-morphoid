using System;
using System.Collections.Generic;
using UnityEngine;

using Utils;

namespace Domain {
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
            bodyPart.cell.InstantiatePrefab();

            if (!cells.ContainsKey(bodyPart.parentCellId))
            {
                // Its a root
                bodyPart.cell.gameObject.transform.position = new Vector3(0, 0, 0);
            } 
            else 
            {
                // Its a child
                Cell parentCell = cells[bodyPart.parentCellId];

                // TODO: add trasform to the cell and use it here
                bodyPart.GetTransform().Tap(child => {
                    parentCell.GetTransform().Tap(parent => {
                        child.position = parent.position + parentCell.gameObject.transform.forward;
                        child.rotation = parent.rotation;
                    });
                });
            }

            cells.Add(bodyPart.cell.id, bodyPart.cell);
        }
    }
}