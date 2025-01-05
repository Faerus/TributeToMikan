using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(TrailRenderer), typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    public Color color;
    public Vector2 initialVelocity;
    public Vector2 minVelocity;
    public Vector2 maxVelocity = Vector2.positiveInfinity;

    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;
    public Rigidbody2D rigidBody;

    public bool hasLost;

    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        trailRenderer = this.GetComponent<TrailRenderer>();

        this.SetColor(color);
        this.SetVelocity(initialVelocity);
    }

    private void Update()
    {
        // Limit velocity
        rigidBody.linearVelocityX = Math.Clamp(Math.Abs(rigidBody.linearVelocityX), minVelocity.x, maxVelocity.x) * (rigidBody.linearVelocityX < 0 ? -1 : 1);
        rigidBody.linearVelocityY = Math.Clamp(Math.Abs(rigidBody.linearVelocityY), minVelocity.y, maxVelocity.y) * (rigidBody.linearVelocityY < 0 ? -1 : 1);
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
        trailRenderer.colorGradient = this.CreateGradient(color);
    }

    private Gradient CreateGradient(Color color)
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(new GradientColorKey[] {
            new GradientColorKey(color, 0.0f),
            new GradientColorKey(color, 1.0f)
        }, new GradientAlphaKey[] {
            new GradientAlphaKey(1.0f, 0.0f),
            new GradientAlphaKey(1.0f, 0.5f),
            new GradientAlphaKey(0.0f, 1.0f)
        });

        return gradient;
    }

    public void SetVelocity(Vector2 velocity)
    {
        rigidBody.linearVelocity = velocity;
    }

    public void AddForce(Vector2 force)
    {
        rigidBody.AddForce(force, ForceMode2D.Impulse);
    }
}
