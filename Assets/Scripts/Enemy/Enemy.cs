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
    protected Vector3 hitPosition;

    protected bool isPlayerDead;
    protected int currentHealth;
    protected float shootStartTime;

    protected EnemyState currentState;

    #region Unity Methods

    protected virtual void Start()
    {
        levelController = LevelController.GetInstance();
        objectPool = ObjectPoolUtil.GetInstance();

        isPlayerDead = false;

        shootStartTime = Time.time;

        selfTransform = this.transform;
        StopAllCoroutines();


        target = levelController.GetPlayer().GetFPSCam().transform.position;

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
        // Debug.Log(levelController);
        if (levelController.GetGameStartState() == false)
            return;

        CheckState();
    }

    #endregion

    public int GetCurrentHealth()
    {
        return this.currentHealth;
    }

    protected void GameStart()
    {
        InstantiateHealthBar();
        currentState = EnemyState.Initial;
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

    public void Damage(int amount, Vector3 hitPos)
    {
        currentHealth -= amount;
        instantiatedHealthBar.ChangeHealth(currentHealth);
        this.hitPosition = hitPos;

        if (currentHealth <= 0)
        {
            LevelController.GetInstance().RemoveEnemy();
            currentState = EnemyState.Dead;
        }
    }

    protected void FindPlayerPosToLookAt()
    {
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
    Dead,
    End
}
