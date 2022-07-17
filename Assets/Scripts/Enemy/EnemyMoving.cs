using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : Enemy
{
    [SerializeField] private List<Transform> movePoints;

    private int nextPoint = 0;
    private float currentTime = 0f;

    private bool canShootWhileWaiting = true;
    protected override void CheckState()
    {
        switch (currentState)
        {
            case EnemyState.Initial:

                currentTime += Time.deltaTime;

                if (currentTime >= Random.Range(0.5f, 1.5f))
                {
                    currentTime = 0f;
                    this.randomPlayerPos = new Vector3(Random.Range(target.x - enemyData.playerXBound, target.x + enemyData.playerXBound),
                                            Random.Range(target.y - (enemyData.playerYBound - 0.3f), target.y + enemyData.playerYBound), target.z);

                    transform.rotation = Quaternion.LookRotation(randomPlayerPos - transform.position);
                    Shoot();
                    currentState = EnemyState.Moving;
                }

                break;

            case EnemyState.Idle:
                currentTime += Time.deltaTime;

                if (currentTime >= enemyData.movingTime / 2 && canShootWhileWaiting)
                {
                    RotateGun();
                    canShootWhileWaiting = false;
                }


                if (currentTime >= enemyData.movingTime)
                {
                    currentTime = 0f;
                    canShootWhileWaiting = true;
                    currentState = EnemyState.Moving;
                }

                break;

            case EnemyState.Moving:
                MovingBetweenPoints();
                break;

            case EnemyState.Shoot:

                RotateGun();
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

    protected override void RotateGun()
    {
        StartCoroutine(RotateEnemy());
    }

    private IEnumerator RotateEnemy()
    {
        WaitForSeconds delay = new WaitForSeconds(0.01f);


        this.randomPlayerPos = new Vector3(Random.Range(target.x - enemyData.playerXBound, target.x + enemyData.playerXBound),
        Random.Range(target.y - (enemyData.playerYBound - 0.3f), target.y + enemyData.playerYBound), target.z);

        Quaternion targetRotation = Quaternion.LookRotation(randomPlayerPos - transform.position);

        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
            yield return delay;
        }

        Shoot();
    }

    protected override void InstantiateHealthBar()
    {
        if (levelController.GetGameStartState() == false)
            return;

        GameObject obj = Instantiate(enemyData.healthBarPrefab, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z),
        Quaternion.identity, healthBarParentCanvas);

        instantiatedHealthBar = obj.GetComponent<EnemyHealthBar>();
        currentHealth = enemyData.maxHealth;
        instantiatedHealthBar.SetMaxHealth(enemyData.maxHealth);
    }
}
