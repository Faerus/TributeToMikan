using System;
using UnityEngine;

public class Propulser : MonoBehaviour
{
    public Vector2 velocityIntensityPercent = Vector2.one;
    public Vector2 velocityForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Ball>(out Ball ball))
        {
            ball.rigidBody.linearVelocity *= velocityIntensityPercent;
            ball.rigidBody.AddForce(velocityForce, ForceMode2D.Impulse);

            // ball.rigidBody.linearVelocityX = Math.Max(Math.Abs(ball.rigidBody.linearVelocityX) + velocityIntensity.x, 0) * (ball.rigidBody.linearVelocityX < 0 ? -1 : 1);
            // ball.rigidBody.linearVelocityY = Math.Max(Math.Abs(ball.rigidBody.linearVelocityY) + velocityIntensity.y, 0) * (ball.rigidBody.linearVelocityY < 0 ? -1 : 1);
        }
    }
}
