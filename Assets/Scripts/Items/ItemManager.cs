using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] GameObject itemPickupPrefab;
    private List<GameObject> itemPickups = new List<GameObject>();
    private List<Vector2> pickupPositions = new List<Vector2>();

    [SerializeField] int pickupAmount;
    [SerializeField] float minSpaceBetweenPickups;
    [SerializeField] ContactFilter2D castFilter;
    [SerializeField] float pickupCoolDown;

    private Item[] itemTypes;

    private void Awake()
    {
        itemTypes = GetComponents<Item>();
        PlayerCollision.ItemPickedup += StartDeactivateRoutine;
    }
    private void OnDestroy()
    {
        PlayerCollision.ItemPickedup -= StartDeactivateRoutine;
    }

    private void Start()
    {
        int failedAttempts = 0;
        for (int i = 0; i < pickupAmount && failedAttempts < 300; i++)
        {
            Vector2 randPos = LevelAssetData.instance.GetRandomAssetPosition(false, false);
            while (Physics2D.OverlapCircle(randPos, minSpaceBetweenPickups, castFilter, new Collider2D[1]) > 0
            && failedAttempts < 300)
            {

                randPos = LevelAssetData.instance.GetRandomAssetPosition(false, false);
                failedAttempts++;
            }
            var pickup = Instantiate(itemPickupPrefab, randPos, Quaternion.identity, transform);
            itemPickups.Add(pickup);
        }
        print(failedAttempts);
    }

    void StartDeactivateRoutine(GameObject pickup) => StartCoroutine(DeactivatePickup(pickup));
    IEnumerator DeactivatePickup(GameObject pickup)
    {
        pickup.SetActive(false);
        yield return new WaitForSeconds(pickupCoolDown);
        pickup.SetActive(true);

    }
}
