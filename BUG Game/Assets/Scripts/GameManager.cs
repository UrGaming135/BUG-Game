using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public static GameManager instance;
    public Camera primaryCamera;
    public Camera secondaryCamera;
    public List<Transform> coverPointTransformList;

    private bool isPlayerAlive { get { return player; } }

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        } else
        {
            Destroy(instance);
        }

        secondaryCamera.enabled = false;
        primaryCamera.enabled = true;
        coverPointTransformList = GetCoverPointList();
    }

    private void Update()
    {
        if (isPlayerAlive)
        {
            secondaryCamera.transform.position = primaryCamera.transform.position;
            secondaryCamera.transform.rotation = primaryCamera.transform.rotation;
        }
    }

    private List<Transform> GetCoverPointList()
    {
        var coverPoints = FindObjectsOfType<CoverPoint>();
        var result = new List<Transform>();
        foreach (var coverPoint in coverPoints)
        {
            result.Add(coverPoint.transform);
        }
        return result;
    }

    public void SwapActiveCams()
    {
        primaryCamera.gameObject.SetActive(false);
        secondaryCamera.gameObject.SetActive(true);
        secondaryCamera.enabled = true;

    }
}
