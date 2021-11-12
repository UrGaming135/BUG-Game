using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public static GameManager instance;
    public Camera primaryCamera;
    public Camera secondaryCamera;

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
    }

    private void Update()
    {
        if (isPlayerAlive)
        {
            secondaryCamera.transform.position = primaryCamera.transform.position;
            secondaryCamera.transform.rotation = primaryCamera.transform.rotation;
        }
    }

    public void SwapActiveCams()
    {
        primaryCamera.gameObject.SetActive(false);
        secondaryCamera.gameObject.SetActive(true);
        secondaryCamera.enabled = true;

    }
}
