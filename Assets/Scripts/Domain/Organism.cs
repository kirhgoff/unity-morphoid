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

        private Boolean stopDoingThis = false;

        public Organism(BodyPlan bodyPlan)
        {
            this.bodyPlan = bodyPlan;
            currentTime = 0f;
        }

        public void Update()
        {
            if (stopDoingThis)
            {
                return;
            }

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
            var cell = bodyPart.cell;
            var prefab = Resources.Load<GameObject>(cell.prefabName);
            cell.gameObject = GameObject.Instantiate(prefab) as GameObject;
            cell.gameObject.name = bodyPart.name + "#" + cell.id.ToString();
            cell.gameObject.transform.localScale = cell.initialSize;

            if (IsRoot(bodyPart))
            {
                // We assign global coordinates here
                bodyPart.cell.gameObject.transform.position = new Vector3(0, 0, 0);
            } 
            else 
            {
                Place(bodyPart.cell, cells[bodyPart.parentCellId]);
            }

            cell.InitializeCubeConnectionPoints();

            cells.Add(bodyPart.cell.id, bodyPart.cell);
            // TODO: remove this
            if (bodyPart.name != "body") stopDoingThis = true;
        }

        Boolean IsRoot(BodyPart bodyPart)
        {
            return bodyPart.parentCellId == Guid.Empty;
        }

        private void Place(Cell childCell, Cell parentCell)
        {
            ConnectionPoint connectionPoint = parentCell.GetUnoccupiedConnectionPoint();

            if (connectionPoint == null)
            {
                Debug.LogError("No available connection points on parent cell");
                return;
            }

            Vector3 localChildCellPosition = connectionPoint.position + connectionPoint.forward * childCell.gameObject.transform.localScale.x / 2f;
            Vector3 worldChildCellPosition = parentCell.gameObject.transform.TransformPoint(localChildCellPosition);
            Quaternion worldChildCellOrientation = Quaternion.LookRotation(parentCell.gameObject.transform.TransformDirection(connectionPoint.forward));

            childCell.gameObject.transform.position = worldChildCellPosition;
            childCell.gameObject.transform.rotation = worldChildCellOrientation;

            childCell.gameObject.transform.SetParent(parentCell.gameObject.transform);

            connectionPoint.isOccupied = true;
        }
    }
}