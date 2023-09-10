using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public GameObject nodeUICanvas;
    public TextMeshProUGUI upgradeCost;
    public TextMeshProUGUI sellAmount;
    private Node target;
    public Button upgradeButton;

    public void SetTarget(Node _target)
    {
        target = _target;
        transform.position = _target.GetBuildPosision();

        if(!target.isUpgrade) 
        {
            upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
            upgradeButton.interactable = true;
        }
        else 
        {
            upgradeCost.text = "DONE";
            upgradeButton.interactable = false;
        }

        sellAmount.text = "$" + target.turretBlueprint.GetSellAmount();

        nodeUICanvas.SetActive(true);
    }

    public void Hide()
    {
        nodeUICanvas.SetActive(false);
    }

    public void Upgrade()
    {
        target.UpgradeTurret();
        BuildManager.instance.DeselectedNode();
    }

    public void SellTurret()
    {
        target.SellTurret();
        BuildManager.instance.DeselectedNode();
    }
    
}
