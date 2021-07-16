using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITooltip 
{
    public bool isBeenSeen { get; set; }
    public void OnMouseEnter();
    public void OnMouseExit();
}
