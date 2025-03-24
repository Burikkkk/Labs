using UnityEngine;

public class Arrive : AgentBehaviour
{
    public float targetRadius; // Радиус остановки (если цель в нём – останавливаемся)
    public float slowRadius;   // Радиус замедления
    public float timeToTarget = 0.1f; // Время для достижения скорости (чем меньше, тем резче торможение)

    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        Vector3 direction = target.transform.position - transform.position; // Вектор к цели
        float distance = direction.magnitude; // Расстояние до цели
        float targetSpeed;

        // Если агент в радиусе остановки — прекращаем движение
        if (distance < targetRadius)
            return steering;

        // Если далеко – движемся на максимальной скорости
        if (distance > slowRadius)
            targetSpeed = agent.maxSpeed;
        else
            targetSpeed = agent.maxSpeed * (distance / slowRadius); // Чем ближе, тем медленнее

        // Вычисляем нужную скорость
        Vector3 desiredVelocity = direction.normalized * targetSpeed;
        steering.linear = desiredVelocity - agent.velocity; // Разница между нужной и текущей скоростью
        steering.linear /= timeToTarget; // Делаем плавное изменение

        // Ограничиваем ускорение
        if (steering.linear.magnitude > agent.maxAccel)
        {
            steering.linear.Normalize();
            steering.linear *= agent.maxAccel;
        }

        return steering;
    }
}

