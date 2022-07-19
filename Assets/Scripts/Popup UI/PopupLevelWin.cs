using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PopupLevelWin : PopupBase
{
    public static event Action OnNextLevelButtonClicked;

    public override void onDestroyView()
    {

    }

    public override void OnHideView()
    {
    }

    public override void OnShowView()
    {
        PopupController.GetInstance().popupGamePanel.HideView();
    }

    public override void OnStart()
    {

    }

    public void NextLevelClickEvent()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int nextScene = (SceneManager.GetActiveScene().buildIndex + 1) % sceneCount;
        // OnNextLevelButtonClicked?.Invoke();
        Debug.Log("Scene Count: " + sceneCount);
        Debug.Log("Next scene: " + nextScene);
        SceneManager.LoadSceneAsync(nextScene);
        HideView();
    }
}
