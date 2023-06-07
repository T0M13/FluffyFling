using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastBird : Bird
{
    [Header("Ability Stats")]
    [SerializeField] private float speedMultiplier;
    [SerializeField] private Vector3 speedLimit = new Vector3(60, 60, 0);
    [SerializeField] private Vector3 currentVelocity;
    [SerializeField] private GameObject abilityEffect;
    [SerializeField] private bool boostX;
    [SerializeField] private bool boostY;

    public override void ActivateAbility(Vector2 _)
    {
        if (state != BirdState.Thrown) return;
        if (abilityActivated) return;
        if (hasCollided) return;
        abilityActivated = true;
        Ability();
    }


    private void Ability()
    {
        if (AudioManager.instance)
            AudioManager.instance.Play("fastAbility");
        
        abilityEffect.SetActive(true);
        abilityEffect.transform.SetParent(null);

        if (boostX)
        {
            currentVelocity.x = Body.velocity.x * speedMultiplier;
            if (currentVelocity.x > speedLimit.x)
            {
                currentVelocity.x = speedLimit.x;
            }
        }
        if (boostY)
        {
            currentVelocity.y = Body.velocity.y * speedMultiplier;
            if (currentVelocity.y > speedLimit.y)
            {
                currentVelocity.y = speedLimit.y;
            }
        }

        Body.velocity = currentVelocity;

    }
}
