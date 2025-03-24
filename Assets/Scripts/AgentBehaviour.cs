using UnityEngine;
using System.Collections;

public class AgentBehaviour : MonoBehaviour
{
    public GameObject target;
    protected Agent agent;


    public virtual void Awake()
    {
        agent = gameObject.GetComponent<Agent>(); 
    }

    public virtual void Update() {

        agent.SetSteering(GetSteering()); //получает поведение(Seek, Flee)
    }

    public virtual Steering GetSteering()
    {
        return new Steering();
    }



}