using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlackBoard
{
    public static Vector3 offset;

    public static Vector2 center;
    public static Vector2 PosX;
    public static Vector2 NegX;
    public static Vector2 Posy;
    public static Vector2 NegY;


    public static float Xratio;
    public static float YRatio;

    public static Vector2 gridOffset;
    public static GridController gridController;

    public static List<ITile> waterTiles, grassTiles, sandTiles, snowTiles, wastelandTiles, bambooTiles, savannahTiles, sandWaterTiles, grassWaterTiles, tundraTiles, mixedTiles, iceTiles, cherryBlossomTiles;

    public static void Initialize()
    {
        waterTiles = new List<ITile>();
        grassTiles = new List<ITile>();
        sandTiles = new List<ITile>();
        snowTiles = new List<ITile>();
        wastelandTiles = new List<ITile>();
        bambooTiles = new List<ITile>();
        savannahTiles = new List<ITile>();
        sandWaterTiles = new List<ITile>();
        grassWaterTiles = new List<ITile>();
        tundraTiles = new List<ITile>();
        mixedTiles = new List<ITile>();
        iceTiles = new List<ITile>();
        cherryBlossomTiles = new List<ITile>();
    }

    public static void UnAssignTile(ITile _tile)
    {
        switch (_tile.myColor)
        {
            case fiducialColor.water:
                waterTiles.Remove(_tile);
                break;
            case fiducialColor.grass:
                grassTiles.Remove(_tile);
                break;
            case fiducialColor.sand:
                sandTiles.Remove(_tile);
                break;
            case fiducialColor.snow:
                snowTiles.Remove(_tile);
                break;
            case fiducialColor.tundra:
                tundraTiles.Remove(_tile);
                break;
            case fiducialColor.wasteland:
                wastelandTiles.Remove(_tile);
                break;
            case fiducialColor.mixed:
                mixedTiles.Remove(_tile);
                break;
            case fiducialColor.savannah:
                savannahTiles.Remove(_tile);
                break;
            case fiducialColor.watergrass:
                grassWaterTiles.Remove(_tile);
                break;
            case fiducialColor.sandwater:
                sandWaterTiles.Remove(_tile);
                break;
            case fiducialColor.bamboo:
                bambooTiles.Remove(_tile);
                break;
            case fiducialColor.ice:
                iceTiles.Remove(_tile);
                break;
            case fiducialColor.cherryBlossom:
                cherryBlossomTiles.Remove(_tile);
                break;
        }
    }

    public static void AssignTile(ITile _tile)
    {
        switch (_tile.myColor)
        {
            case fiducialColor.water:
                waterTiles.Add(_tile);
                break;
            case fiducialColor.grass:
                grassTiles.Add(_tile);
                break;
            case fiducialColor.sand:
                sandTiles.Add(_tile);
                break;
            case fiducialColor.snow:
                snowTiles.Add(_tile);
                break;
            case fiducialColor.tundra:
                tundraTiles.Add(_tile);
                break;
            case fiducialColor.wasteland:
                wastelandTiles.Add(_tile);
                break;
            case fiducialColor.mixed:
                mixedTiles.Add(_tile);
                break;
            case fiducialColor.savannah:
                savannahTiles.Add(_tile);
                break;
            case fiducialColor.watergrass:
                grassWaterTiles.Add(_tile);
                break;
            case fiducialColor.sandwater:
                sandWaterTiles.Add(_tile);
                break;
            case fiducialColor.bamboo:
                bambooTiles.Add(_tile);
                break;
            case fiducialColor.ice:
                iceTiles.Add(_tile);
                break;
            case fiducialColor.cherryBlossom:
                cherryBlossomTiles.Add(_tile);
                break;

        }
    }
}
