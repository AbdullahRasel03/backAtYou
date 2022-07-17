using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopupBase : MonoBehaviour, IDeviceBackButtonInterface
{
    [SerializeField] protected bool closeOnOutsideTap;
    void Start()
    {
        this.OnStart();
    }

    public void ShowView()
    {
        this.gameObject.SetActive(true);
        this.PushToStack();
        this.OnShowView();
        // TODO: ADD stats for SHOW
    }

    public void HideView()
    {
        this.gameObject.SetActive(false);
        this.PopFromStack();
        this.OnHideView();
        // TODO: ADD stats for HIDE
    }

    public bool IsViewVisible()
    {
        return this.gameObject.activeSelf;
    }

    public void PopupCloseButtonPress()
    {
        this.HideView();
    }

    public void OnTappedOutsidePopup()
    {
        if (this.closeOnOutsideTap)
            this.HideView();
    }

    private void OnDestroy()
    {
        this.onDestroyView();
    }

    public void PushToStack()
    {
        if (PopupController.GetInstance() != null)
        {
            PopupController.GetInstance().Push(this);
        }
    }

    public void PopFromStack()
    {
        if (PopupController.GetInstance() != null)
        {
            PopupController.GetInstance().Pop(this);
        }
    }

    public virtual void OnDeviceBackButtonPressed()
    {
        // TODO: Add SFX
        this.HideView();
    }

    public virtual bool IsPopableOnDeviceBackBtn()
    {
        return true;
    }

    public abstract void onDestroyView();
    public abstract void OnHideView();
    public abstract void OnShowView();
    public abstract void OnStart();

}
