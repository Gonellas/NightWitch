using UnityEngine;

public class TestEnemy : Enemy, IEnemy
{
    private bool movingRight = true; // borrable
    private float timer = 0f;// borrable
    private float speed = 1f;// borrable

    [SerializeField] private float _hp;

    [SerializeField] private GameObject coin;

    public void LoseHP(float damage)
    {
        _hp -= damage;

        if (_hp <= 0)
        {
            Instantiate(coin, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        SteeringBehaviour();
    }

    protected override void SteeringBehaviour()
    {
        // borrable todo lo que esta en el update 
        timer += Time.deltaTime;

        if (timer <= 1f)
        {
            if (movingRight)
                transform.Translate(Vector3.right * Time.deltaTime * speed);
            else
                transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        else if (timer <= 2f)
        {
            if (movingRight)
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            else
                transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        else
        {
            timer = 0f;
            movingRight = !movingRight;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spell1")
        {
            LoseHP(10);
        }
    }
}


