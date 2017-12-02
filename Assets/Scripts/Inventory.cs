using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : SingletonBehaviour<Inventory>
{
    public Vector3[] itemExposedPosition = new Vector3[3];
    public Vector3[] itemSlotsPosition = new Vector3[3];

    public GameObject meatPrefab;
    public GameObject applePrefab;
    public GameObject fishPrefab;

    public GameObject shieldPrefab;
    public GameObject swordPrefab;
    public GameObject bootPrefab;

    public GameObject diamondPrefab;
    public GameObject coinPrefab;

    public GameObject canPrefab;
    public GameObject lintPrefab;
    public GameObject bonePrefab;
    public GameObject rockPrefab;


    private GameObject[] slots = new GameObject[3];
    private List<Item.Element> inventory;
    private List<GameObject> itemExposed;


    // Use this for initialization
    void Start ()
    {
        itemExposed = new List<GameObject>();
        inventory = new List<Item.Element>();

        // debug
        inventory.Insert(0, Item.Element.MEAT);
        inventory.Insert(0, Item.Element.MEAT);
        inventory.Insert(0, Item.Element.MEAT);
		inventory.Insert(0, Item.Element.MEAT);
        inventory.Insert(0, Item.Element.MEAT);
		inventory.Insert(0, Item.Element.MEAT);
		inventory.Insert(0, Item.Element.MEAT);
        inventory.Insert(0, Item.Element.MEAT);

		slots[0] = instanciateItem(Item.Element.MEAT);
        slots[1] = instanciateItem(Item.Element.MEAT);
        slots[2] = instanciateItem(Item.Element.MEAT);

        slots[0].transform.position = itemSlotsPosition[0];
        slots[1].transform.position = itemSlotsPosition[1];
        slots[2].transform.position = itemSlotsPosition[2];
    }
	
	// Update is called once per frame
	void Update ()
    {
        /*for (int i = 0; i < itemExposedPosition.Length && i < itemExposed.Count; i++)
            GameObject.Destroy(itemExposed[i]);

        itemExposed.Clear();

        for (int i = 0; i < itemExposedPosition.Length && i < inventory.Count && i < itemExposedPosition.Length; i++)
        {
            GameObject item = instanciateItem(inventory[i]);
            item.transform.position = itemExposedPosition[i];
            itemExposed.Insert(i, item);
        }*/
    }

    //  container modifier
    public void PushFront(Item item)
    {
        inventory.Insert(0, item.element);
    }

    public Item.Element getItem(int index)
    {
        Item.Element item = Item.Element.NONE;
        if (index >= 0 && index < 3 && slots[index])
        {
            item = slots[index].GetComponent<Item>().element;
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

    public GameObject instanciateItem(Item.Element type)
    {
        switch (type)
        {
			case Item.Element.MEAT:   return Instantiate(meatPrefab);
            case Item.Element.APPLE:  return Instantiate(applePrefab);
            case Item.Element.FISH:   return Instantiate(fishPrefab);

            case Item.Element.SHIELD: return Instantiate(shieldPrefab);
            case Item.Element.SWORD:  return Instantiate(swordPrefab);
            case Item.Element.BOOT:   return Instantiate(bootPrefab);

            case Item.Element.DIAMOND: return Instantiate(diamondPrefab);
            case Item.Element.COIN:    return Instantiate(coinPrefab);

            case Item.Element.CAN:  return Instantiate(canPrefab);
            case Item.Element.LINT: return Instantiate(lintPrefab);
            case Item.Element.BONE: return Instantiate(bonePrefab);
            case Item.Element.ROCK: return Instantiate(rockPrefab);

            default: return null;
        }
    }
}
