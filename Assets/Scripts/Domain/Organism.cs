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
                bodyPart.cell.gameObject.transform.rotation = new Vector3(0, 0, 0);
            } 
            else 
            {
                Place(bodyPart.cell, cells[bodyPart.parentCellId]);
            }

            cells.Add(bodyPart.cell.id, bodyPart.cell);
        }

        Boolean IsRoot(BodyPart bodyPart)
        {
            return bodyPart.parentCellId == Guid.Empty;
        }

        private void Place666(Cell childCell, Cell parentCell)
        {
            ConnectionPoint connectionPoint = parentCell.GetUnoccupiedConnectionPoint();

            if (connectionPoint == null)
            {
                Debug.LogError("No available connection points on parent cell");
                return;
            }

            // First, we want to get the direction to the connection point from the center of the parent cell.
            Vector3 directionToConnectionPoint = connectionPoint.position.normalized;

            // Then we calculate the distance we need to move the child cell away from the parent cell along that direction. 
            // This is half the scale of the parent cell (i.e., radius), plus half the scale of the child cell.
            float distanceToMove = parentCell.gameObject.transform.localScale.x / 2.0f + childCell.gameObject.transform.localScale.x / 2.0f;

            // Finally, we calculate the new position for the child cell.
            Vector3 childPosition = parentCell.gameObject.transform.position + directionToConnectionPoint * distanceToMove;

            // Apply the new position and rotation to the child cell.
            childCell.gameObject.transform.position = childPosition;
            childCell.gameObject.transform.rotation = Quaternion.LookRotation(
                parentCell.GetTransform().TransformDirection(connectionPoint.forward),
                parentCell.GetTransform().up
            );

            connectionPoint.isOccupied = true;
        }

        private void Place665(Cell childCell, Cell parentCell)
        {
            ConnectionPoint connectionPoint = parentCell.GetUnoccupiedConnectionPoint();

            if (connectionPoint == null)
            {
                Debug.LogError("No available connection points on parent cell");
                return;
            }

            Vector3 directionToConnectionPoint = parentCell.GetTransform().TransformDirection(connectionPoint.position.normalized);
            float distanceToMove = parentCell.gameObject.transform.localScale.x / 2.0f + childCell.gameObject.transform.localScale.x / 2.0f;

            // Transform the local position of the connection point to world coordinates
            Vector3 connectionPointWorldPosition = parentCell.GetTransform().TransformPoint(connectionPoint.position);

            Vector3 childPosition = connectionPointWorldPosition + directionToConnectionPoint * distanceToMove;
            childCell.gameObject.transform.position = childPosition;

            childCell.gameObject.transform.rotation = Quaternion.LookRotation(
                parentCell.GetTransform().TransformDirection(connectionPoint.forward),
                parentCell.GetTransform().up
            );

            connectionPoint.isOccupied = true;
        }

        private void Place(Cell childCell, Cell parentCell)
        {
            ConnectionPoint connectionPoint = parentCell.GetUnoccupiedConnectionPoint();

            if (connectionPoint == null)
            {
                Debug.LogError("No available connection points on parent cell");
                return;
            }

            Vector3 directionToConnectionPoint = parentCell.GetTransform().TransformDirection(connectionPoint.position.normalized);
            float distanceToMove = parentCell.gameObject.transform.localScale.x / 2.0f + childCell.gameObject.transform.localScale.x / 2.0f;

            // Transform the local position of the connection point to world coordinates
            Vector3 connectionPointWorldPosition = parentCell.GetTransform().TransformPoint(Vector3.Scale(connectionPoint.position, parentCell.GetTransform().localScale));

            Vector3 childPosition = connectionPointWorldPosition + directionToConnectionPoint * distanceToMove;
            childCell.gameObject.transform.position = childPosition;

            childCell.gameObject.transform.rotation = Quaternion.LookRotation(
                parentCell.GetTransform().TransformDirection(connectionPoint.forward),
                parentCell.GetTransform().up
            );

            connectionPoint.isOccupied = true;
        }

    }
}