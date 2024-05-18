using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class Slot : MonoBehaviour//, IPointerUpHandler

{
    public Image itemIcon;
    public GameObject BackLight;
    public GameObject FrontLight;
    public Item item;
}
