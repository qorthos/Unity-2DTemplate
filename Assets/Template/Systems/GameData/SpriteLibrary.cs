using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteLibrary", menuName = "Data/SpriteLibrary")]
public class SpriteLibrary : ScriptableObject
{
    public static SpriteLibrary Instance;

    private void OnEnable()
    {
        Instance = this;
    }

    private void OnDisable()
    {
        Instance = null;
    }

    public List<SpriteFamily> Families;

    public Sprite GetSprite(string familyName, string spriteName)
    {
        var family = Families.FirstOrDefault(x => x.Name.Equals(familyName));

        if (family == null)
        {
            Debug.LogError($"no such family, {familyName}");
            return null;
        }

        var key = family.Keys.FirstOrDefault(x => x.Name.Equals(spriteName));

        if (family == null)
        {
            Debug.LogError($"no such key ${spriteName} in family {familyName}");
            return null;
        }

        return key.Sprite;
    }
}
