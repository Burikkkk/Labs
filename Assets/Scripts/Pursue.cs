using UnityEngine;

public class Pursue : Seek
{
    public float maxPrediction; //��� ����, ��� ������ ����� ������������� ��������� ����
    private GameObject targetAux; //������ �� �������� ����
    private Agent targetAgent; //�������� ����

    public override void Awake()
    {
        base.Awake();
        targetAgent = target.GetComponent<Agent>();
        targetAux = target;
        target = new GameObject(); //����������� ���� ��� �������� � ���
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

        if (speed <= distance / maxPrediction) //��� ����� �������� ��������, �� ������ ������������ ������������
            prediction = maxPrediction;
        else
            prediction = distance / speed; //���� ����� �������� ������, �� ������ ����� ������ ������������


        //���������� ����������� ����
        target.transform.position = targetAux.transform.position;
        target.transform.position += targetAgent.velocity * prediction;
        
        return base.GetSteering();
    }
}
