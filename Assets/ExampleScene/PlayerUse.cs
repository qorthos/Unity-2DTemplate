using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerUse : MonoBehaviour
{
    [ReadOnly] [SerializeField] public List<Useable> TargetUseables;
    [ReadOnly] [SerializeField] public List<Useable> CharacterUseables;

    private void OnTriggerEnter(Collider other)
    {
        var useable = other.GetComponent<Useable>();
        if (useable == null)
            return;

        if (other.tag.Equals("TargetUseable"))
        {
            TargetUseables.Add(useable);
        }
        else if (other.tag.Equals("Character"))
        {
            CharacterUseables.Add(useable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var useable = other.GetComponent<Useable>();
        if (useable == null)
            return;

        if (other.tag.Equals("TargetUseable"))
        {
            TargetUseables.Remove(useable);
        }
        else if (other.tag.Equals("Character"))
        {
            CharacterUseables.Remove(useable);
        }
    }
}
