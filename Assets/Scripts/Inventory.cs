using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : SingletonBehaviour<Inventory>
{
    public Vector3[] itemExposedPosition = new Vector3[3];
    public Vector3[] itemSlotsPosition = new Vector3[3];
    public GameObject applePrefab;
    public GameObject weaponPrefab;
    public GameObject goldPrefab;
    public GameObject crapPrefab;


    private GameObject[] slots = new GameObject[3];
    private List<Item.Type> inventory;
    private List<GameObject> itemExposed;


    // Use this for initialization
    void Start ()
    {
        itemExposed = new List<GameObject>();
        inventory = new List<Item.Type>();
        
        // debug
        inventory.Insert(0, Item.Type.FOOD);
        inventory.Insert(0, Item.Type.GOLD);
        inventory.Insert(0, Item.Type.GOLD);
		inventory.Insert(0, Item.Type.FOOD);
        inventory.Insert(0, Item.Type.WEAPON);
		inventory.Insert(0, Item.Type.FOOD);
		inventory.Insert(0, Item.Type.FOOD);
        inventory.Insert(0, Item.Type.WEAPON);

		slots[0] = instanciateItem(Item.Type.FOOD);
        slots[1] = instanciateItem(Item.Type.WEAPON);
        slots[2] = instanciateItem(Item.Type.WEAPON);

        slots[0].transform.position = itemSlotsPosition[0];
        slots[1].transform.position = itemSlotsPosition[1];
        slots[2].transform.position = itemSlotsPosition[2];
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
    public void PushFront(Item item)
    {
        inventory.Insert(0, item.type);
    }

    public Item.Type getItem(int index)
    {
        Item.Type item = Item.Type.NONE;
        if (index >= 0 && index < 3 && slots[index])
        {
            item = slots[index].GetComponent<Item>().type;
            GameObject.Destroy(slots[index]);

            if (inventory.Count > 0)
            {
                slots[index] = instanciateItem(inventory[0]);
                slots[index].transform.position = itemSlotsPosition[index];
                inventory.RemoveAt(0);
            }
            else slots[index] = null;
        }
        return item;
    }

    public GameObject instanciateItem(Item.Type type)
    {
        switch (type)
        {
			case Item.Type.FOOD:  return Instantiate(applePrefab);
            case Item.Type.WEAPON: return Instantiate(weaponPrefab);
            case Item.Type.GOLD:   return Instantiate(goldPrefab);
            case Item.Type.CRAP:   return Instantiate(crapPrefab);
            default: return null;
        }
    }
}
