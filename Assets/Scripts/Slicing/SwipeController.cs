using System;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    public static Action OnUpSwipe;
    public static Action OnDownSwipe;
    public static Action OnRightSwipe;
    public static Action OnLeftSwipe;


    [SerializeField] private Transform bladeObj;
    [SerializeField] private Blade blade;

    private Vector3 touchStartPos = Vector3.zero;

    private bool canStartSwipe = false;

    private bool isSlicing = false;

    private Vector3 playerPos;

    // void OnEnable()
    // {
    //     LevelGenerator.OnLevelGenerated += CanStartSwipe;
    //     PopupLevelLost.OnRestartlButtonClicked += CanStartSwipe;
    //     PopupLevelWin.OnNextLevelButtonClicked += CanStartSwipe;
    // }

    // void OnDisable()
    // {
    //     LevelGenerator.OnLevelGenerated -= CanStartSwipe;
    //     PopupLevelLost.OnRestartlButtonClicked -= CanStartSwipe;
    //     PopupLevelWin.OnNextLevelButtonClicked -= CanStartSwipe;
    // }

    void Start()
    {

    }

    void CanStartSwipe()
    {
        canStartSwipe = !canStartSwipe;
    }
    void Update()
    {
        // if (!canStartSwipe)
        //     return;

        if (Input.GetMouseButtonDown(0))
        {
            blade.transform.rotation = Quaternion.Euler(180f, 0f, 0f);
            this.touchStartPos = Input.mousePosition;

            isSlicing = true;

            Quaternion rotation = Quaternion.LookRotation(-transform.forward, Input.mousePosition.normalized);

            blade.transform.rotation = Quaternion.Euler(rotation.x, (Input.mousePosition.x > 0) ? 45f : -45f, rotation.z);
        }

        if (Input.GetMouseButtonUp(0))
        {
            blade.MakeTrailEffectOnOrOff(false);
            blade.MakeColliderOnOrOff(false);
            blade.MakeWeaponGfxOnOrOff(false);
            isSlicing = false;

            // float dirX = Input.mousePosition.x - touchStartPos.x;
            // SetAngle(Input.mousePosition, dirX);

            Quaternion rotation = Quaternion.LookRotation(-transform.forward, (Input.mousePosition - touchStartPos).normalized);
            blade.transform.rotation = rotation;
        }

        if (isSlicing)
        {
            blade.MakeTrailEffectOnOrOff(true);
            blade.MakeColliderOnOrOff(true);
            blade.MakeWeaponGfxOnOrOff(true);

            float dirX = Input.mousePosition.x - touchStartPos.x;
            SetAngle(Input.mousePosition, dirX);
            blade.SetTrailPos();

            Quaternion rotation = Quaternion.LookRotation(-transform.forward, (Input.mousePosition - touchStartPos).normalized);
            blade.transform.rotation = Quaternion.Slerp(blade.transform.rotation, rotation, 100 * Time.deltaTime);
        }
    }

    private void SetAngle(Vector3 currentTouchPos, float directionX)
    {

        Vector2 direction = currentTouchPos - touchStartPos;
        //Debug.LogError("direction: " + direction);

        Vector2 normalised = direction.normalized;
        //Debug.LogError("normalised: " + normalised);

        float angleBetween = Vector2.Angle(Vector2.up, direction);
        // Debug.LogError("angleBetween: " + angleBetween);

        if (normalised.x > 0)
            blade.SetAngle(angleBetween);

        else
            blade.SetAngle(-angleBetween);


    }


}
