using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Vector3 cameraLocation;
    
    [Header("Spawn")]
    public Spawn spawn;
    public int spawnDelay = 500;
    public Vector2 spawnVelocity;

    public async void StartLevel(IEnumerable<Ball> balls)
    {
        foreach (Ball ball in balls)
        {
            ball.gameObject.SetActive(true);
            ball.initialVelocity = spawnVelocity; // For first level
            ball.SetVelocity(spawnVelocity);
            ball.transform.position = spawn.transform.position;

            await Task.Delay(spawnDelay);
        }
    }

    public void GoalReached(Ball ball)
    {
        ball.gameObject.SetActive(false);
        GameManager.Instance.GoalReached(ball);
    }

}
