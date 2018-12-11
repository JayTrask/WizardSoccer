using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Curse : NetworkBehaviour
{

    public Vector3 startingVelocity;

    bool destroy = false;

    public GameObject explosion;

    private void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var accuracy = hit.GetComponent<Accuracy>();

        if (accuracy != null)
        {
            accuracy.reduceAccuracy(10);
        }

        destroy = true;
        CmdOnImpact();
    }

    private void LateUpdate()
    {
        if (destroy)
        {
            Destroy(gameObject);
        }
    }

    public virtual void Launch()
    {
        GetComponent<Rigidbody>().velocity = startingVelocity;
    }

    [Command]
    public virtual void CmdOnImpact()
    {
        if (explosion != null)
        {
            GameObject tempExplosion = Instantiate(explosion);
            tempExplosion.transform.position = transform.position;
            NetworkServer.Spawn(tempExplosion);
        }


    }

}

