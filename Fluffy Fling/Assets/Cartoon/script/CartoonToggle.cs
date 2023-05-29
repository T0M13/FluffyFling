using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script shows or hides a Gameobject

public class CartoonToggle : MonoBehaviour
{
    public GameObject toggle; //object to show or hide


    public void CartoonSimpleToggle() {

        toggle.SetActive(!toggle.activeSelf);
    }
}
