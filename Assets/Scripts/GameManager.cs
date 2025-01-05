using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Camera mainCamera;

    public SpriteRenderer[] leaderboard;

    public Level[] levels;
    private int currentLevelIndex;
    public int losersPerLevelCount = 1;
    private int maxWinnerForLevel;
    private readonly List<Ball> winners = new();

    [Header("Balls")]
    public GameObject ballPrefab;
    public List<Color> enteringBalls;
    private readonly List<Ball> balls = new();
    public Vector2 minVelocity;
    public Vector2 maxVelocity = Vector2.positiveInfinity;

    private void Start()
    {
        Instance = this;

        this.InstanciateBalls();
        
        this.StartLevel(levels[currentLevelIndex]);
    }

    private void Update()
    {
        // Make sure to keep all balls in current level
        foreach(Ball ball in balls)
        {
            if(!this.PointInCameraViewport(mainCamera, ball.transform.position))
            {
                ball.transform.position = levels[currentLevelIndex].spawn.transform.position;
                Debug.LogWarning($"{ball.gameObject.name} left camera viewport, resetting location to spawn.");
            }
        }
    }

    private bool PointInCameraViewport(Camera camera, Vector3 point)
    { 
        var v = camera.WorldToViewportPoint(point);
        return v.x > 0 && v.x < 1 && v.y > 0 && v.y < 1 && v.z > 0;
    }

    private void StartLevel(Level level)
    {
        mainCamera.transform.position = level.cameraLocation;
        var competingBalls = balls.Where(b => !b.hasLost).ToList();
        maxWinnerForLevel = competingBalls.Count() - losersPerLevelCount; 
        this.ResetLeaderboard();
        level.StartLevel(competingBalls);
    }

    private void InstanciateBalls()
    {
        balls.Clear();
        foreach(var color in enteringBalls)
        {
            Ball ball = this.InstantiateBall(color);
            balls.Add(ball);
        }
    }

    private Ball InstantiateBall(Color color)
    {
        GameObject gameObject = Instantiate(ballPrefab, transform);
        gameObject.name = $"Ball '{color.ToHexString()}'";
        gameObject.SetActive(false);

        Ball ball = gameObject.GetComponent<Ball>();
        ball.color = color;
        ball.minVelocity = minVelocity;
        ball.maxVelocity = maxVelocity;
        return ball;
    }

    private void ResetLeaderboard()
    {
        winners.Clear();
        foreach(var renderer in leaderboard)
        {
            renderer.color = Color.black;
        }
    }
    public void GoalReached(Ball ball)
    {
        leaderboard[winners.Count()].color = ball.color;
        winners.Add(ball);

        // Check level completion
        if(winners.Count >= maxWinnerForLevel)
        {
            // Mark losers
            balls.Except(winners).ToList().ForEach(b => {
                balls.Remove(b);
                Destroy(b.gameObject);
            });
            balls.Clear();
            balls.AddRange(winners);

            currentLevelIndex++;
            if(levels.Length >= currentLevelIndex + 1)
            {
                this.StartLevel(levels[currentLevelIndex]);
            }
        }
    }
}
