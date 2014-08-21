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
    BTopping bTopping;
    public ToppingType topping; // the topping type wich will be instantiate on game start
    [HideInInspector]
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
        BreakableObstacle,
        SolidObstacle,
		SolidWallCorner,
		SolidWallMiddle,
		SolidWallEnd,
        OilField,
        BurningOilField,
		OilVase,
		WaterField,
		WaterVase
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
                //renderer.sharedMaterial = moveRangeMaterial;
                this.Clickable = true;
				animation.Play("Grid_MOVERANGE");
                break;
            case ColorState.ATTACKRANGE:
                //renderer.sharedMaterial = attackRangeMaterial;
				animation.Play("Grid_ATTACKRANGE");
                this.Clickable = true;
                break;
            case ColorState.PATH:
                //renderer.sharedMaterial = pathMaterial;
				animation.Play("Grid_PATH");
				this.Clickable = true;
                break;
            case ColorState.CLICKABLE:
                //renderer.sharedMaterial = clickableMaterial;
                this.Clickable = true;
                break;
            case ColorState.ATTACKAREA:
                //renderer.sharedMaterial = attackAreaMaterial;
				animation.Play("Grid_ATTACKAREA");
                break;
            case ColorState.DEFAULT:
            default:
                //renderer.sharedMaterial = defaultMaterial;
				animation.Play("Grid_DEFAULT");
                this.Clickable = false;
                break;
        }
    }

    public void UpdateTopping()
    {
        if (topping == ToppingType.NONE)
        {
            mapTile.topping = null;
        }
        else
        {
            Type t = Type.GetType(topping.ToString());
            mapTile.topping = (Topping)Activator.CreateInstance(t);
            mapTile.topping.Spawn(mapTile); // init reference to parent mapTile
        }
        lastTopping = mapTile.topping;
    }

    public void SpawnTopping()
    {
        // remove old children objects
		// this deletion is required since all toppings spawned in editor mode wont be refert properly
		// HACK
        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i).gameObject);
        }
        foreach (GameObject go in children)
            DestroyImmediate(go);

        // spawn prefab if there is any
	    GameObject prefab = Resources.Load<GameObject>(prefabPath + mapTile.topping.prefabName); // note: not .prefab!
	    GameObject handle = (GameObject)Instantiate(prefab, Vector3.zero, prefab.transform.rotation);
	    handle.transform.parent = this.transform;
	    handle.transform.localPosition = Vector3.zero;
	    bTopping = handle.GetComponent<BTopping>();
	    if (bTopping == null) Debug.LogError("The Prefab for " + topping + " must have a BTopping component attached to!");
    }

    public void DestroyTopping()
    {
        if (bTopping == null) return;
        bTopping.DestroyTopping();
        bTopping = null;
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
