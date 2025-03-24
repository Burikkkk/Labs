using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool set = false; // флаг, указывающий, был ли установлен снаряд
    private Vector3 firePos; // начальная позиция выстрела
    private Vector3 direction; // направление движения снаряда
    private float speed; // скорость снаряда
    private float timeElapsed; // время, прошедшее с момента выстрела

    void Update()
    {
        if (!set) return;

        timeElapsed += Time.deltaTime; // обновляем время полета

        // рассчитываем новую позицию с учетом начального импульса и гравитации
        transform.position = firePos + direction * speed * timeElapsed;
        transform.position += Physics.gravity * (timeElapsed * timeElapsed) / 2.0f;

        // если снаряд упал ниже определенной высоты, уничтожаем его
        if (transform.position.y < -1.0f)
            Destroy(gameObject);
    }

    // метод установки параметров полета снаряда
    public void Set(Vector3 firePos, Vector3 direction, float speed)
    {
        this.firePos = firePos; 
        this.direction = direction.normalized; 
        this.speed = speed; 
        transform.position = firePos; 
        set = true;
    }

    // метод для вычисления времени падения снаряда на заданную высоту
    public float GetLandingTime(float height = 0.0f)
    {
        Vector3 position = transform.position;

        // вычисляем дискриминант квадратного уравнения для времени падения
        float valueInt = (direction.y * direction.y) * (speed * speed) - (Physics.gravity.y * 2 * (position.y - height));
        if (valueInt < 0) return -1.0f; // если дискриминант меньше 0, решения нет

        float sqrtValue = Mathf.Sqrt(valueInt);

        // находим два возможных времени падения
        float valueAdd = (-direction.y * speed + sqrtValue) / Physics.gravity.y;
        float valueSub = (-direction.y * speed - sqrtValue) / Physics.gravity.y;

        // выбираем корректное значение времени
        if (float.IsNaN(valueAdd) && !float.IsNaN(valueSub)) return valueSub;
        if (!float.IsNaN(valueAdd) && float.IsNaN(valueSub)) return valueAdd;
        if (float.IsNaN(valueAdd) && float.IsNaN(valueSub)) return -1.0f;

        return Mathf.Max(valueAdd, valueSub);
    }

    // метод вычисления точки падения снаряда
    public Vector3 GetLandingPos(float height = 0.0f)
    {
        float time = GetLandingTime(); // получаем время падения
        if (time < 0.0f) return Vector3.zero; // если падение невозможно, возвращаем вектор (0,0,0)

        // вычисляем координаты точки приземления
        return new Vector3(
            firePos.x + direction.x * speed * time,
            height,
            firePos.z + direction.z * speed * time
        );
    }

    // статический метод вычисления направления выстрела для попадания в цель
    public static Vector3 GetFireDirection(Vector3 startPos, Vector3 endPos, float speed)
    {
        Vector3 direction = Vector3.zero; // начальное значение направления
        Vector3 delta = endPos - startPos; // разница между целевой и начальной позицией

        // коэффициенты квадратного уравнения для времени полета
        float a = Vector3.Dot(Physics.gravity, Physics.gravity);
        float b = -4 * (Vector3.Dot(Physics.gravity, delta) + speed * speed);
        float c = 4 * Vector3.Dot(delta, delta);

        // проверяем, есть ли реальные корни
        if (4 * a * c > b * b) return direction; // если нет решений, возвращаем вектор (0,0,0)

        // вычисляем два возможных времени полета
        float time0 = Mathf.Sqrt((-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a));
        float time1 = Mathf.Sqrt((-b - Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a));

        // выбираем минимальное положительное время
        float time = time0 < 0 ? time1 : (time1 < 0 ? time0 : Mathf.Min(time0, time1));

        // вычисляем направление полета
        direction = (2 * delta - Physics.gravity * (time * time)) / (2 * speed * time);
        return direction;
    }
}
