using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBird : Bird
{
    [Header("Ghost Ability")]
    [SerializeField] private int birdLayer = 6;
    [SerializeField] private int obstacleLayer = 8;

    protected override void AfterAbility()
    {
        Physics.IgnoreLayerCollision(birdLayer, obstacleLayer, false);
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
        Physics.IgnoreLayerCollision(birdLayer, obstacleLayer, true);
    }

}
