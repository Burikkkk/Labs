using UnityEngine;

public class Agent : MonoBehaviour
{

    public float maxSpeed;
    public float maxAccel;
    public float orientation; // “екущий угол поворота
    public float rotation; // —корость поворота
    public Vector3 velocity;
    protected Steering steering; // ’ранит текущее направление движени€

    void Start()
    {
        velocity = Vector3.zero;
        steering = new Steering();
    }

    public void SetSteering(Steering steering)
    {
        this.steering = steering;
    }

    public virtual void Update()
    {
        Vector3 displacement = velocity * Time.deltaTime;
        orientation += rotation * Time.deltaTime;

        // ќграничение угла (чтобы не выходил за пределы 0-360)
        if (orientation < 0.0f) orientation += 360.0f;
        else if (orientation > 360.0f) orientation -= 360.0f;

        transform.Translate(displacement, Space.World);
        transform.rotation = Quaternion.Euler(0, orientation, 0);
    }

    public virtual void LateUpdate() //движение дл€ каждого кадра
    {
        velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;

        if (velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity *= maxSpeed;
        }

        if (steering.angular == 0.0f)
            rotation = 0.0f;

        if (steering.linear.sqrMagnitude == 0.0f)
            velocity = Vector3.zero;

        steering = new Steering();
    }
}