using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatterables : DestructableObject, IDestructable
{
    public void DestroyObj()
    {
        Shatter(this.gameObject, data.totalCuts);
    }

    private void Shatter(GameObject obj, int genObjs)
    {
        while (genObjs != 0)
        {

            GameObject[] pieces = MeshManipulation.MeshCut.Cut(obj, obj.transform.position, GetAngle(obj, genObjs),
                                data.insideMaterial);

            for (int i = 0; i < pieces.Length; i++)
            {
                int layerToIgnore = LayerMask.NameToLayer("Shatterables");
                pieces[i].layer = layerToIgnore;

                Rigidbody rb = pieces[i].AddComponent<Rigidbody>();
                pieces[i].AddComponent<MeshCollider>().convex = true;

                int val = Random.Range(0, 3);

                Vector3 forceVec = Vector3.zero;

                if (val == 0)
                    forceVec = Vector3.forward;

                else if (val == 1)
                    forceVec = Vector3.right;

                else if (val == 2) ;
                forceVec = Vector3.left;


                rb.AddForce(forceVec * data.shatterForce * Time.fixedDeltaTime, ForceMode.Impulse);
            }

            genObjs--;
        }

        Destroy(this.gameObject);
    }

    private Vector3 GetAngle(GameObject obj, int genObjs)
    {
        Quaternion q = Quaternion.Euler(Random.Range(-40, 40), Random.Range(-40, 40), Random.Range(-40, 40));

        Vector3[] faces = { obj.transform.forward, obj.transform.right, obj.transform.up };

        return q * faces[genObjs % 3];
    }
}


