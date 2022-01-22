using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotButton : MonoBehaviour
{
    public GameDataChannel GameDataChannel;
    public int SlotNumber;

    public TMPro.TMP_Text NameText;
    public TMPro.TMP_Text DateText;

    // Start is called before the first frame update
    void Start()
    {
        NameText.text = $"Slot {SlotNumber}";
    }

    // Update is called once per frame
    void Update()
    {
        switch (SlotNumber)
        {
            case 0:
                DateText.text = GameDataChannel.Slot0Time;
                break;

            case 1:
                DateText.text = GameDataChannel.Slot1Time;
                break;

            case 2:
                DateText.text = GameDataChannel.Slot2Time;
                break;
        }

        

    }
}
