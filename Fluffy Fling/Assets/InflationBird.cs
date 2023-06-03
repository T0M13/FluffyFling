using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InflationBird : Bird
{

    [Header("Inflation Ability")]
    [SerializeField] private Vector3 inflationSize = new Vector3(2.5f, 2.5f, 2.5f);
    [SerializeField] private float newMass = 30;

    protected override void BeforeAbility()
    {
        inflationSize = new Vector3(2.5f, 2.5f, 2.5f);
    }

    public override void ActivateAbility(Vector2 _)
    {
        if (state != BirdState.Thrown) return;
        if (abilityActivated) return;
        if (hasCollided) return;
        Debug.Log("Activating Ability");
        abilityActivated = true;
        Ability();
    }

    public override void AutomaticAbility()
    {
        if (state != BirdState.Thrown) return;
        if (abilityActivated) return;
        if (hasCollided) return;
        Debug.Log("Activating Ability");
        abilityActivated = true;
    }

    private void Ability()
    {
        transform.localScale = inflationSize;
        Body.mass = newMass;
    }

}
