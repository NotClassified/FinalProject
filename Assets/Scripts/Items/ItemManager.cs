using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    [SerializeField] GameObject itemPickupPrefab;
    private List<GameObject> itemPickups = new List<GameObject>();

    [SerializeField] int pickupAmount;
    [SerializeField] float minSpaceBetweenPickups;
    [SerializeField] ContactFilter2D castFilter;
    [SerializeField] float pickupCoolDown;

    private Item[] itemTypes;

    private void Awake()
    {
        instance = this;
        itemTypes = GetComponents<Item>();
        PlayerCollision.ItemPickedup += StartDeactivateRoutine;
    }
    private void OnDestroy()
    {
        PlayerCollision.ItemPickedup -= StartDeactivateRoutine;
    }

    private void Start()
    {
        LevelManager levelManager = LevelManager.instance;
        LevelManager.PositionMethod posMethod = new LevelManager.PositionMethod(levelManager.GetRandomAssetPosition);

        for (int i = 0; i < pickupAmount /*&& failedAttempts < 300*/; i++)
        {
            Vector2 position = levelManager.FindAreaWithoutC<Item>(posMethod, minSpaceBetweenPickups, true);

            var pickup = Instantiate(itemPickupPrefab, position, Quaternion.identity, transform);
            itemPickups.Add(pickup);
        }
    }

    void StartDeactivateRoutine(GameObject pickup) => StartCoroutine(DeactivatePickup(pickup));
    IEnumerator DeactivatePickup(GameObject pickup)
    {
        pickup.SetActive(false);
        yield return new WaitForSeconds(pickupCoolDown);
        pickup.SetActive(true);

    }

    public Item GetRandomItem() => itemTypes[Random.Range(0, itemTypes.Length)];
}
