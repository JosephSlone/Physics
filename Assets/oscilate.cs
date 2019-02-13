using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oscilate : MonoBehaviour
{

    [SerializeField] bool moveBox = true;
    [SerializeField] bool rotateBox = true;

    [SerializeField] public float maximum = 10f;
    [SerializeField] public float minimum = -10f;
    [SerializeField] public float oscilationSpeed = 0.5f;

    [SerializeField] public float maxAngle = 10.0f;
    [SerializeField] public float minAngle = 350.0f;

    [SerializeField] public float rotationSpeed = 1f;

    [SerializeField] GameObject spherePrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] AnimationCurve curve;
    [SerializeField] int objectsToGenerate = 100;
    [SerializeField] float density = 0.1f;
    [SerializeField] float maxDiameter = 1.5f;
    [SerializeField] float minDiameter = 0.1f;

    static float currentAngle = 0.0f;
    static Vector3 rotationDirection = Vector3.right;
    static float t = 0.0f;

    public List<GameObject> objects;

    private void Start()
    {
        generateObjects();
                
    }

    float curvedSize()
    {
        float r =  curve.Evaluate(UnityEngine.Random.value);
        return Mathf.Lerp(minDiameter, maxDiameter, r);
    }

    private void generateObjects()
    {
        for(int i = 0; i < objectsToGenerate; i++)
        {
            print("Spawning");
            StartCoroutine(timedSpawn());
        }
    }

    IEnumerator timedSpawn()
    {
       
        GameObject ob = Instantiate(spherePrefab, spawnPoint.position, Quaternion.identity);

        float diameter = curvedSize();        
        float volume = (4.0f / 3.0f) * Mathf.PI * Mathf.Pow((diameter / 2.0f), 3.0f);
        float mass = volume * density;

        ob.transform.localScale += new Vector3(diameter, diameter, diameter);
        ob.GetComponent<Rigidbody>().mass = mass;
        objects.Add(ob);

        yield return new WaitForSeconds(0.1f);
    }

    void Update()
    {
        if (moveBox)
        {
            // animate the position of the game object...
            transform.position = new Vector3(Mathf.Lerp(minimum, maximum, t), 0, 0);

            // .. and increase the t interpolater
            t += (oscilationSpeed/10) * Time.deltaTime;

            // now check if the interpolator has reached 1.0
            // and swap maximum and minimum so game object moves
            // in the opposite direction.
            if (t > 1.0f)
            {
                float temp = maximum;
                maximum = minimum;
                minimum = temp;
                t = 0.0f;
            }
        }

        if (rotateBox)
        {
            transform.Rotate(rotationDirection * Time.deltaTime * rotationSpeed, Space.Self);
            currentAngle = transform.localEulerAngles.x;

            if (currentAngle > 90)
            {
                currentAngle = currentAngle - 360;
            }

            if (currentAngle > maxAngle)
            {
                rotationDirection = Vector3.left;
            }

            if (currentAngle < minAngle)
            {
                rotationDirection = Vector3.right;
            }

        }

    }
}

