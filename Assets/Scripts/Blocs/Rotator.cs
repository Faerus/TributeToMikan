using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed;

    private void Start()
    {
        
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }
}
