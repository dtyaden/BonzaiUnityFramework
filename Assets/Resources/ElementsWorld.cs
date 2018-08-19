using System.Collections;
using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Contains the positions/actions of tiles and gameobjects for a 
/// </summary>
public class BonzaiTileAndObjects {

    public Tile[][] tiles;
    int x_length, y_length;
    private Dictionary<int, ElementsGameObject> idsToObject = new Dictionary<int, ElementsGameObject>();
    private Dictionary<int, GameObject> gameObjects = new Dictionary<int, GameObject>();
    private Dictionary<int, Tile> instantiatedTiles = new Dictionary<int, Tile>();
    private Dictionary<int, Tile> tilesToRemove = new Dictionary<int, Tile>();

    public BonzaiTileAndObjects(JSONObject data, BonzaiTurnState world) {
        JSONArray tilesJSONArray = ((JSONArray)data["tiles"]);
        x_length = tilesJSONArray.Count;
        y_length = ((JSONArray)tilesJSONArray[0]).Count;
        this.tiles = new Tile[x_length][];
        for (int x = 0; x < x_length; x++) {
            tiles[x] = new Tile[y_length];
            for (int y = 0; y < y_length; y++) {
                Tile currentTile;
                this.tiles[x][y] = JsonUtility.FromJson<Tile>(((JSONArray)tilesJSONArray[x])[y].ToString());
                currentTile = tiles[x][y];
                JSONArray gameObjects = null;
                // send the game object JSON to the tile to be parsed.
                try
                {
                    gameObjects = (JSONArray)((tilesJSONArray[x])[y])["gameObjects"];
                    currentTile.parseGameObjectJSON(gameObjects, idsToObject);
                }
                catch (System.InvalidCastException e)
                {
                    //Debug.Log("caught case exception" + e.StackTrace);
                    //Debug.Log("That wasn't a json array. Don't kill yourself now, Unity...");
                }
            }
        }
    }

    public Dictionary<int, ElementsGameObject> getTilesAsDictionary() 
    {
        Dictionary<int, ElementsGameObject> tileDictionary = new Dictionary<int, ElementsGameObject>();
        for(int i = 0; i < x_length; i++)
        {
            for(int j= 0; j< y_length; j++)
            {
                Tile currentTile = tiles[i][j];
                tileDictionary.Add(currentTile.id, currentTile);
            }
        }
        return tileDictionary;
    }

    public Dictionary<int, Tile> renderTiles(Dictionary<int, Tile> existingTiles)
    {
        for (int i = 0; i < x_length; i++)
        {
            for (int j = 0; j < y_length; j++)
            {
                Tile currentTile = tiles[i][j];
                Tile existingTile;
                existingTiles.TryGetValue(currentTile.id, out existingTile);
                if (existingTile == null)
                {
                    GameObject unityGameObject = currentTile.instantiateObject(0);
                    registerTile(currentTile, unityGameObject);
                }
                else if(!currentTile.name.Equals(existingTile.name))
                {
                    GameObject unityGameObject = currentTile.instantiateObject(0);
                    registerTile(currentTile, unityGameObject);
                    tilesToRemove.Add(currentTile.getId(), currentTile);
                }
            }
        }
        return instantiatedTiles;
    }

    public void registerTile(Tile tile, GameObject unityGameObject)
    {
        instantiatedTiles.Add(tile.id, tile);
        gameObjects.Add(tile.id, unityGameObject);
    }

    public Tile[][] getTiles()
    {
        return tiles;
    }

    public Dictionary<int, ElementsGameObject> getIdsToObjects()
    {
        return idsToObject;
    }

    public Dictionary<int, T> renderGameObjects<T>(Dictionary<int, T> newObjects, Dictionary<int, T> existingElements, float height) where T : ElementsGameObject
    {
        // New list of objects that are alive for this turn.
        Dictionary<int, T> instantiatedGameObjects = new Dictionary<int, T>();
        // Get elements that don't exist in this turn (they're dead).
        // Remove non-existing objects
        Dictionary<int, T> removableObjects = existingElements.Where(pair => !newObjects.ContainsKey(pair.Key)).ToDictionary(x => x.Key, x => x.Value);
        foreach (KeyValuePair<int, T> pair in removableObjects)
        {
            existingElements.Remove(pair.Key);
            GameObject.Destroy(pair.Value.getGameObject());
        }
        foreach (KeyValuePair<int, T> pair in newObjects)
        {
            if (!existingElements.ContainsKey(pair.Key))
            {
                pair.Value.instantiateObject(height);
                instantiatedGameObjects.Add(pair.Key, pair.Value);
            }
            else
            {
                T existingElement;
                existingElements.TryGetValue(pair.Key, out existingElement);
                Command c = pair.Value.command;
                if(c.name != null && !c.name.Equals(""))
                {
                    Debug.Log("Command exists when updating: " + c.name);
                }
                // Add the already instantiated object to the collection of elements that we're keeping.
                if (pair.Value.name.Equals(existingElement.name))
                {
                    existingElement.updatePosition(new Vector3(pair.Value.position.x, pair.Value.position.y, height));
                    instantiatedGameObjects.Add(existingElement.id, existingElement);
                }
                else
                {
                    GameObject.Destroy(existingElement.getGameObject());
                    pair.Value.instantiateObject(height);
                    instantiatedGameObjects.Add(pair.Key, pair.Value);
                }
            }
        }
        return instantiatedGameObjects;
    }
}
