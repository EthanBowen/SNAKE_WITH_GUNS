using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class SnakeHead : MonoBehaviour
{
    public _GameController controller;

    [SerializeField]
    private GameObject EyesCalmRef;
    [SerializeField]
    private GameObject EyesShockedRef;

    [SerializeField]
    private AudioSource AS;

    private bool dying = false;

    [Header("Movement")]

    public float baseSpeed;
    public float baseRotationSpeed;

    private float modifiedSpeed = 1.0f;
    public float speedIncreaseWithEachPart = 0.02f;

    private float rotationInput = 0.0f;

    [Space(10)]
    [Header("Jumping")]

    private bool tryingToJump = false;
    private bool jumping = false;

    public float jumpLength = 1.0f;
    public float jumpHeight = 5.0f;
    public float jumpSize = 0.5f;

    [Space(10)]
    [Header("Trailing Body")]

    private GameObject[] bodyPieces;
    [HideInInspector]
    public Vector3[] pathPositions;
    [HideInInspector]
    public Vector3[] pathRotations;
    [HideInInspector]
    public Vector3[] pathScales;
    public int pathNodesCount = 1000;

    private int currentUpdateIndex = 0;
    [HideInInspector]
    public float distanceOnPath = 0.0f;
    public float pathAccuracy = 10.0f;

    public float distanceBetweenBodySegments = 3.0f;
    private int nextBodySegmentCount = 1;


    [SerializeField]
    private GameObject SegmentEvenPrefab;
    [SerializeField]
    private GameObject SegmentOddPrefab;
    [SerializeField]
    private GameObject ExplosionPrefab;

    public int currentGunToAdd = 0;
    [SerializeField]
    private GameObject[] GunPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        EyesCalmRef.SetActive(true);
        EyesShockedRef.SetActive(false);

        bodyPieces = new GameObject[(int)(pathNodesCount/distanceBetweenBodySegments)];

        pathPositions = new Vector3[pathNodesCount];
        pathRotations = new Vector3[pathNodesCount];
        pathScales = new Vector3[pathNodesCount];

        for (int i = 0; i < pathPositions.Length; i++)
        {
            pathPositions[i] = transform.position;
            pathRotations[i] = transform.eulerAngles;
            pathScales[i] = transform.localScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!dying)
        {

            // Update Position every frame
            transform.position += transform.right * modifiedSpeed * baseSpeed * Time.deltaTime;

            // Update Rotation every frame
            //if (!jumping)
            //{
                transform.eulerAngles += new Vector3(0, 0, (rotationInput * modifiedSpeed * baseRotationSpeed * Time.deltaTime));
            //}

            // Attempt to jump every frame
            if (tryingToJump && !jumping)
            {
                jumping = true;
                StartCoroutine(JumpingMovement());
            }

            // Keep track of how far the head has moved
            distanceOnPath += modifiedSpeed * baseSpeed * Time.deltaTime * pathAccuracy;
            while (distanceOnPath >= pathPositions.Length)
            {
                distanceOnPath -= pathPositions.Length;
            }

            if (currentUpdateIndex != Mathf.FloorToInt(distanceOnPath))
            {
                int goalIndex = Mathf.FloorToInt(distanceOnPath);
                if (goalIndex < currentUpdateIndex)
                {
                    goalIndex += pathPositions.Length;
                }

                float gapSize = goalIndex - currentUpdateIndex;

                Vector3 lastUpdatePosition = pathPositions[currentUpdateIndex];
                Vector3 lastUpdateRotation = pathRotations[currentUpdateIndex];
                Vector3 lastUpdateScale = pathScales[currentUpdateIndex];

                Vector3 vectorToCurrentPosition = transform.position - lastUpdatePosition;
                Vector3 vectorToCurrentRotation = transform.eulerAngles - lastUpdateRotation;
                Vector3 vectorToCurrentScale = transform.localScale - lastUpdateScale;

                for (int i = 1; i <= gapSize; i++)
                {
                    currentUpdateIndex++;
                    if (currentUpdateIndex >= pathPositions.Length)
                    {
                        currentUpdateIndex = 0;
                    }

                    pathPositions[currentUpdateIndex] = lastUpdatePosition + (vectorToCurrentPosition * (i / gapSize));
                    pathRotations[currentUpdateIndex] = lastUpdateRotation + (vectorToCurrentRotation * (i / gapSize));
                    pathScales[currentUpdateIndex] = lastUpdateScale + (vectorToCurrentScale * (i / gapSize));
                }
            }
        }
    }

    void CreateBodySegment()
    {
        GameObject part;
        if(nextBodySegmentCount % 2 == 0)
        {
            part = Instantiate(SegmentEvenPrefab);
        }
        else 
        {
            part = Instantiate(SegmentOddPrefab);
        }

        bodyPieces[nextBodySegmentCount-1] = part;

        if(nextBodySegmentCount < 5)
        {
            part.GetComponent<CapsuleCollider>().enabled = false;
        }

        SpriteRenderer spriteRenderer = part.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.rendererPriority = -nextBodySegmentCount;
        }
        
        BodyPart bodyPart = part.GetComponent<BodyPart>();

        if (bodyPart != null)
        {
            bodyPart.headRef = this;
            bodyPart.followDistance = nextBodySegmentCount * distanceBetweenBodySegments;

            bodyPart.SetPositionOnPath();

            nextBodySegmentCount++;
        }

        if(GunPrefabs != null && GunPrefabs.Length > 0)
        {
            if (currentGunToAdd >= GunPrefabs.Length) { currentGunToAdd = 0; }
            GameObject gun = Instantiate(GunPrefabs[currentGunToAdd], bodyPart.transform.position + (Vector3.forward * -0.05f), bodyPart.transform.rotation);

            bodyPart.gun = gun.GetComponent<Gun>();

            gun.transform.parent = bodyPart.transform;
        }
    }

    void IncreaseSpeed()
    {
        modifiedSpeed += speedIncreaseWithEachPart;
    }

    //**************
    // MOVEMENTSTUFF
    //**************

    public void RotatePlayer(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        rotationInput = -input.x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        tryingToJump = context.performed;
    }

    IEnumerator JumpingMovement()
    {
        float timeElapsed = 0.0f;
        while(timeElapsed < jumpLength)
        {
            timeElapsed += Time.deltaTime * modifiedSpeed;

            float jumpProgress = Mathf.Sin((timeElapsed/jumpLength) * Mathf.PI);

            float newZValue = -jumpProgress * jumpHeight;
            Vector3 newScale = Vector3.one * jumpProgress * jumpSize;

            transform.position = new Vector3(transform.position.x, transform.position.y, newZValue);
            transform.localScale = Vector3.one + newScale;
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
        transform.localScale = Vector3.one;

        jumping = false;
    }

    //---------------
    // LIFE AND DEATH
    //---------------

    IEnumerator Die()
    {
        EyesCalmRef.SetActive(false);
        EyesShockedRef.SetActive(true);

        dying = true;

        if(AS != null)
        {
            AS.Play();
        }

        float rotationAnimation = 0.0f;

        float startingAngle = transform.rotation.eulerAngles.z;
        while(rotationAnimation < 2.0f )
        {
            rotationAnimation += Time.deltaTime;

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, startingAngle + Mathf.Sin(rotationAnimation*Mathf.PI*3.0f) * 30.0f);
            yield return null;
        }

        //yield return new WaitForSeconds(2.0f);

        StartCoroutine(Explode());
        if(controller != null )
        {
            controller.GameOver();
        }
    }

    IEnumerator Explode()
    {
        { // Spawn explosion at player
            GameObject obj = Instantiate(ExplosionPrefab, transform.position, transform.rotation);
            obj.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Random.Range(0.0f, 360.0f));
            obj.transform.localScale = obj.transform.localScale.x * transform.localScale;

            ExplosionController explosionController = obj.GetComponent<ExplosionController>();
            if(explosionController)
            {
                explosionController.volume = Random.Range(0.8f, 1.0f);
            }
        }

        SpriteRenderer[] SRs = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer sr in SRs)
        {
            sr.enabled = false;
        }
        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(.2f);

        int index = 0;
        while (index < nextBodySegmentCount - 1 )
        {
            // Spawn explosion at body part
            GameObject obj = Instantiate(ExplosionPrefab, bodyPieces[index].transform.position, bodyPieces[index].transform.rotation);
            obj.transform.eulerAngles = new Vector3(bodyPieces[index].transform.eulerAngles.x, bodyPieces[index].transform.eulerAngles.y, Random.Range(0.0f, 360.0f));
            obj.transform.localScale = obj.transform.localScale.x * bodyPieces[index].transform.localScale;

            Destroy(bodyPieces[index]);

            yield return new WaitForSeconds(.2f);

            index++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Food food = other.GetComponent<Food>();
        if (food != null)
        {
            currentGunToAdd = food.gun;
            CreateBodySegment();
            IncreaseSpeed();
            controller.ModifyScore(100);
            food.Move();
        }
        else
        {
            if(!dying)
            {
                if (other.tag.Equals("Kill"))
                {
                    dying = true;
                    StartCoroutine(Die());
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!dying)
        {
            if (collision.gameObject.tag.Equals("Enemy"))
            {
                dying = true;
                StartCoroutine(Die());
            }
        }
    }
}
