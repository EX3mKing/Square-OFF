using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : Damageable
{
    public float maxCyanEnergy = 100;
    public float maxYellowEnergy = 100;
    public float maxMagentaEnergy = 100;

    // how fast does the energy run out
    public float depletionRateCyan = 1;
    public float depletionRateYellow = 1;
    public float depletionRateMagenta = 1;

    public UI_EnergyBar cyanSlider;
    public UI_EnergyBar yellowSlider;
    public UI_EnergyBar magentaSlider;
    
    private Dictionary<Weapon.Element, float> _energyTypesMax = new Dictionary<Weapon.Element, float>();
    private Dictionary<Weapon.Element, float> _energyTypesCur = new Dictionary<Weapon.Element, float>();
    private (Weapon.Element, float)[] _energyTypesTemp;

    private void Start()
    {
        _energyTypesCur = new Dictionary<Weapon.Element, float>()
        {
            {Weapon.Element.Cyan, maxCyanEnergy},
            {Weapon.Element.Yellow, maxYellowEnergy},
            {Weapon.Element.Magenta, maxMagentaEnergy}
        };
        _energyTypesMax = new Dictionary<Weapon.Element, float>()
        {
            {Weapon.Element.Cyan, maxCyanEnergy},
            {Weapon.Element.Yellow, maxYellowEnergy},
            {Weapon.Element.Magenta, maxMagentaEnergy}
        };
    }

    private void Update()
    {
        _energyTypesCur[Weapon.Element.Cyan] -= depletionRateCyan * Time.deltaTime;
        _energyTypesCur[Weapon.Element.Yellow] -= depletionRateYellow * Time.deltaTime;
        _energyTypesCur[Weapon.Element.Magenta] -= depletionRateMagenta * Time.deltaTime;
        
        _energyTypesCur[Weapon.Element.Cyan] = Mathf.Clamp(_energyTypesCur[Weapon.Element.Cyan], 0, maxCyanEnergy);
        _energyTypesCur[Weapon.Element.Yellow] = Mathf.Clamp(_energyTypesCur[Weapon.Element.Yellow], 0, maxYellowEnergy);
        _energyTypesCur[Weapon.Element.Magenta] = Mathf.Clamp(_energyTypesCur[Weapon.Element.Magenta], 0, maxMagentaEnergy);

        cyanSlider.value = _energyTypesCur[Weapon.Element.Cyan] / maxCyanEnergy;
        yellowSlider.value = _energyTypesCur[Weapon.Element.Yellow] / maxYellowEnergy;
        magentaSlider.value = _energyTypesCur[Weapon.Element.Magenta] / maxMagentaEnergy;
        
        if (_energyTypesCur[Weapon.Element.Cyan] == 0 && _energyTypesCur[Weapon.Element.Yellow] == 0 && _energyTypesCur[Weapon.Element.Magenta] == 0)
        {
            print("Game Over");
        }
    }

    public float UseEnergy(Weapon.Element element, float amount)
    {
        float leftOver = _energyTypesCur[element] - amount;
        _energyTypesCur[element] = leftOver >= 0 ? leftOver : _energyTypesCur[element];
        return leftOver;
    }

    public void RestoreEnergy(Weapon.Element element, float amount)
    {
        _energyTypesCur[element] = Mathf.Clamp(_energyTypesCur[element] + amount,0, _energyTypesMax[element]);
    }

    public override float TakeDamage(Weapon.Element element, float damage)
    {
        // applies the weaknesses to the damage
        damage = base.TakeDamage(element, damage);
        SortEnergy(element);
        
        for (int i = 0; i < 3; i++)
        {
            if (_energyTypesTemp[i].Item2 > damage)
            {
                _energyTypesTemp[i].Item2 -= damage;
                break;
            }
            _energyTypesTemp[i].Item2 = 0;
            damage -= _energyTypesTemp[i].Item2;
            damage *= 2; // damage is doubled when overspilling to the next energy type
        }
        
        foreach (var energy in _energyTypesTemp)
        {
            _energyTypesCur[energy.Item1] = energy.Item2;
        }
        
        return damage;
    }
    
    // bubble sort, and sets the selected element to the top
    private void SortEnergy(Weapon.Element element)
    {
        _energyTypesTemp = new []{ 
            (Weapon.Element.Cyan, _energyTypesCur[Weapon.Element.Cyan]), 
            (Weapon.Element.Yellow, _energyTypesCur[Weapon.Element.Yellow]), 
            (Weapon.Element.Magenta, _energyTypesCur[Weapon.Element.Magenta])};

        (Weapon.Element, float) temp;
        
        for (int i = 0; i < 2; i++)
        {
            for (int sort = 0; sort < 2 - i; sort++)
            {
                if (_energyTypesTemp[sort].Item2 <= _energyTypesTemp[sort + 1].Item2)
                {
                    temp = _energyTypesTemp[sort + 1];
                    _energyTypesTemp[sort + 1] = _energyTypesTemp[sort];
                    _energyTypesTemp[sort] = temp;
                }
            }
        }
        
        for (int i = 0; i < 2; i++)
        {
            for (int sort = 0; sort < 2 - i; sort++)
            {
                if (_energyTypesTemp[sort + 1].Item1 == element)
                {
                    temp = _energyTypesTemp[sort + 1];
                    _energyTypesTemp[sort + 1] = _energyTypesTemp[sort];
                    _energyTypesTemp[sort] = temp;
                }
            }
        }
    }
}
