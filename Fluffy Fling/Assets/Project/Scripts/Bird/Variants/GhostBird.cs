using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBird : Bird
{
    [Header("Ghost Ability")]
    [SerializeField] private int birdLayer = 6;
    [SerializeField] private int obstacleLayer = 8;
    [SerializeField] private Material normalMat;
    [SerializeField] private Material ghostMat;
    [SerializeField] private SkinnedMeshRenderer matRenderer;
    [SerializeField] private GameObject ghostEffect;

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
        if (AudioManager.instance)
            AudioManager.instance.Play("ghostAbility");

        Physics.IgnoreLayerCollision(birdLayer, obstacleLayer, true);
        ghostEffect.SetActive(true);
        ghostEffect.transform.SetParent(null);
        matRenderer.material = ghostMat;
    }

}
