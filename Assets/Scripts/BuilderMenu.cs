using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderMenu : MonoBehaviour
{
    public bool active;

    public string[] catagories;
    public Dictionary<string, string[]> items = new Dictionary<string, string[]>();
    public Dictionary<string, string[]> revCatagoryMap = new Dictionary<string, string[]>();
    public Dictionary<string, string> catagoryMap = new Dictionary<string, string>();

    public void CentreItem(string item)
    { 
        
    }
}
