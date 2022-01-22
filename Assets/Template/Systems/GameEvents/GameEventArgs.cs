using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CameraSnapGEM : YarnArgs
{
    public string TargetName;
}

public class GameQuitGEM : EventArgs
{

}

public class YarnArgs : EventArgs
{
    public Action OnComplete;
    public string ErrorMessage;
}

//public class YarnScriptGEM : EventArgs
//{
//    public string NodeName;
//    public string CallingObjectName;
//    public string TargetObjectName;
//}

