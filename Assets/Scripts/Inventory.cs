using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : SingletonBehaviour<Inventory>
{
    public Vector3[] itemExposedPosition = new Vector3[3];
    public Vector3[] itemSlots = new Vector3[3];
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
        
        // debug
        inventory.Insert(0, Item.Type.APPLE);
        inventory.Insert(0, Item.Type.GOLD);
        inventory.Insert(0, Item.Type.GOLD);
        inventory.Insert(0, Item.Type.APPLE);
        inventory.Insert(0, Item.Type.WEAPON);
        inventory.Insert(0, Item.Type.APPLE);
        inventory.Insert(0, Item.Type.APPLE);
        inventory.Insert(0, Item.Type.WEAPON);

        slot1 = instanciateItem(Item.Type.APPLE);
        slot2 = instanciateItem(Item.Type.WEAPON);
        slot3 = instanciateItem(Item.Type.WEAPON);

        slot1.transform.position = itemSlots[0];
        slot2.transform.position = itemSlots[1];
        slot3.transform.position = itemSlots[2];
    }
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 0; i < itemExposedPosition.Length && i < itemExposed.Count; i++)
            GameObject.Destroy(itemExposed[i]);

        itemExposed.Clear();

        for (int i = 0; i < itemExposedPosition.Length && i < inventory.Count && i < itemExposedPosition.Length; i++)
        {
            GameObject item = instanciateItem(inventory[i]);
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
            case 0:
                item = slot1.GetComponent<Item>().type;
                GameObject.Destroy(slot1);
                slot1 = instanciateItem(inventory[0]);
                break;
            case 1:
                item = slot2.GetComponent<Item>().type;
                GameObject.Destroy(slot2);
                slot2 = instanciateItem(inventory[0]);
                break;
            default:
                item = slot3.GetComponent<Item>().type;
                GameObject.Destroy(slot3);
                slot3 = instanciateItem(inventory[0]);
                break;
        }

        inventory.RemoveAt(0);
        return item;
    }

    GameObject instanciateItem(Item.Type type)
    {
        switch (type)
        {
            case Item.Type.APPLE:  return Instantiate(applePrefab);
            case Item.Type.WEAPON: return Instantiate(weaponPrefab);
            case Item.Type.GOLD:   return Instantiate(goldPrefab);
            default: return Instantiate(goldPrefab);
        }
    }
}
