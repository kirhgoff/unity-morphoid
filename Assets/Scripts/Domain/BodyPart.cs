using System;

namespace Domain {
    public class BodyPart
    {
        public String name;
        public float relativeTimeToAppear; 
        public Guid parentCellId; 
        public Cell cell;
    }
}