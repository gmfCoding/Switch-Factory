using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnergyCapable
{
    int getPowerConsumption();
    int getPowerProduction();

    bool canProduce();
    bool canConsume();
}
