using System;

public class BodyPart
{
    public String name;
    public float relativeTimeToAppear; 
    public Guid parentCellId; 
    public Cell cell; // TODO: use a builder instead
}
