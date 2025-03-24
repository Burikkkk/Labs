using UnityEngine;

public class Pursue : Seek
{
    public float maxPrediction; //чем выше, тем раньше агент предсказывает положение цели
    private GameObject targetAux; //ссылка на реальную цель
    private Agent targetAgent; //скорость цели

    public override void Awake()
    {
        base.Awake();
        targetAgent = target.GetComponent<Agent>();
        targetAux = target;
        target = new GameObject(); //виртуальная цель для движения к ней
    }

    void OnDestroy()
    {
        Destroy(target);
    }

    public override Steering GetSteering()
    {
        Vector3 direction = targetAux.transform.position - transform.position;
        float distance = direction.magnitude;
        float speed = agent.velocity.magnitude;
        float prediction;

        if (speed <= distance / maxPrediction) //сли агент движется медленно, он делает максимальное предсказание
            prediction = maxPrediction;
        else
            prediction = distance / speed; //Если агент движется быстро, он делает более точное предсказание


        //обновление вирутальной цели
        target.transform.position = targetAux.transform.position;
        target.transform.position += targetAgent.velocity * prediction;
        
        return base.GetSteering();
    }
}
