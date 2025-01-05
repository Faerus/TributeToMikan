using UnityEngine;
using UnityEngine.UIElements;

public enum PathMovementStyle
{
    Continuous,
    Slerp,
    Lerp,
}

public class PathFollower : MonoBehaviour
{
    public float speed;
    public Transform pathParent;

    public bool loop = true;
    public bool startAtFirstPoint = true;
    public PathMovementStyle movementStyle;

    private Transform[] _points;

    private int _currentTargetIdx;

    private void Awake()
    {
        _points = pathParent.GetComponentsInChildren<Transform>();
        if (startAtFirstPoint)
        {
            transform.position = _points[0].position;
        }
    }

    private void Update()
    {
        if (_points == null || _points.Length == 0)
            return;

        var distance = Vector3.Distance(transform.position, _points[_currentTargetIdx].position);
        if (Mathf.Abs(distance) < 0.1f)
        {
            _currentTargetIdx++;
            if (_currentTargetIdx >= _points.Length)
            {
                _currentTargetIdx = loop ? 0 : _points.Length - 1;
            }
        }
        switch (movementStyle)
        {
            default:
            case PathMovementStyle.Continuous:
                transform.position = Vector3.MoveTowards(transform.position, _points[_currentTargetIdx].position, speed * Time.deltaTime);
                break;
            case PathMovementStyle.Lerp:
                transform.position = Vector3.Lerp(transform.position, _points[_currentTargetIdx].position, speed * Time.deltaTime);
                break;
            case PathMovementStyle.Slerp:
                transform.position = Vector3.Slerp(transform.position, _points[_currentTargetIdx].position, speed * Time.deltaTime);
                break;
        }        
    }

    private void OnDrawGizmosSelected()
    {
        if (_points == null || _points.Length == 0)
            return;
 
        for (int i = 0; i < _points.Length; i++)
        {
            Gizmos.color = Color.yellow;
            if (i < _currentTargetIdx)
            {
                Gizmos.color = Color.red;
            }
            
            if (i > _currentTargetIdx)
            {
                Gizmos.color = Color.green;
            }

            Gizmos.DrawWireSphere(_points[i].position, 1f);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, _points[_currentTargetIdx].position);
    }
}
