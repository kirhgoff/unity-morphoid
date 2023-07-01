using System;
using UnityEngine;

namespace Domain {
    public class ConnectionPoint
    {
        public Guid id { get; private set; }
        public Vector3 position { get; set; }
        public Vector3 forward { get; set; }
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