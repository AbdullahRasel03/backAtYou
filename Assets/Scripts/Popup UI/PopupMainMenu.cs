using System;
using UnityEngine;

public class PopupMainMenu : PopupBase
{
    [SerializeField] private PopupGamePanel gamePanel;

    public static event Action OnGameStarted;
    public override void onDestroyView()
    {

    }

    public override void OnHideView()
    {
        gamePanel.ShowView();
    }

    public override void OnShowView()
    {
        gamePanel.HideView();
    }
    public override void OnStart()
    {

    }

    public void PlayEvent()
    {
        LevelController.GetInstance().SetGameStartState();
        OnGameStarted?.Invoke();
        HideView();
    }
}
