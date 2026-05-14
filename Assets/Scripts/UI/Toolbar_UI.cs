using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Toolbar_UI : MonoBehaviour
{
    [SerializeField] private List<Slot_UI> toolBarSlots = new List<Slot_UI>();

    private Slot_UI selectedSlot;

    private void Start()
    {
        SelectSlot(0);
    }

    private void Update()
    {
        CheckAlphaNumericKeys();
    }       
    public void SelectSlot(int index)
    {
        for (int i = 0; i < toolBarSlots.Count; i++)
        {
            if(toolBarSlots.Count == 9)
            {
                if(selectedSlot != null)
                {
                    selectedSlot.SetHighlight(false);
                }
                selectedSlot  = toolBarSlots[index];
                selectedSlot.SetHighlight(true);
                
            }
        }
    }

    private void CheckAlphaNumericKeys()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSlot(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSlot(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectSlot(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectSlot(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectSlot(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectSlot(5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectSlot(6);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SelectSlot(7);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SelectSlot(8);
        }
    }
}
