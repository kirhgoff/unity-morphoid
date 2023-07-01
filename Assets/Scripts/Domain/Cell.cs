using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Domain
{
    public class Cell
    {
        // Cell properties
        public Guid id { get; set; }
        public string prefabName { get; set; }
        public float lifespan { get; set; }
        public float currentAge { get; set; }

        // Geometry
        public List<ConnectionPoint> connectionPoints { get; private set; }
        public Vector3 initialSize { get; set; }
        public Vector3 finalSize { get; set; }

        // Unity
        public GameObject gameObject { get; set; }

        public Cell(String prefabName, float lifespan, Vector3 initialSize, Vector3 finalSize)
        {
            this.id = Guid.NewGuid();
            this.prefabName = prefabName;
            this.lifespan = lifespan;
            this.currentAge = 0.0f;
            this.connectionPoints = new List<ConnectionPoint>();
            this.initialSize = initialSize; // start at 10% of the final size
            this.finalSize = finalSize; // end at 100% of the final size

            // Just for the test
            InitializeCubeConnectionPoints();
        }

        // TODO: pass position and orientation for the cell
        public void InstantiatePrefab()
        {
            GameObject prefab = Resources.Load<GameObject>(prefabName);
            gameObject = GameObject.Instantiate(prefab) as GameObject;

            // TODO: create a real name for the cell
            gameObject.name = "Cell#" + id.ToString();
            // TODO: do this later when it is added to organism
            gameObject.transform.localScale = initialSize;
            gameObject.transform.position = Vector3.zero;
        }

        public void UpdateSize()
        {
            float factor = currentAge / lifespan;

            Vector3 newSize = Vector3.Lerp(initialSize, finalSize, factor);
            gameObject.transform.localScale = newSize;
        }

        public ConnectionPoint GetUnoccupiedConnectionPoint()
        {
            return connectionPoints.FirstOrDefault(point => !point.isOccupied);
        }

        public void InitializeCubeConnectionPoints()
        {
            void AddConnectionPoint(float x, float y, float z, Vector3 direction)
            {
                connectionPoints.Add(new ConnectionPoint(new Vector3(x, y, z), direction));
            }

            AddConnectionPoint(0, 0, 0.5f, Vector3.forward);
            AddConnectionPoint(0, 0, -0.5f, Vector3.back);
            AddConnectionPoint(0.5f, 0, 0, Vector3.right);
            AddConnectionPoint(-0.5f, 0, 0, Vector3.left);
            AddConnectionPoint(0, 0.5f, 0, Vector3.up);
            AddConnectionPoint(0, -0.5f, 0, Vector3.down);
        }
    }
}