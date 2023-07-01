using System;
using UnityEngine;
using Domain;

public class GrowingOrganism: MonoBehaviour
{
    private Organism organism;

    void Start()
    {
        var bodyPlan = new BodyPlanBuilder()
            .WithLifespan(100f)
            .AddRoot("body", 0.0f, new Cell("Cube", 100f, Vector3.one * 0.1f, Vector3.one))
            .Build();

        organism = new Organism(bodyPlan);
        Debug.Log("Organism created");
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Updating organism");
        organism.Update();
    }
}
