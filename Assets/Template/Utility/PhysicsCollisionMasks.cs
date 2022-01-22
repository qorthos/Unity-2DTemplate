using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class PhysicsCollisionMasks
{
    private static bool isInit = false;
    private static Dictionary<int, int> _masksByLayer;

    public static void Init()
    {
        isInit = true;
        _masksByLayer = new Dictionary<int, int>();
        for (int i = 0; i < 32; i++)
        {
            int mask = 0;
            for (int j = 0; j < 32; j++)
            {
                if (!Physics.GetIgnoreLayerCollision(i, j))
                {
                    mask |= 1 << j;
                }
            }
            _masksByLayer.Add(i, mask);
        }
    }

    public static int MaskForLayer(int layer)
    {
        if (!isInit)
            Init();

        return _masksByLayer[layer];
    }
}