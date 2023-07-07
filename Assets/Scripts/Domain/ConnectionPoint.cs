using System;
using UnityEngine;

namespace Domain {
    // we need connection point and connection direction
    // because position could be not on axis

    public class ConnectionPoint
    {
        public Guid id { get; private set; }
        public Vector3 position { get; set; }
        public Vector3 forward { get; set; } // TODO: rename to joing direction
        public bool isOccupied { get; set; }

        public ConnectionPoint(Vector3 position, Vector3 forward)
        {
            this.id = Guid.NewGuid();
            this.position = position;
            this.forward = forward;
            this.isOccupied = false;
        }
    }
}