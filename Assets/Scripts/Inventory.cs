using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : SingletonBehaviour<Inventory>
{
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

    private Sprite[] sprites = new Sprite[12]; 


    private Item.Element[] slots = new Item.Element[3];
    private List<Item.Element> inventory;
    private List<GameObject> itemExposed;


    // Use this for initialization
    void Start()
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

        slots[0] = Item.Element.MEAT;
        slots[1] = Item.Element.MEAT;
        slots[2] = Item.Element.MEAT;

        // c'est pas du debug, cest important, (resource manager behaviour)
        sprites[0] = meatPrefab.GetComponent<SpriteRenderer>().sprite;
        sprites[1] = applePrefab.GetComponent<SpriteRenderer>().sprite;
        sprites[2] = fishPrefab.GetComponent<SpriteRenderer>().sprite;
        sprites[3] = shieldPrefab.GetComponent<SpriteRenderer>().sprite;
        sprites[4] = swordPrefab.GetComponent<SpriteRenderer>().sprite;
        sprites[5] = bootPrefab.GetComponent<SpriteRenderer>().sprite;
        sprites[6] = diamondPrefab.GetComponent<SpriteRenderer>().sprite;
        sprites[7] = coinPrefab.GetComponent<SpriteRenderer>().sprite;
        sprites[8] = canPrefab.GetComponent<SpriteRenderer>().sprite; 
        sprites[9] = lintPrefab.GetComponent<SpriteRenderer>().sprite;
        sprites[10]= bonePrefab.GetComponent<SpriteRenderer>().sprite;
        sprites[11]= rockPrefab.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
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

    public Item.Element popItem(int index)
    {
        Item.Element item = Item.Element.NONE;
        if (index >= 0 && index < 3 && slots[index] != Item.Element.NONE)
        {
            item = slots[index];

            if (inventory.Count > 0)
            {
                slots[index] = inventory[0];
                inventory.RemoveAt(0);
            }
            else slots[index] = Item.Element.NONE;
        }
        return item;
    }

    public Item.Element[] getSlots()
    {
        return slots;
    }

    public Item.Element[] getInventory()
    {
        Item.Element[] result = new Item.Element[3];
        for (int i = 0; i < 3; i++)
        {
            if (i < inventory.Count) result[i] = inventory[i];
            else result[i] = Item.Element.NONE;
        }
        return result;
    }

    public Sprite getSprite(Item.Element type)
    {
        return sprites[(int)type-1];
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
