using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;



[Serializable]
public class SpriteSequence
{
    public string Name;
    public List<int> Indices = new List<int>();
    public bool Repeats;
    public float Milliseconds;

    public bool Flip = false;
    
}