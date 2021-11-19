using Assets.Scripts;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField]
    public DropType dropType;
    [SerializeField]
    [Range(0.0f, 100.0f)]
    public float dropChance;
    [SerializeField]
    public float amount;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.CompareTag("Player") ? other.gameObject : null;
        if (player)
        {
            var playerController = player.GetComponent<PlayerController>();
            var characterManager = player.GetComponent<CharacterManager>();
            if (dropType == DropType.Ammo && playerController && playerController.currentWeapon && playerController.currentWeapon.remainingAmmo != playerController.currentWeapon.maxAmmo)
            {
                playerController.currentWeapon.AddAmmo((int)amount);
                Destroy(gameObject);
            }
            else if (dropType == DropType.Health && characterManager.currentHealth != characterManager.maxHealth)
            {
                characterManager.AddHealth(amount);
                Destroy(gameObject);
            }

        }
    }
}
