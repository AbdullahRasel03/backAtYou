using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : Enemy
{
    [SerializeField] private List<Transform> movePoints;

    private int nextPoint = 0;
    private float currentTime = 0f;
    protected override void CheckState()
    {
        switch (currentState)
        {
            case EnemyState.Initial:

                currentTime += Time.deltaTime;

                if (currentTime >= Random.Range(0.5f, 1.5f))
                {
                    Shoot();
                    currentState = EnemyState.Moving;
                    currentTime = 0f;
                }

                break;

            case EnemyState.Idle:
                currentTime += Time.deltaTime;

                if (currentTime >= Random.Range(1f, enemyData.movingTime))
                {
                    currentTime = 0f;
                    currentState = EnemyState.Moving;
                }

                break;

            case EnemyState.Moving:
                MovingBetweenPoints();
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

    private void MovingBetweenPoints()
    {
        if (movePoints.Count == 0)
            return;


        if (transform.position == movePoints[nextPoint].position)
        {
            nextPoint++;
            if (nextPoint >= movePoints.Count)
            {
                nextPoint = 0;
            }

            currentState = EnemyState.Shoot;
        }

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoints[nextPoint].position,
                                                    enemyData.movingSpeed * Time.deltaTime);


            Vector3 healthBarPos = new Vector3(transform.position.x, instantiatedHealthBar.transform.position.y, transform.position.z);

            instantiatedHealthBar.transform.position = healthBarPos;
        }
    }
}
