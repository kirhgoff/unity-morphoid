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
            .AddPart("head", "body", 0.2f, new Cell("Cube", 10f, Vector3.one * 0.1f, Vector3.one))
            .AddPart("hand1", "body", 0.3f, new Cell("Cube", 10f, Vector3.one * 0.1f, Vector3.one / 1.2f))
            .AddPart("leg1", "body", 0.4f, new Cell("Cube", 10f, Vector3.one * 0.1f, Vector3.one / 1.2f))
            .AddPart("hand2", "body", 0.5f, new Cell("Cube", 10f, Vector3.one * 0.1f, Vector3.one / 1.2f))
            .AddPart("leg2", "body", 0.6f, new Cell("Cube", 10f, Vector3.one * 0.1f, Vector3.one / 1.2f))
            .Build();

        organism = new Organism(bodyPlan);
    }

    void Update()
    {
        organism.Update();
    }
}
