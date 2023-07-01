using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domain {
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

        public Cell(String prefabName, float lifespan, Vector3 initialSize, Vector3 finalSize)
        {
            this.id = Guid.NewGuid();
            this.prefabName = prefabName;
            this.lifespan = lifespan;
            this.currentAge = 0.0f;
            this.initialSize = initialSize; // start at 10% of the final size
            this.finalSize = finalSize; // end at 100% of the final size
        }

        // TODO: pass position and orientation for the cell
        public void InstantiatePrefab()
        {
            GameObject prefab = Resources.Load<GameObject>(prefabName);
            gameObject = GameObject.Instantiate(prefab) as GameObject;
            
            // TODO: create a real name for the cell
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
}