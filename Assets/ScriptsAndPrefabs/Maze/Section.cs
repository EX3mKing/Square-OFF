using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    public Type type;
    public GameObject FRONT;
    public GameObject BACK;
    public GameObject RIGHT;
    public GameObject LEFT;
    public enum Type
    {
        Hall,
        BossHall
    }
}
