using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosives : DestructableObject, IDestructable
{
    [SerializeField] private GameObject explosiveGfx;
    [SerializeField] private ParticleSystem explosionParticle;
    public void DestroyObj()
    {
        explosiveGfx?.SetActive(false);
        explosionParticle?.Play();
        DamageThings();
    }

    private void DamageThings()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, data.explosionRadius);

        foreach (Collider enemy in enemies)
        {
            IDamageable damageable = enemy.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(data.damageDeal);
            }
        }

        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, data.explosionRadius);
    }
}
