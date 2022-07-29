using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatic : Enemy
{
    [SerializeField] protected Collider mainCollider;
    private float currentTime = 0f;

    private Collider[] bodyColliders;
    private Rigidbody[] bodyRigidBodies;

    protected override void Start()
    {
        base.Start();
        bodyColliders = GetComponentsInChildren<Collider>();
        bodyRigidBodies = GetComponentsInChildren<Rigidbody>();
        DeactiveRagDoll();
    }

    protected override void CheckState()
    {
        switch (currentState)
        {
            case EnemyState.Initial:

                currentTime += Time.deltaTime;

                if (currentTime >= Random.Range(0.5f, 1.5f))
                {
                    currentState = EnemyState.Shoot;
                    currentTime = 0f;
                }

                break;

            case EnemyState.Idle:
                currentTime += Time.deltaTime;

                if (currentTime >= Random.Range(enemyData.shootDelayMin, enemyData.shootDelayMax))
                {
                    currentTime = 0f;
                    currentState = EnemyState.Shoot;
                }

                break;

            case EnemyState.Moving:
                break;

            case EnemyState.Shoot:

                RotateGun();
                Shoot();

                currentState = EnemyState.Idle;

                break;

            case EnemyState.Dead:
                if (instantiatedHealthBar != null)
                    Destroy(instantiatedHealthBar.gameObject);

                Destroy(mainCollider);
                ActiveRagDoll();
                break;

            case EnemyState.End:
                break;
        }
    }

    protected override void RotateGun()
    {
        // this.randomPlayerPos = new Vector3(Random.Range(target.x - enemyData.playerXBound, target.x + enemyData.playerXBound),
        // Random.Range(target.y - (enemyData.playerYBound - 0.3f), target.y + enemyData.playerYBound), target.z);

        FindPlayerPosToLookAt();

        gun.rotation = Quaternion.LookRotation(randomPlayerPos - gun.position);
    }

    protected override void InstantiateHealthBar()
    {
        if (levelController.GetGameStartState() == false)
            return;

        GameObject obj = Instantiate(enemyData.healthBarPrefab, new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z),
        Quaternion.identity, healthBarParentCanvas);

        instantiatedHealthBar = obj.GetComponent<EnemyHealthBar>();
        currentHealth = enemyData.maxHealth;
        instantiatedHealthBar.SetMaxHealth(enemyData.maxHealth);
    }

    private void ActiveRagDoll()
    {
        Vector3 hitDirection = (transform.position - hitPosition).normalized;
        hitDirection.y = 1f;

        foreach (Collider collider in bodyColliders)
        {
            collider.enabled = true;
        }

        foreach (Rigidbody rb in bodyRigidBodies)
        {
            rb.isKinematic = false;

            rb.AddForce(hitDirection * enemyData.deadKnockOutForce * Time.fixedDeltaTime, ForceMode.Impulse);
        }

        currentState = EnemyState.End;
    }

    private void DeactiveRagDoll()
    {
        mainCollider.enabled = true;

        for (int i = 1; i < bodyColliders.Length; i++)
        {
            bodyColliders[i].enabled = false;
        }

        foreach (Rigidbody rb in bodyRigidBodies)
        {
            rb.isKinematic = true;
        }
    }


}
