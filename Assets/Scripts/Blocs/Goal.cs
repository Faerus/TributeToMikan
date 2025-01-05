using Unity.VisualScripting;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Level level;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Ball>(out Ball ball))
        {
            level.GoalReached(ball);
        }
    }
}
