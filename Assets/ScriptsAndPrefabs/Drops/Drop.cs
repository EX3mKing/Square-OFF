using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public float cyan;
    public float yellow;
    public float magenta;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerEnergy playerEnergy = GameManager.instance.playerEnergy;
            playerEnergy.RestoreEnergy(Weapon.Element.Cyan, cyan);
            playerEnergy.RestoreEnergy(Weapon.Element.Yellow, yellow);
            playerEnergy.RestoreEnergy(Weapon.Element.Magenta, magenta);
            Destroy(gameObject);
        }
    }
}
