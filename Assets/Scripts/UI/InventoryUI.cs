using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : SingletonBehaviour<InventoryUI> {

    [SerializeField]
    Image[] _slots, _inventory;
    

    private GUIStyle TextStyle = new GUIStyle();

    // Use this for initialization
    void Start ()
    {
        TextStyle.normal.textColor = new Color(0, 0.9F, 0.1F);
        TextStyle.fontSize = 40;
    }
	
	// Update is called once per frame
	void Update () {
        if (Inventory.Instance == null)
            return;
		for(int i = 0; i < _slots.Length; ++i)
        {
            if(     i < Inventory.Instance.getSlots().Length
                && Inventory.Instance.getSlots()[i] != Item.Element.NONE)
            {
                // TODO: Attach sprite
                Item.Element element = Inventory.Instance.getSlots()[i];
                _slots[i].sprite = Inventory.Instance.getSprite(element);
                _slots[i].color = Color.white;
            }
            else
            {
                _slots[i].sprite = null;
                _slots[i].color = Color.clear;
            }
        }
        for (int i = 0; i < _inventory.Length; ++i)
        {
            if (    i < Inventory.Instance.getInventory().Length
                &&  Inventory.Instance.getInventory()[i] != Item.Element.NONE)
            {
                // TODO: Attach sprite
                Item.Element element = Inventory.Instance.getInventory()[i];
                _inventory[i].sprite = Inventory.Instance.getSprite(element);
                _inventory[i].color = Color.white;
            }
            else
            {
                _inventory[i].sprite = null;
                _inventory[i].color = Color.clear;
            }
        }

        
        
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 50), "Score : " + (Inventory.Instance.Score() + GameManager.Instance.totalScore), TextStyle);
        GUI.Label(new Rect(Screen.width * 1450 / 1600, Screen.height * 700 / 900, 100, 50), "+" + Mathf.Max(0, Inventory.Instance.ItemCount() - 6), TextStyle);
    }
}
