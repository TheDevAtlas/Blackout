using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public Transform target;
    public float chaseSpeed;
    public float minSpeed;
    public float maxSpeed;
    public float difficulty = 10f;
    public bool isMove;
    public float moveCooldown = 5f;

    public bool stronk;

    public enum State
    {
        forrest,
        patrol,
        interest,
        window,
        house,
        chase
    }

    public State state = State.patrol;

    private void Start()
    {
        FindNewPatrolPoint();
    }

    void House()
    {
            GameObject[] listGame = GameObject.FindGameObjectsWithTag("House");

            // Array to store the closest game objects
            GameObject[] closestObjects = new GameObject[3];

            // Array to store the corresponding distances
            float[] closestDistances = new float[3] { float.MaxValue, float.MaxValue, float.MaxValue };

            // Iterate through the objects array
            foreach (GameObject obj in listGame)
            {
                // Calculate the distance between the object and the player
                float distance = Vector3.Distance(obj.transform.position, transform.position);

                // Check if the distance is smaller than any of the stored closest distances
                for (int i = 0; i < closestDistances.Length; i++)
                {
                    if (distance < closestDistances[i])
                    {
                        // Shift the previous closest objects and distances down the array
                        for (int j = closestDistances.Length - 1; j > i; j--)
                        {
                            closestObjects[j] = closestObjects[j - 1];
                            closestDistances[j] = closestDistances[j - 1];
                        }

                        // Store the current object and distance at the correct index
                        closestObjects[i] = obj;
                        closestDistances[i] = distance;

                        break;
                    }
                }
            }
            target = closestObjects[Random.Range(0, closestDistances.Length)].transform;
            transform.position = target.position;
            isMove = false;
    }

    void GoIntoWindow()
    {
        WindowController window = target.gameObject.GetComponentInParent<WindowController>();

        if (!window.isBoarded || stronk)
        {
            window.isBoarded = false;
            state = State.house;
            Invoke("House", moveCooldown);

            GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject obj in enemy)
            {
                obj.GetComponent<EnemyLogic>().state = State.interest;
                this.enabled = false;
            }
        }
        else
        {
            state = State.patrol;
            Invoke("FindNewPatrolPoint", moveCooldown);
        }
    }

    private void FindNewInterestPoint()
    {
            GameObject[] listGame = GameObject.FindGameObjectsWithTag("Window");

            // Array to store the closest game objects
            GameObject[] closestObjects = new GameObject[3];

            // Array to store the corresponding distances
            float[] closestDistances = new float[3] { float.MaxValue, float.MaxValue, float.MaxValue };

            // Iterate through the objects array
            foreach (GameObject obj in listGame)
            {
                // Calculate the distance between the object and the player
                float distance = Vector3.Distance(obj.transform.position, transform.position);

                // Check if the distance is smaller than any of the stored closest distances
                for (int i = 0; i < closestDistances.Length; i++)
                {
                    if (distance < closestDistances[i])
                    {
                        // Shift the previous closest objects and distances down the array
                        for (int j = closestDistances.Length - 1; j > i; j--)
                        {
                            closestObjects[j] = closestObjects[j - 1];
                            closestDistances[j] = closestDistances[j - 1];
                        }

                        // Store the current object and distance at the correct index
                        closestObjects[i] = obj;
                        closestDistances[i] = distance;

                        break;
                    }
                }
                target = closestObjects[0].transform;
                    isMove = true;
            if(Vector3.Distance(target.position, transform.position) < 0.75f)
            {
                Invoke("GoIntoWindow", moveCooldown);
            }
                    
        }
    }

    private void FindNewPatrolPoint()
    {
        float roll = Random.Range(0f, 20f);
        if (roll < difficulty)
        {
            GameObject[] listGame = GameObject.FindGameObjectsWithTag("Patrol");

            // Array to store the closest game objects
            GameObject[] closestObjects = new GameObject[3];

            // Array to store the corresponding distances
            float[] closestDistances = new float[3] { float.MaxValue, float.MaxValue, float.MaxValue };

            // Iterate through the objects array
            foreach (GameObject obj in listGame)
            {
                // Calculate the distance between the object and the player
                float distance = Vector3.Distance(obj.transform.position, transform.position);

                // Check if the distance is smaller than any of the stored closest distances
                for (int i = 0; i < closestDistances.Length; i++)
                {
                    if (distance < closestDistances[i])
                    {
                        // Shift the previous closest objects and distances down the array
                        for (int j = closestDistances.Length - 1; j > i; j--)
                        {
                            closestObjects[j] = closestObjects[j - 1];
                            closestDistances[j] = closestDistances[j - 1];
                        }

                        // Store the current object and distance at the correct index
                        closestObjects[i] = obj;
                        closestDistances[i] = distance;

                        break;
                    }
                }
            }

            while (true)
            {
                target = closestObjects[Random.Range(0, closestDistances.Length)].transform;

                if(Vector3.Distance(target.position, transform.position) > 1f)
                {
                    isMove = true;
                    break;
                }
            }
        }
        else
        {
            if(state == State.patrol)
            {
                Invoke("FindNewPatrolPoint", moveCooldown);
            }
            else if(state == State.interest)
            {
                Invoke("FindNewInterestPoint", moveCooldown);
            }
            else if (state == State.forrest)
            {
                Invoke("FindNewForrestPoint", moveCooldown);
            }
        }
    }

    private void FindNewForrestPoint()
    {
        float roll = Random.Range(0f, 20f);
        if (roll < difficulty)
        {
            GameObject[] listGame = GameObject.FindGameObjectsWithTag("Forrest");

            // Array to store the closest game objects
            GameObject[] closestObjects = new GameObject[3];

            // Array to store the corresponding distances
            float[] closestDistances = new float[3] { float.MaxValue, float.MaxValue, float.MaxValue };

            // Iterate through the objects array
            foreach (GameObject obj in listGame)
            {
                // Calculate the distance between the object and the player
                float distance = Vector3.Distance(obj.transform.position, transform.position);

                // Check if the distance is smaller than any of the stored closest distances
                for (int i = 0; i < closestDistances.Length; i++)
                {
                    if (distance < closestDistances[i])
                    {
                        // Shift the previous closest objects and distances down the array
                        for (int j = closestDistances.Length - 1; j > i; j--)
                        {
                            closestObjects[j] = closestObjects[j - 1];
                            closestDistances[j] = closestDistances[j - 1];
                        }

                        // Store the current object and distance at the correct index
                        closestObjects[i] = obj;
                        closestDistances[i] = distance;

                        break;
                    }
                }
            }

            while (true)
            {
                target = closestObjects[Random.Range(0, closestDistances.Length)].transform;

                if (Vector3.Distance(target.position, transform.position) > 1f)
                {
                    isMove = true;
                    break;
                }
            }
        }
        else
        {
            if (state == State.patrol)
            {
                Invoke("FindNewPatrolPoint", moveCooldown);
            }
            else if (state == State.interest)
            {
                Invoke("FindNewInterestPoint", moveCooldown);
            }
            else if (state == State.forrest)
            {
                Invoke("FindNewForrestPoint", moveCooldown);
            }
        }
    }

    private void Update()
    {
        if (!isMove)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            if(state == State.patrol)
            {
                isMove = false;
                Invoke("FindNewPatrolPoint", moveCooldown);
            }else if(state == State.interest)
            {
                isMove = false;
                Invoke("FindNewInterestPoint", moveCooldown);
            }
            else if (state == State.forrest)
            {
                Invoke("FindNewForrestPoint", moveCooldown);
            }
        }
    }
}
