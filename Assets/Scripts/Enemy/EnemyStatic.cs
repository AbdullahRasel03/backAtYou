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

                Shoot();

                currentState = EnemyState.Idle;

                break;

            case EnemyState.Dead:
                Destroy(instantiatedHealthBar.gameObject);
                Destroy(this.gameObject);
                break;
        }
    }
}
