using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Ball>(out Ball ball))
        {
            ball.transform.position = target.position;
        }
    }
}
