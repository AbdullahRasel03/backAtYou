using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SlowMotionController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector3 cameraFollowOffset;
    [SerializeField] private float slowDownFactor = 0.05f;
    private bool canFollow = false;
    private bool canSlowDown = false;
    private GameObject enemyObj = null;
    private GameObject bulletObj = null;

    void Awake()
    {
        mainCamera.gameObject.SetActive(true);
    }

    void OnEnable()
    {
        Bullet.OnSlowMotionStarted += StartSlowMotion;
    }

    void OnDisable()
    {
        Bullet.OnSlowMotionStarted -= StartSlowMotion;
    }

    private void StartSlowMotion(GameObject bullet, GameObject enemy)
    {
        bulletObj = bullet;
        enemyObj = enemy;

        mainCamera.gameObject.SetActive(false);

        canFollow = true;
    }

    void Update()
    {

        if (canFollow && bulletObj != null)
        {
            transform.position = (bulletObj.transform.position + cameraFollowOffset);
        }

        if (enemyObj != null && Vector3.Distance(enemyObj.transform.position, transform.position) <= 5f)
        {
            canFollow = false;
            canSlowDown = true;
        }

        if (canSlowDown)
        {
            Time.timeScale = slowDownFactor;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            StartCoroutine(ResetSlowDown());
        }
    }



    private IEnumerator ResetSlowDown()
    {
        yield return new WaitForSeconds(1.8f);
        canSlowDown = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
