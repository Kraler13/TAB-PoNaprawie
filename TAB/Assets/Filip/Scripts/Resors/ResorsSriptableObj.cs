using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Resors")]
public class ResorsSriptableObj : ScriptableObject
{
    public float Stone;
    public float Wood;
    public float Food;
    public float WorkForce;

    public int ForestCountTiles;
    public int ForestCountTilesToAdd;

    public int StoneCountTiles;
    public int StoneCountTilesToAdd;

    public int FoodCountTiles = 15;
    public int FoodCountTilesToAdd;

    public bool isNewGame = true;

    public List<BoxCollider> boxCollidersToDestroy;

}
