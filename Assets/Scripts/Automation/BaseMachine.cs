using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMachine : MonoBehaviour, IEnergyCapable
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public abstract bool canConsume();
    public abstract bool canProduce();
    public abstract int getPowerConsumption();
    public abstract int getPowerProduction();

}
