using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailEffect;
    [SerializeField] private Camera cam;
    [SerializeField] private Collider col;
    [SerializeField] private GameObject weaponsGfx;

    private float angleYAxis;

    void OnEnable()
    {
        col.enabled = false;
    }


    internal void SetTrailPos(Vector3 playerPos)
    {
        playerPos = this.transform.position;
        Vector3 mousePos = Input.mousePosition;
        this.transform.position = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 3.99f));
    }

    internal void MakeTrailEffectOnOrOff(bool val)
    {
        trailEffect.enabled = val;
    }

    internal void MakeColliderOnOrOff(bool val)
    {
        col.enabled = val;
    }

    internal void MakeWeaponGfxOnOrOff(bool val)
    {
        weaponsGfx.SetActive(val);
    }

    internal void SetAngle(float val)
    {
        angleYAxis = val;
    }

    void OnTriggerEnter(Collider other)
    {
        IDeflectable deflectable = other.gameObject.GetComponent<IDeflectable>();

        if (deflectable != null)
        {
            deflectable.Deflect(angleYAxis);
        }
    }


}
