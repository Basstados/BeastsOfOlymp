using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class BMapTile : MonoBehaviour
{

    public string prefabPath;
    public Material defaultMaterial;
    public Material moveRangeMaterial;
    public Material attackRangeMaterial;
    public Material pathMaterial;
    public Material attackAreaMaterial;
    public Material clickableMaterial;

    public MapTile mapTile;
    public ToppingType topping; // the topping type wich will be instantiate on game start
    public Topping lastTopping; // used to track change of topping
    [HideInInspector]
    public ColorState colorState; // the current visual state of this instance

    public bool InRange
    {
        get
        {
            return !(colorState == ColorState.DEFAULT);
        }
    }

    public bool Clickable { get; set; }

    public enum ToppingType
    {
        NONE,
        SOLID_OBSTACLE,
        BREAKABLE_OBSTACLE,
        OIL,
        BURNING_OIL
    }

    public enum ColorState
    {
        MOVERANGE,
        ATTACKRANGE,
        DEFAULT,
        PATH,
        CLICKABLE,
        ATTACKAREA
    }

    public void ChangeColorState(BMapTile.ColorState colorState)
    {
        StopCoroutine("TweenRoutine");
        this.colorState = colorState;

        switch (colorState)
        {
            case ColorState.MOVERANGE:
                renderer.sharedMaterial = moveRangeMaterial;
                this.Clickable = true;
                break;
            case ColorState.ATTACKRANGE:
                renderer.sharedMaterial = attackRangeMaterial;
                this.Clickable = true;
                break;
            case ColorState.PATH:
                renderer.sharedMaterial = pathMaterial;
                StartCoroutine(TweenRoutine(pathMaterial, Color.yellow, Color.gray));
                break;
            case ColorState.CLICKABLE:
                renderer.sharedMaterial = clickableMaterial;
                this.Clickable = true;
                break;
            case ColorState.ATTACKAREA:
                renderer.sharedMaterial = attackAreaMaterial;
                break;
            case ColorState.DEFAULT:
            default:
                renderer.sharedMaterial = defaultMaterial;
                this.Clickable = false;
                break;
        }
    }

    void Update()
    {
        if (lastTopping != mapTile.topping)
        {
            if (mapTile.topping == null)
            {
                topping = ToppingType.NONE;
            }
            else
            {
                switch (mapTile.topping.GetType().ToString())
                {
                    case "SolidObstacle":
                        topping = ToppingType.SOLID_OBSTACLE;
                        break;
                    case "OilField":
                        topping = ToppingType.OIL;
                        break;
                    case "BurningOilField":
                        topping = ToppingType.BURNING_OIL;
                        break;
                    case "BreakableObstacle":
                        topping = ToppingType.BREAKABLE_OBSTACLE;
                        break;
                    default:
                        topping = ToppingType.NONE;
                        break;
                }
            }
            
            UpdateTopping();
        }

    }

    public void UpdateTopping()
    {
        switch (topping)
        {
            case ToppingType.NONE:
                mapTile.topping = null;
                break;
            case ToppingType.SOLID_OBSTACLE:
                mapTile.topping = new SolidObstacle();
                break;
            case ToppingType.OIL:
                mapTile.topping = new OilField();
                break;
            case ToppingType.BURNING_OIL:
                mapTile.topping = new BurningOilField();
                break;
            case ToppingType.BREAKABLE_OBSTACLE:
                mapTile.topping = new BreakableObstacle();
                break;
        }

        // TODO make this more elegant (not copy-code)

        // if topping has changed, instantiate new prefab

        // remove old children objects
        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i).gameObject);
        }
        foreach (GameObject go in children)
            DestroyImmediate(go);

        // spawn prefab if there is any
        if (topping != BMapTile.ToppingType.NONE)
        {
            Debug.Log(prefabPath + topping.ToString());
            GameObject prefab = Resources.Load<GameObject>(prefabPath + topping.ToString()); // note: not .prefab!
            GameObject handle = (GameObject)Instantiate(prefab, Vector3.zero, prefab.transform.rotation);
            handle.transform.parent = this.transform;
            handle.transform.localPosition = Vector3.zero;


        }

        lastTopping = mapTile.topping;
    }

    IEnumerator TweenRoutine(Material mat, Color c1, Color c2)
    {
        float t = 0;
        bool rising = true;

        while (true)
        {
            if (rising)
            {
                t += Time.deltaTime;
                if (t > 1) rising = false;
            }
            else
            {
                t -= Time.deltaTime;
                if (t < 0) rising = true;
            }

            mat.color = Color.Lerp(c1, c2, t);
            // wait 1 frame
            yield return 0;
        }
    }
}
