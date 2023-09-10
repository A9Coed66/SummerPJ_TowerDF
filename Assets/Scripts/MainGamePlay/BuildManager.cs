using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    // private void Start() {
    //     buildManager = BuildManager.instance;
    // }

    private void Awake() {
        if(instance!=null)
        {
            Debug.Log("More than one BuildManager in screen");
            return;
        }
        instance = this;
    }
    public GameObject buildEffect;
    public GameObject upgradeEffect;
    public GameObject sellEffect;

    public TurretBlueprint turretToBuild;
    public NodeUI nodeUI;
    private Node selectedNode;

    //Nếu chỉ lấy phần tử thì có thể viêt trên một dòng như cách ở dưới luôn
    public bool CanBuild {get {return turretToBuild != null;}}
    public bool HasMoney {get {return PlayerStats.Money>=turretToBuild.cost;}}

    public void BuildTurretOn(Node node)
    {
        
    }

    public void SelectNode(Node node)
    {
        if(selectedNode==node)
        {
            DeselectedNode();
            return;
        }
        selectedNode = node;
        turretToBuild = null;

        nodeUI.SetTarget(node);

    }

    public void DeselectedNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }

    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
        DeselectedNode();
    }

    public TurretBlueprint GetTurretToBuild()
    {
        return turretToBuild;
    }
}
