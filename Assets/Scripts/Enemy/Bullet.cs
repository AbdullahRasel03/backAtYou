using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IDeflectable
{
    [SerializeField] private float bulletSpeedNormal = 10f;
    [SerializeField] private float bulletSpeedSlow = 1f;
    [SerializeField] private float bulletSpeedDeflect;
    [SerializeField] private float slowDownTime = 5f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private MeshRenderer mesh;

    private float currentBulletSpeed;

    private Vector3 movement = Vector3.zero;

    private LevelController levelController;
    private Vector3 playerPos;

    private Vector3 enemeyPos;

    private bool isDeflecting;

    private IEnumerator removeCoRoutine;

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
        PopupLevelWin.OnLevelWin += HideBullet;

        currentBulletSpeed = bulletSpeedNormal;
        mesh.material.color = new Color(0.9245283f, 0.7159268f, 0.1788003f, 1);
        isDeflecting = false;
    }

    void OnDisable()
    {
        Player.OnPlayerDead -= HideBullet;
        PopupLevelWin.OnLevelWin -= HideBullet;
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

        if (distanceZ <= 3.6f && distanceZ >= 2.2f && !isDeflecting)
        {
            currentBulletSpeed = bulletSpeedSlow;
            mesh.material.color = Color.red;
        }

        else if (isDeflecting || (distanceZ > 3.6f || distanceZ < 2.2f))
        {
            currentBulletSpeed = bulletSpeedNormal;
            mesh.material.color = new Color(0.9245283f, 0.7159268f, 0.1788003f, 1);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        rb.velocity = Vector3.zero;
        IDamageable dmg = other.gameObject.GetComponent<IDamageable>();
        IDestructable des = other.gameObject.GetComponent<IDestructable>();

        if (dmg != null)
        {
            isDeflecting = false;
            dmg.Damage(1);
            ObjectPoolUtil.GetInstance().ReturnObj(this.gameObject);
        }

        else if (des != null)
        {
            isDeflecting = false;
            des.DestroyObj();
            ObjectPoolUtil.GetInstance().ReturnObj(this.gameObject);
        }
    }

    public void SetEnemyPos(Vector3 val)
    {
        enemeyPos = val;
    }



    public void Deflect(float angle)
    {
        // if (slowDownCoroutine != null)
        // {
        //     StopCoroutine(slowDownCoroutine);
        // }

        if (removeCoRoutine != null)
        {
            StopCoroutine(removeCoRoutine);
        }

        this.removeCoRoutine = RemoveIfNotHit();
        StartCoroutine(removeCoRoutine);

        isDeflecting = true;

        currentBulletSpeed = bulletSpeedDeflect;
        mesh.material.color = new Color(0.9245283f, 0.7159268f, 0.1788003f, 1);
        // this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, angle, 0f), 1000 * Time.deltaTime);

        transform.eulerAngles = new Vector3(0, angle, 0);
    }
}
