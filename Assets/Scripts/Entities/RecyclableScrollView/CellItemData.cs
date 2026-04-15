using PolyAndCode.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellItemData : MonoBehaviour,ICell
{
    //UI
public TextMeshProUGUI nameLabel;
public TextMeshProUGUI desLabel;
//Model
private Invenitems _contactInfo;
private int _cellIndex;
//This is called from the SetCell method in DataSource
public void ConfigureCell(Invenitems invenitems,int cellIndex)
{
_cellIndex = cellIndex;
_contactInfo = invenitems;
nameLabel.text = invenitems.name;
desLabel.text = invenitems.Description;
}

}
