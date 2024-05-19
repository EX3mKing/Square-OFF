using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
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

    public Volume cyanVolume;
    public Volume yellowVolume;
    public Volume magentaVolume;

    private Dictionary<Element, float> _energyTypesMax = new Dictionary<Element, float>();
    private Dictionary<Element, float> _energyTypesCur = new Dictionary<Element, float>();
    private (Element, float)[] _energyTypesTemp;

    private void Start()
    {
        _energyTypesCur = new Dictionary<Element, float>()
        {
            {Element.Cyan, maxCyanEnergy},
            {Element.Yellow, maxYellowEnergy},
            {Element.Magenta, maxMagentaEnergy}
        };
        _energyTypesMax = new Dictionary<Element, float>()
        {
            {Element.Cyan, maxCyanEnergy},
            {Element.Yellow, maxYellowEnergy},
            {Element.Magenta, maxMagentaEnergy}
        };
    }

    private void Update()
    {
        _energyTypesCur[Element.Cyan] -= depletionRateCyan * Time.deltaTime;
        _energyTypesCur[Element.Yellow] -= depletionRateYellow * Time.deltaTime;
        _energyTypesCur[Element.Magenta] -= depletionRateMagenta * Time.deltaTime;
        
        _energyTypesCur[Element.Cyan] = Mathf.Clamp(_energyTypesCur[Element.Cyan], 0, maxCyanEnergy);
        _energyTypesCur[Element.Yellow] = Mathf.Clamp(_energyTypesCur[Element.Yellow], 0, maxYellowEnergy);
        _energyTypesCur[Element.Magenta] = Mathf.Clamp(_energyTypesCur[Element.Magenta], 0, maxMagentaEnergy);

        cyanSlider.value = _energyTypesCur[Element.Cyan] / maxCyanEnergy;
        yellowSlider.value = _energyTypesCur[Element.Yellow] / maxYellowEnergy;
        magentaSlider.value = _energyTypesCur[Element.Magenta] / maxMagentaEnergy;
        
        cyanVolume.weight = 1 - _energyTypesCur[Element.Cyan] / maxCyanEnergy;
        yellowVolume.weight = 1 - _energyTypesCur[Element.Yellow] / maxYellowEnergy;
        magentaVolume.weight = 1 - _energyTypesCur[Element.Magenta] / maxMagentaEnergy;
        
        if (_energyTypesCur[Element.Cyan] == 0 && _energyTypesCur[Element.Yellow] == 0 && _energyTypesCur[Element.Magenta] == 0)
        {
            print("Game Over");
            GameManager.instance.GameOver();
        }
        
    }

    public float UseEnergy(Element element, float amount)
    {
        float leftOver = _energyTypesCur[element] - amount;
        _energyTypesCur[element] = leftOver >= 0 ? leftOver : _energyTypesCur[element];
        return leftOver;
    }

    public void RestoreEnergy(Element element, float amount)
    {
        _energyTypesCur[element] = Mathf.Clamp(_energyTypesCur[element] + amount,0, _energyTypesMax[element]);
    }

    public override float TakeDamage(Element element, float damage)
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
    private void SortEnergy(Element element)
    {
        _energyTypesTemp = new []{ 
            (Element.Cyan, _energyTypesCur[Element.Cyan]), 
            (Element.Yellow, _energyTypesCur[Element.Yellow]), 
            (Element.Magenta, _energyTypesCur[Element.Magenta])};

        (Element, float) temp;
        
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
