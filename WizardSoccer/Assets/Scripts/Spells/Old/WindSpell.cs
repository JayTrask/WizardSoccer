using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WindSpell : MultiplePointSpell {

    List<Vector3> coordinates;

    int maxPointCount = 20;
    float minDragDistance = 0.15f;
    Vector3 windDirection;

    // Use this for initialization
	
    public override void Cast()
    {
        targetShow.GetComponent<MeshRenderer>().enabled = false;

        foreach (Vector3 place in coordinates)
        {
            GameObject tempThing = Instantiate(Resources.Load("Particle Effects/WindSpell") as GameObject);
            tempThing.transform.position = place;
            tempThing.AddComponent<Tornado>();
            tempThing.GetComponent<Tornado>().windDirection = windDirection;
        }

        state = "idle";
    }


    public override void SaveCoordinates(Vector3 updatedCoordinates)
    {
        // Made this so it stops taking in points the first time maxPointCount is reached; TODO: Stop the casting mesh once the point
        // count is reached
        if ((updatedCoordinates - coordinates[coordinates.Count - 1]).magnitude > minDragDistance && coordinates.Count < maxPointCount)
        {
            //if (coordinates.Count > maxPointCount)
            //{
            //    coordinates.RemoveAt(0);
            //}

            coordinates.Add(updatedCoordinates);

        }

        Vector3 direction = coordinates[coordinates.Count - 1] - coordinates[0];
        windDirection = direction / direction.magnitude; 
    }

    public override void InitializeCoordinates()
    {
        coordinates = new List<Vector3>();
        coordinates.Add(GetWorldPoint());
        targetShow.transform.position = GetWorldPoint();
        targetShow.GetComponent<MeshRenderer>().enabled = true;
    }

    public override void AnimatePlayer()
    {
        animator.SetTrigger("Attack2");
    }

    public Vector3 GetWindDirection()
    {
        return windDirection;
    }
}
