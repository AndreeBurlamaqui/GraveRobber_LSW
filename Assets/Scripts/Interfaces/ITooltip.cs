using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ITooltip 
{
    bool isBeenSeen { get; set; }
    void OnMouseEnter();
    void OnMouseExit();
}
