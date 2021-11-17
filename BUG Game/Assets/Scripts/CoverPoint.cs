using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverPoint : MonoBehaviour
{
    [SerializeField]
    private float gizmoRadius = 1f;
    [SerializeField]
    private Color color = Color.white;
    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }
}
