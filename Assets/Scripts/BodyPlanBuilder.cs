using System;
using System.Collections.Generic;

public class BodyPlanBuilder
{
    private BodyPlan bodyPlan;
    private Dictionary<string, Guid> parts;

    public BodyPlanBuilder()
    {
        bodyPlan = new BodyPlan();
    }

    // Adds a part with auto-generated ID and sets it as the parent for the next parts
    public BodyPlanBuilder AddRoot(string name, float relativeTimeToAppear, Cell cell)
    {
        if (parts.Count > 0)
        {
            throw new Exception("Root part must be added first");
        }

        cell.id = Guid.NewGuid();

        // TODO: create a constructor for BodyPart and use it here
        bodyPlan.bodyParts.Add(new BodyPart
        {
            name = name,
            relativeTimeToAppear = relativeTimeToAppear,
            parentCellId = Guid.Empty,
            cell = cell
        });
        parts[name] = cell.id;
        
        return this;
    }

    // Overload that allows adding a part with a specific parent
    public BodyPlanBuilder AddPart(string name, string parentName, float relativeTimeToAppear, Cell cell)
    {
        if (!parts.ContainsKey(parentName))
        {
            throw new Exception("Incorrect parent name");
        }

        cell.id = Guid.NewGuid();;
        bodyPlan.bodyParts.Add(new BodyPart
        {
            name = name,
            relativeTimeToAppear = relativeTimeToAppear,
            parentCellId = parts[parentName],
            cell = cell
        });
        parts[name] = cell.id; 

        return this;
    }

    public BodyPlanBuilder WithLifespan(float lifespan)
    {
        bodyPlan.lifespan = lifespan;
        return this;
    }

    public BodyPlan Build()
    {
        return bodyPlan;
    }
}
