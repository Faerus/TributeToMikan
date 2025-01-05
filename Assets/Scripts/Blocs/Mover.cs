
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 5;
    public bool isLooping = true;
    public Vector3[] locations;
    private int currentTargetIndex;
    private Vector3 initialLocation;

    private void Start()
    {
        initialLocation = transform.position;
    }

    private void Update()
    {
        var distance = Vector3.Distance(transform.position, this.GetTargetLocation());
        if (Mathf.Abs(distance) < 0.1f)
        {
            if(currentTargetIndex < locations.Length - 1)
            {
                ++currentTargetIndex;
            }
            else if(isLooping)
            {
                currentTargetIndex = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, this.GetTargetLocation(), speed * Time.deltaTime);        
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < locations.Length; i++)
        {
            if (i < currentTargetIndex) {
                Gizmos.color = Color.red;
            }
            else if (i > currentTargetIndex) {
                Gizmos.color = Color.green;
            }
            else {
                Gizmos.color = Color.yellow;
            }

            Gizmos.DrawWireSphere(this.GetLocation(i), 1f);
            if(i + 1 < locations.Length || isLooping) {
                Gizmos.DrawLine(this.GetLocation(i), this.GetLocation(i + 1));
            }
        }
    }

    private Vector3 GetTargetLocation()
    {
        return this.GetLocation(currentTargetIndex);
    }

    private Vector3 GetLocation(int index)
    {
        index %= locations.Length;

#if UNITY_EDITOR
        if(!Application.isPlaying)
        {
            return transform.position + locations[index];
        }
#endif
        return initialLocation + locations[index];
    }
}
