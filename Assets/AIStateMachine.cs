using System.Collections;
using UnityEngine;

public class AIStateMachine : MonoBehaviour
{
    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");        
    }
    public enum State
    {
        Idle,
        Evade,
        Persecution,
        Patrol
    }

    public State currentState;

    private SteeringBehaviour str;
    private GameObject Player { get; set; }
    [Tooltip("This is the minimum distance.")]
    public float maximumDistance = 10f;
    public float contactDistance = 1f;
    public Vector3 directionToPlayer;
    public float distanceToPlayer;
    bool preparePersecution = false;


    void Start()
    {
        Debug.Log("fdsaasf");
        currentState = State.Idle; // Start in the Idle state
    }

    IEnumerator DelayedPersecution(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeState(State.Persecution);
    }

    void Update()
    {
        directionToPlayer = transform.position - Player.transform.position;
        distanceToPlayer = directionToPlayer.magnitude;
        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.Evade:
                HandleEvade();
                break;
            case State.Persecution:
                HandlePersecution();
                break;
            case State.Patrol:
                HandlePatrol();
                break;
        }
    }

    void HandleIdle()
    {
        Debug.Log("HandleIdle");

        // Idle behavior
        // Change state based on conditions
        if (distanceToPlayer <= contactDistance)
        {
            ChangeState(State.Persecution);
        }
        if (distanceToPlayer <= maximumDistance)
        {
            ChangeState(State.Evade);
        }
    }

    void HandleEvade()
    {
        Debug.Log("HandleEvade");

        // Evade behavior
        // Change state based on conditions
        str.flee(Player.transform.position);
        if (distanceToPlayer > maximumDistance && !preparePersecution)
        {
            ChangeState(State.Idle);
        }
        if (distanceToPlayer <= contactDistance)
        {
            preparePersecution = true;
            StartCoroutine(DelayedPersecution(2.0f));

        }
    }

    void HandlePersecution()
    {
        Debug.Log("HandlePersecution");
        // Persecution behavior
        // Change state based on conditions
        str.seek(Player.transform.position);
        if (distanceToPlayer <= contactDistance)
        {
            ChangeState(State.Evade);
        }
    }

    void HandlePatrol()
    {
        Debug.Log("HandlePatrol");

        // Patrol behavior
        // Change state based on conditions
        // Check the distance to the player

        if (distanceToPlayer <= maximumDistance) {
            //TODO: Raycasting
            StartCoroutine(DelayedPersecution(2.0f));
        }
    }

    // Method to change the state
    public void ChangeState(State newState)
    {
        currentState = newState;
    }
}