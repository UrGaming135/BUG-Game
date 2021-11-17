using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class CoverPointNavigation : MonoBehaviour
{
    // Locate the closest, unoccupied cover point.
    public Transform closestCoverPoint;
    private float range;
    private GameObject target;
    private List<Transform> coverPoints;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameManager.instance.player;
        coverPoints = GameManager.instance.coverPointTransformList;
        range = GetComponent<EnemyAI>().attackRange;
    }

    // Update is called once per frame
    void Update()
    {
        closestCoverPoint = GetClosestCoverPoint();
    }

    private Transform GetClosestCoverPoint()
    {
        // Use cover point list to find the closest cover point that is within pew pew distance from player.
        coverPoints.OrderBy(coverPoint => Vector3.Distance(coverPoint.position, transform.position));

        foreach(var coverPoint in coverPoints)
        {
            if (Vector3.Distance(coverPoint.position, target.transform.position) <= range)
            {
                return coverPoint;
            }
        }
        return null;
    }
}
