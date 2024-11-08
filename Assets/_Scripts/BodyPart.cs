using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BodyPart : MonoBehaviour
{
    public SnakeHead headRef;

    public Gun gun;
    
    public float followDistance = 1.0f;

    private float distanceOnPath = 0.0f;

    public bool follow = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetPositionOnPath();
        if(gun)
        {
            gun.Shoot();
        }
    }

    public void SetPositionOnPath()
    {
        distanceOnPath = headRef.distanceOnPath - followDistance;
        while (distanceOnPath < 0.0f)
        {
            distanceOnPath += headRef.pathPositions.Length;
        }

        int index = Mathf.FloorToInt(distanceOnPath);
        float spaceBetween = distanceOnPath % 1.0f;

        Vector3 newPos;
        Vector3 newRot;
        Vector3 newSca;
        if (index == headRef.pathPositions.Length - 1)
        {
            newPos = headRef.pathPositions[index] + ((headRef.pathPositions[0] - headRef.pathPositions[index]) * spaceBetween);
            newRot = headRef.pathRotations[index] + ((headRef.pathRotations[0] - headRef.pathRotations[index]) * spaceBetween);
            newSca = headRef.pathScales[index] + ((headRef.pathScales[0] - headRef.pathScales[index]) * spaceBetween);
        }
        else
        {
            newPos = headRef.pathPositions[index] + ((headRef.pathPositions[index + 1] - headRef.pathPositions[index]) * spaceBetween);

            float zRot = headRef.pathRotations[index].z;
            if (headRef.pathRotations[index].z < 90 && headRef.pathRotations[index + 1].z > 270)
            {
                zRot += 360;
            }
            else if (headRef.pathRotations[index + 1].z < 90 && headRef.pathRotations[index].z > 270)
            {
                zRot -= 360;
            }
            Vector3 vec = new Vector3(headRef.pathRotations[index].x, headRef.pathRotations[index].y, zRot);

            newRot = vec + ((headRef.pathRotations[index + 1] - vec) * spaceBetween);

            newSca = headRef.pathScales[index] + ((headRef.pathScales[index + 1] - headRef.pathScales[index]) * spaceBetween);
        }

        transform.position = newPos;
        transform.eulerAngles = newRot;
        transform.localScale = newSca;
    }


}
