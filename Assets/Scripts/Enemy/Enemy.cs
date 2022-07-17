using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected EnemyData enemyData;
    [SerializeField] protected Transform healthBarParentCanvas;
    [SerializeField] protected Transform gun;
    [SerializeField] protected Transform shootPoint;

    protected LevelController levelController;
    protected ObjectPoolUtil objectPool;
    protected Vector3 target;
    protected Vector3 randomPlayerPos;
    protected Transform selfTransform;

    protected EnemyHealthBar instantiatedHealthBar;

    protected bool isPlayerDead;
    protected int currentHealth;
    protected float shootStartTime;

    protected EnemyState currentState;

    #region Unity Methods
    void Start()
    {
        isPlayerDead = false;

        shootStartTime = Time.time;

        selfTransform = this.transform;
        StopAllCoroutines();

        levelController = LevelController.GetInstance();
        objectPool = ObjectPoolUtil.GetInstance();
        target = levelController.GetPlayerPos().position;

        if (levelController.GetGameStartState())
            GameStart();
    }



    void OnEnable()
    {
        Player.OnPlayerDead += PlayerDeadEvent;
        PopupMainMenu.OnGameStarted += GameStart;
    }

    void OnDisable()
    {
        Player.OnPlayerDead -= PlayerDeadEvent;
        PopupMainMenu.OnGameStarted -= GameStart;
    }



    void Update()
    {
        if (levelController.GetGameStartState() == false)
            return;

        CheckState();
    }

    #endregion

    protected void GameStart()
    {
        InstantiateHealthBar();
        currentState = EnemyState.Initial;
        //StartCoroutine(Shoot());
    }

    protected void PlayerDeadEvent()
    {
        isPlayerDead = true;
    }

    protected void Shoot()
    {
        GameObject obj = objectPool.GetObj(enemyData.bulletPrefab, null);
        obj.transform.position = shootPoint.position;
        obj.transform.rotation = shootPoint.rotation;
        obj.GetComponent<Bullet>().SetEnemyPos(this.transform.position);
    }

    // protected IEnumerator Shoot()
    // {
    //     InstantiateHealthBar();
    //     yield return new WaitForSeconds(Random.Range(0.2f, 1f));

    //     while (currentState != EnemyState.Dead && !isPlayerDead)
    //     {
    //         RotateGun();

    //         GameObject obj = objectPool.GetObj(enemyData.bulletPrefab, null);
    //         obj.transform.position = shootPoint.position;
    //         obj.transform.rotation = shootPoint.rotation;
    //         obj.GetComponent<Bullet>().SetEnemyPos(this.transform.position);
    //         yield return new WaitForSeconds(Random.Range(enemyData.fireRateMin, enemyData.fireRateMax));
    //     }
    // }



    public void Damage(int amount)
    {
        currentHealth -= amount;
        instantiatedHealthBar.ChangeHealth(currentHealth);

        if (currentHealth <= 0)
        {
            LevelController.GetInstance().RemoveEnemy();
            currentState = EnemyState.Dead;
        }
    }

    protected virtual void CheckState() { }
    protected virtual void RotateGun() { }

    protected virtual void InstantiateHealthBar() { }
}


public enum EnemyState
{
    Initial,
    Idle,
    Shoot,
    Moving,
    Dead
}
