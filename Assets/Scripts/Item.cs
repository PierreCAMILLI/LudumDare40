using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type
    {
        
        NONE,
		FOOD,
        WEAPON,
        GOLD,
        CRAP
    };

    public enum Element
    {
        NONE,

        MEAT,
        APPLE,
        FISH,

        SHIELD,
        SWORD,
        BOOT,

        DIAMOND,
        COIN,

        CAN,
        LINT,
        BONE,
        ROCK
    };

    public Type type;
    public Element element;
    public bool thrown = false;
    public bool cooldownSensitive = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
