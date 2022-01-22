using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarn;

public abstract class GameDataContainer : ObservableObject
{
    public abstract Value GetValue(string variableName);
    public abstract void SetValue(string variableName, Value value);
}