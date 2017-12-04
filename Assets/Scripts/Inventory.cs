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


    // Use this for initialization
    void Start()
    {
        inventory = new List<Item.Element>();

        // debug
        inventory.Insert(0, Item.Element.MEAT);
        inventory.Insert(0, Item.Element.APPLE);
        inventory.Insert(0, Item.Element.BONE);
        inventory.Insert(0, Item.Element.BOOT);
        inventory.Insert(0, Item.Element.CAN);
        inventory.Insert(0, Item.Element.COIN);
        inventory.Insert(0, Item.Element.DIAMOND);
        inventory.Insert(0, Item.Element.FISH);

        slots[0] = Item.Element.LINT;
        slots[1] = Item.Element.MEAT;
        slots[2] = Item.Element.ROCK;

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
        
    }

    //  container modifier
    public void PushFront(Item item)
    {
        for (int i = 0; i < 3; i++)
        {
            if (slots[i] == Item.Element.NONE)
            {
                slots[i] = item.element;
                return;
            }
        }

        int moncul = Random.Range(0, 3);
        inventory.Insert(0, slots[moncul]);
        slots[moncul] = item.element;
    }

	//  container modifier
	public void PushFrontElement(Item.Element item)
	{
		for (int i = 0; i < 3; i++)
		{
			if (slots[i] == Item.Element.NONE)
			{
				slots[i] = item;
				return;
			}
		}

		int moncul = Random.Range(0, 3);
		inventory.Insert(0, slots[moncul]);
		slots[moncul] = item;
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
