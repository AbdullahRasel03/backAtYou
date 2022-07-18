using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatic : Enemy
{
    private float currentTime = 0f;
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
                Destroy(instantiatedHealthBar.gameObject);
                Destroy(this.gameObject);
                break;
        }
    }

    protected override void RotateGun()
    {
        // this.randomPlayerPos = new Vector3(Random.Range(target.x - enemyData.playerXBound, target.x + enemyData.playerXBound),
        // Random.Range(target.y - (enemyData.playerYBound - 0.3f), target.y + enemyData.playerYBound), target.z);

        Vector3 dir = (target - gun.position).normalized;

        float min = 0, max = 0;

        if (dir.x > 0)
        {
            min = target.x - 0.15f;
            max = target.x + dir.x + 0.15f;
        }

        else if (dir.x < 0)
        {
            min = target.x + dir.x - 0.15f;
            max = target.x + 0.15f;
        }

        else
        {
            min = target.x - 0.25f;
            max = target.x + 0.25f;
        }

        this.randomPlayerPos = new Vector3(Random.Range(min, max), Random.Range(target.y - 0.15f, target.y - 0.7f), target.z);



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


}
