using UnityEngine;

public class Leave : AgentBehaviour
{
    public float escapeRadius; // Радиус немедленного побега
    public float dangerRadius; // Радиус страха (если цель внутри – убегаем)
    public float timeToTarget = 0.1f; // Время для достижения скорости

    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        Vector3 direction = transform.position - target.transform.position; // Вектор от цели
        float distance = direction.magnitude;

        // Если цель слишком далеко – ничего не делаем
        if (distance > dangerRadius)
            return steering;

        float reduce; // Как сильно замедляемся
        if (distance < escapeRadius)
            reduce = 0f; // Если в радиусе побега – мчимся на максимальной скорости
        else
            reduce = distance / dangerRadius * agent.maxSpeed; // Иначе плавно снижаем скорость

        float targetSpeed = agent.maxSpeed - reduce; // Чем дальше – тем медленнее

        // Вычисляем нужную скорость
        Vector3 desiredVelocity = direction.normalized * targetSpeed;
        steering.linear = desiredVelocity - agent.velocity;
        steering.linear /= timeToTarget;

        // Ограничиваем ускорение
        if (steering.linear.magnitude > agent.maxAccel)
        {
            steering.linear.Normalize();
            steering.linear *= agent.maxAccel;
        }

        return steering;
    }
}
