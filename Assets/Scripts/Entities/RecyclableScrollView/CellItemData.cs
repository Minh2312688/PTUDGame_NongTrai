using PolyAndCode.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellItemData : MonoBehaviour,ICell
{
    //UI
public TextMeshProUGUI nameLabel;
public TextMeshProUGUI desLabel;

public TextMeshProUGUI quantityLabel;

//Model
private Invenitem _contactInfo;
private int _cellIndex;
//This is called from the SetCell method in DataSource
public void ConfigureCell(Invenitem Invenitem,int cellIndex)
{
_cellIndex = cellIndex;
_contactInfo = Invenitem;
nameLabel.text = Invenitem.name;
desLabel.text = Invenitem.Description;
quantityLabel.text = Invenitem.quantity.ToString();
}

}
