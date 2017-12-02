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
    public float cooldownTime;

    private Rigidbody2D rb = null;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(thrown)
        {
            if (!rb) rb = GetComponent<Rigidbody2D>();
            if (rb.velocity.magnitude <= 0.001F) thrown = false;
        }

        if(cooldownSensitive)
        {
            cooldownTime -= Time.deltaTime;
            if (cooldownTime <= 0.0F)
                Destroy(this.gameObject);
        }
	}
}
