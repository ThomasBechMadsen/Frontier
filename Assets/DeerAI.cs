using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum DeerState { Normal, OnEdge, Scared };

public class DeerAI : MonoBehaviour {

    public float normalSpeed;
    public float onEdgeSpeed;
    public float scaredSpeed;

    //How far the destination should be
    public float normalDestinationDistance;
    public float onEdgeDestinationDistance;
    public float scaredDestinationDistance;

    //Make destinations less predictable
    [Range(0, 1)]
    public float onEdgeDestinationSway;
    [Range(0, 1)]
    public float scaredDestinationSway;

    public float sightDistanceScare;
    public float sightDistanceOnEdge;

    //Time between surroundings check
    public float secondsBetweenSurroundingsCheck;

    public GameObject playerObject;

    private DeerState currentState = DeerState.Normal;

    private NavMeshAgent agent;

    private noiseEmitter playerNoise;

    public float onEdgeHearingDistance;
    public float scaredHearingDistance;

    public bool isDead = false;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        playerNoise = playerObject.GetComponent<noiseEmitter>();
        StartCoroutine(checkSurroundings());
	}

    public void setState(DeerState newState)
    {
        if (newState != currentState) {
            
            currentState = newState;
            findNewDestination();
            switch (currentState)
            {
                case DeerState.Normal:
                    agent.speed = normalSpeed;
                    break;
                case DeerState.OnEdge:
                    agent.speed = onEdgeSpeed;
                    break;
                case DeerState.Scared:
                    agent.speed = scaredSpeed;
                    break;
            }
            print(currentState.ToString());
        }
    }

    public void increaseState()
    {
        if (currentState < DeerState.Scared) {
            setState(currentState + 1);
        }
    }

    public void lowerState()
    {
        if (currentState > DeerState.Normal) {
            setState(currentState - 1);
        }
    }

    private void findNewDestination()
    {
        Vector3 destination = Vector3.zero;
        float distance = 0;
        switch (currentState)
        {
            case DeerState.Normal:
                destination = transform.TransformPoint(Random.insideUnitSphere * normalDestinationDistance);
                distance = normalDestinationDistance;
                break;
            case DeerState.OnEdge:
                float swayValueOnEdge = Random.Range(-onEdgeDestinationSway, onEdgeDestinationSway);
                destination = transform.TransformPoint(new Vector3(swayValueOnEdge, 0, 1) * onEdgeDestinationDistance);
                distance = onEdgeDestinationDistance;
                break;
            case DeerState.Scared:
                float swayValueScared = Random.Range(-scaredDestinationSway, scaredDestinationSway);
                destination = transform.TransformPoint(new Vector3(swayValueScared, 0, 1) * scaredDestinationDistance);
                distance = onEdgeDestinationDistance;
                break;
        }
        NavMeshHit navHit;
        NavMesh.SamplePosition(destination, out navHit, distance, NavMesh.AllAreas);
        agent.destination = navHit.position;
    }

    private IEnumerator checkSurroundings()
    {
        if (!isDead) {

            //Check if destination has been reached
            if (agent.remainingDistance <= 1)
            {
                print("Destination reached!");
                findNewDestination();
            }

            float distance = Vector3.Distance(transform.position, playerObject.transform.position);

            RaycastHit raycastHit;
            if (Physics.Linecast(transform.position, playerObject.transform.position, out raycastHit))
            {
                if (raycastHit.transform.gameObject == playerObject)
                {
                    if (distance <= sightDistanceScare)
                    {
                        print("Sight scared");
                        setState(DeerState.Scared);
                    }
                    else if (distance <= sightDistanceOnEdge)
                    {
                        print("Sight onEdge");
                        increaseState();
                    }
                }
            }
            /* Scrapped for now
            if (distance < scaredHearingDistance + playerNoise.getNoiseValue())
            {
                print("Noise scared");
                setState(DeerState.Scared);
            }
            else if (distance < onEdgeHearingDistance + playerNoise.getNoiseValue())
            {
                print("Noise onEdge");
                increaseState();
            }
            */
            yield return new WaitForSeconds(secondsBetweenSurroundingsCheck);
            StartCoroutine(checkSurroundings());
            
        }
    }
}
