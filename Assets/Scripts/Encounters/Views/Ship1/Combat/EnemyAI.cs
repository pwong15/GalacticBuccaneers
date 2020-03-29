using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
    public enum State {
        Attacking,
        Moving,
    }
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public State GetCurrentState() {
        return State.Attacking;
    }
}
