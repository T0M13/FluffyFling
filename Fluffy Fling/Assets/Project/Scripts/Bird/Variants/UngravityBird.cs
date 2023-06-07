using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UngravityBird : Bird
{
    [Header("Ability Stats")]
    [SerializeField] private float ungravityForce;
    [SerializeField] private float ungravityRadius = 4;
    [SerializeField] private Vector3 ungravityOffset;
    [SerializeField] private Collider[] colliders;
    [SerializeField] private GameObject abilityEffect;
    public override void ActivateAbility(Vector2 _)
    {
        if (state != BirdState.Thrown) return;
        if (abilityActivated) return;
        if (hasCollided) return;
        abilityActivated = true;
        Ability();
    }

    public override void AutomaticAbility()
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
            AudioManager.instance.Play("ungravityAbility");
        
        abilityEffect.transform.position = transform.position + ungravityOffset;
        abilityEffect.SetActive(true);
        abilityEffect.transform.SetParent(null);

        colliders = Physics.OverlapSphere(transform.position + ungravityOffset, ungravityRadius);
        foreach (Collider collider in colliders)
        {
            if (collider == coll) continue;
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.AddForce(Vector3.up * ungravityForce, ForceMode.Impulse);
            }
        }
    }

    protected override void AfterAbility()
    {
        //foreach (Collider collider in colliders)
        //{
        //    if (collider == null) continue;
        //    Rigidbody rb = collider.GetComponent<Rigidbody>();
        //    if (rb != null)
        //    {
        //        rb.useGravity = true;
        //    }
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + ungravityOffset, ungravityRadius);
    }


}
