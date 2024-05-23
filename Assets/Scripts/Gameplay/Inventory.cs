using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Ingredient CurrentIngredient { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickupIngredient(Ingredient ingredient) 
    {
        CurrentIngredient = ingredient;
        CurrentIngredient.OnGetPickedUp(this.gameObject);
    }
}
