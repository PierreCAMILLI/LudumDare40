using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : SingletonBehaviour<Inventory>
{
    public Vector3[] itemExposedPosition = new Vector3[10];
    public GameObject applePrefab;
    public GameObject weaponPrefab;
    public GameObject goldPrefab;


    private GameObject slot1;
    private GameObject slot2;
    private GameObject slot3;
    private List<Item.Type> inventory;
    private List<GameObject> itemExposed;


    // Use this for initialization
    void Start ()
    {
        itemExposed = new List<GameObject>();
        inventory = new List<Item.Type>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 0; i < itemExposedPosition.Length && i < itemExposed.Count; i++)
            GameObject.Destroy(itemExposed[i]);

        itemExposed.Clear();

        for (int i = 0; i < itemExposedPosition.Length && i < inventory.Count && i < itemExposedPosition.Length; i++)
        {
            GameObject item;
            switch (inventory[i])
            {
                case Item.Type.APPLE:   item = Instantiate(applePrefab);    break;
                case Item.Type.WEAPON:  item = Instantiate(weaponPrefab);   break;
                case Item.Type.GOLD:    item = Instantiate(goldPrefab);     break;
                default:                item = Instantiate(goldPrefab);     break;
            }
            item.transform.position = itemExposedPosition[i];
            itemExposed.Insert(i, item);
        }
    }

    //  container modifier
    void PushFront(Item item)
    {
        inventory.Insert(0, item.type);
    }

    Item.Type getItem(int slot)
    {
        Item.Type item;
        switch (slot)
        {
            case 1: item = slot1.GetComponent<Item>().type; break;
            case 2: item = slot2.GetComponent<Item>().type; break;
            case 3: item = slot3.GetComponent<Item>().type; break;
            default: item = Item.Type.GOLD; break;
        }
        return item;
    }
}
