using UnityEngine;
using System.Collections;

public class Hunter : MonoBehaviour {
    GameObject player;
    UnityEngine.AI.NavMeshAgent agent;

    void Start() {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update() {
        if (agent != null) {
            agent.destination = player.transform.position;
        }
    }
}
