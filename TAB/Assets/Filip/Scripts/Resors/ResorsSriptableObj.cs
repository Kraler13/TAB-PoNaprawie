using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Resors")]
public class ResorsSriptableObj : ScriptableObject
{
    public float ResorsOne;
    public float ResorsTwo;

    public int ForestCountTiles;
    public int ForestCountTilesToAdd;

    public int StoneCountTiles;
    public int StoneCountTilesToAdd;

    public bool isNewGame = true;

    public List<BoxCollider> boxCollidersToDestroy;

}
