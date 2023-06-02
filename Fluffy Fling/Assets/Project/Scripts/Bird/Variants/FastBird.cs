using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastBird : Bird
{
    [Header("Ability Stats")]
    [SerializeField] private float speedMultiplier;
    [SerializeField] private Vector3 speedLimit = new Vector3(60, 60, 0);
    [SerializeField] private Vector3 currentVelocity;

    public override void ActivateAbility(Vector2 _)
    {
        base.ActivateAbility(_);
        Ability();
    }


    private void Ability()
    {
        currentVelocity.x = Body.velocity.x * speedMultiplier;
        if (currentVelocity.x > speedLimit.x)
        {
            currentVelocity.x = speedLimit.x;
        }
        //if (currentVelocity.y > speedLimit.y)
        //{
        //    currentVelocity.y = speedLimit.y;
        //}
        Body.velocity = currentVelocity;
    }
}
