using System;
using UnityEngine;
using Domain;

public class GrowingOrganism: MonoBehaviour
{
    private Organism organism;

    void Start()
    {
        var bodyPlan = new BodyPlanBuilder()
            .WithLifespan(10f)
            .AddRoot("body", 0.0f, new Cell("Cube", 10f, Vector3.one * 0.1f, Vector3.one))
            .AddPart("head", "body", 0.2f, new Cell("Cube", 10f, Vector3.one * 0.05f, Vector3.one / 2f))
            .Build();

        organism = new Organism(bodyPlan);
    }

    void Update()
    {
        organism.Update();
    }
}
