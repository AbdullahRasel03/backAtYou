using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelController : MonoBehaviour
{
    public static event Action OnLevelWin;
    public static LevelController instance;

    [SerializeField] private Player player;
    [SerializeField] private List<GameObject> enemyList;

    private bool isGameStarted;
    private void Awake()
    {
        if (LevelController.instance != null)
        {
            Destroy(this);
        }
        LevelController.instance = this;

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PopupController.GetInstance().popupMainMenu.ShowView();
            isGameStarted = false;
        }

        else
        {
            isGameStarted = true;
        }

    }

    void Start()
    {

    }

    public static LevelController GetInstance()
    {
        return LevelController.instance;
    }

    public Transform GetPlayerPos()
    {
        return this.player.transform;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public int GetEnemyCount()
    {
        return enemyList.Count;
    }


    public void RemoveEnemy()
    {
        if (enemyList.Count > 0)
        {
            enemyList.RemoveAt(enemyList.Count - 1);

            if (enemyList.Count == 0)
            {
                OnLevelWin?.Invoke();
                StartCoroutine(GameOverDelay());
            }
        }
    }

    public void SetGameStartState()
    {
        isGameStarted = true;
    }

    public bool GetGameStartState()
    {
        return isGameStarted;
    }

    private IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(1.5f);
        PopupController.GetInstance().popupLevelWin.ShowView();
    }
}
