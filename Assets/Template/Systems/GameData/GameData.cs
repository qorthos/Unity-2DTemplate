using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class GameData
{
    public Dictionary<string, GameDataContainer> Containers = new Dictionary<string, GameDataContainer>();
    public Dictionary<string, Yarn.Value> Variables = new Dictionary<string, Yarn.Value>();

}