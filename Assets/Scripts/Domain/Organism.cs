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

            if (IsRoot(bodyPart))
            {
                // TODO: it works only because the root is always at 0,0,0
                bodyPart.cell.gameObject.transform.position = new Vector3(0, 0, 0);
            } 
            else 
            {
                Cell parentCell = cells[bodyPart.parentCellId];

                bodyPart.GetTransform().Tap(child => {
                    Place(child, parentCell);
                });
            }

            cells.Add(bodyPart.cell.id, bodyPart.cell);
        }

        Boolean IsRoot(BodyPart bodyPart)
        {
            return bodyPart.parentCellId == Guid.Empty;
        }

        private void Place(Transform child, Cell parent)
        {
            ConnectionPoint connectionPoint = parent.GetUnoccupiedConnectionPoint();
            
            if (connectionPoint == null)
            {
                Debug.LogError("No available connection points on parent cell");
                return;
            }

            child.position = parent.GetPosition() + parent.GetTransform().TransformDirection(connectionPoint.position);
            child.rotation = Quaternion.LookRotation(parent.GetTransform().TransformDirection(connectionPoint.forward), parent.GetTransform().up);
            
            connectionPoint.isOccupied = true;
        }        
    }
}