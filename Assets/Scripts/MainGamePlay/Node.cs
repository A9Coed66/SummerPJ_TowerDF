using UnityEngine;
using UnityEngine.EventSystems;
public class Node : MonoBehaviour
{
    public Color enoughMoneyColor;
    public Color notEnoughMoneyColor;
    public Vector3 positionOffset;

    // [HideInInspector]
    public GameObject turret;
    // [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]
    public bool isUpgrade = false;
    private Renderer rend;
    private Color startColor;


    BuildManager buildManager;
    void Start()
    {
        buildManager = BuildManager.instance;
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    public Vector3 GetBuildPosision()
    {
        return transform.position + positionOffset;
    }
    
    void OnMouseDown() {
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        if(turret!=null)    
        {
            buildManager.SelectNode(this);
            return;
        }

        if(!buildManager.CanBuild)
            return;
             
        // buildManager.BuildTurretOn(this);
        BuildTurret(buildManager.GetTurretToBuild());
    }

    void BuildTurret(TurretBlueprint blueprint)
    {
        if(PlayerStats.Money<blueprint.cost)
        {
            Debug.Log("Not enough money to build!");
            return;
        }
        PlayerStats.Money-=blueprint.cost;
        GameObject _turret =  (GameObject)Instantiate(blueprint.prefab, GetBuildPosision(), Quaternion.identity);
        this.turret = _turret;

        GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosision(), Quaternion.identity);
        Destroy(effect, 5f);

        turretBlueprint = buildManager.GetTurretToBuild();
        Debug.Log("Turret build! Money left: " + PlayerStats.Money);
    }

    public void UpgradeTurret()
    {
        // Debug.Log("HI");
        // if (this.turret == null)
        // {
        //     Debug.Log("Turret is not initialized.");
        //     return;
        // }
        if(PlayerStats.Money<turretBlueprint.upgradeCost)
        {
            Debug.Log("Not enough money to upgrade!");
            return;
        }
        isUpgrade = true;
        PlayerStats.Money-=turretBlueprint.upgradeCost;

        //Get rid of the old turret
        Destroy(this.turret);
        Debug.Log("Destroy current turret");
        //Building a new one
        GameObject _turret =  (GameObject)Instantiate(turretBlueprint.upgradePrefab, GetBuildPosision(), Quaternion.identity);
        this.turret = _turret;

        GameObject effect = (GameObject)Instantiate(buildManager.upgradeEffect, GetBuildPosision(), Quaternion.identity);
        Destroy(effect, 5f);
        Debug.Log("Turret upgrade");
    }

    public void SellTurret()
    {
        PlayerStats.Money+= turretBlueprint.GetSellAmount();
        GameObject effect = (GameObject)Instantiate(buildManager.sellEffect, GetBuildPosision(), Quaternion.identity);
        Destroy(effect, 5f);
        Destroy(turret);
        turretBlueprint = null;
        Debug.Log("Sell turret");
    }

    void OnMouseEnter(){
        if(EventSystem.current.IsPointerOverGameObject())
            return;
        if(!buildManager.CanBuild)
            return;
        
        if(buildManager.HasMoney)
        {    
            rend.material.color = enoughMoneyColor;
        }else{
            rend.material.color = notEnoughMoneyColor;
        }
    }    

    void OnMouseExit() 
    {
        rend.material.color = startColor;
    }

}
