using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class PopupLevelLost : PopupBase
{
    public static event Action OnLevelFailed;

    private string titleString = "";
    public override void onDestroyView()
    {

    }
    public override void OnHideView()
    {

    }

    public override void OnShowView()
    {
        PopupController.GetInstance().popupGamePanel.HideView();
        OnLevelFailed?.Invoke();
    }

    public override void OnStart()
    {

    }

    public void RestartClickEvent()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        HideView();
    }
}
