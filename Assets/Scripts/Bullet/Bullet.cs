using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IDeflectable
{
    [SerializeField] private BulletData bulletData;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private MeshRenderer mesh;
    private float currentBulletSpeed;
    private bool isDeflecting;
    private LevelController levelController;
    private Vector3 movement = Vector3.zero;
    private Vector3 playerPos;
    private Vector3 enemeyPos;
    private IEnumerator removeCoRoutine;

    public static event Action<GameObject, GameObject> OnSlowMotionStarted;

    void Start()
    {
        levelController = LevelController.GetInstance();
        playerPos = levelController.GetPlayerPos().position;
    }

    private IEnumerator RemoveIfNotHit()
    {
        yield return new WaitForSeconds(5f);
        ObjectPoolUtil.GetInstance().ReturnObj(this.gameObject);
    }

    void OnEnable()
    {
        Player.OnPlayerDead += HideBullet;
        LevelController.OnLevelWin += HideBullet;

        currentBulletSpeed = bulletData.bulletSpeedNormal;
        mesh.material.color = new Color(0.9245283f, 0.7159268f, 0.1788003f, 1);
        isDeflecting = false;
    }

    void OnDisable()
    {
        Player.OnPlayerDead -= HideBullet;
        LevelController.OnLevelWin -= HideBullet;
    }

    private void HideBullet()
    {
        ObjectPoolUtil.GetInstance().ReturnObj(this.gameObject);
    }

    void Update()
    {
        CheckDistanceToPlayer();
    }

    void FixedUpdate()
    {
        rb.velocity = transform.forward * currentBulletSpeed * Time.fixedDeltaTime;
    }

    private void CheckDistanceToPlayer()
    {
        float distanceZ = Mathf.Abs(this.transform.position.z - playerPos.z);

        if (distanceZ <= 3.25f && distanceZ >= 2.1f && !isDeflecting)
        {
            currentBulletSpeed = bulletData.bulletSpeedSlow;
            mesh.material.color = Color.red;
        }

        else if (isDeflecting || (distanceZ > 3.25f || distanceZ < 2.1f))
        {
            currentBulletSpeed = bulletData.bulletSpeedNormal;
            mesh.material.color = new Color(0.9245283f, 0.7159268f, 0.1788003f, 1);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        rb.velocity = Vector3.zero;
        IDamageable dmg = other.gameObject.GetComponent<IDamageable>();
        IDestructable des = other.gameObject.GetComponent<IDestructable>();
        IKnockable knk = other.gameObject.GetComponent<IKnockable>();

        if (dmg != null)
        {
            isDeflecting = false;
            dmg.Damage(bulletData.damage, transform.position);
            ObjectPoolUtil.GetInstance().ReturnObj(this.gameObject);
        }

        else if (des != null)
        {
            isDeflecting = false;
            des.DestroyObj();
            ObjectPoolUtil.GetInstance().ReturnObj(this.gameObject);
        }

        else if (knk != null)
        {
            isDeflecting = false;
            knk.KnockDownObj(transform.position);
            ObjectPoolUtil.GetInstance().ReturnObj(this.gameObject);
        }
    }

    public void SetEnemyPos(Vector3 val)
    {
        enemeyPos = val;
    }

    public void Deflect(float angle)
    {
        if (removeCoRoutine != null)
        {
            StopCoroutine(removeCoRoutine);
        }

        this.removeCoRoutine = RemoveIfNotHit();
        StartCoroutine(removeCoRoutine);

        isDeflecting = true;

        currentBulletSpeed = bulletData.bulletSpeedDeflect;
        mesh.material.color = new Color(0.9245283f, 0.7159268f, 0.1788003f, 1);

        transform.eulerAngles = new Vector3(0, angle, 0);

        if (levelController.GetEnemyCount() == 1)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 30, bulletData.layerMask))
            {
                Enemy enemy = hit.transform.GetComponent<Enemy>();

                if (enemy.GetCurrentHealth() <= bulletData.damage)
                {
                    OnSlowMotionStarted.Invoke(this.gameObject, enemy.gameObject);
                }
            }
        }




    }
}
