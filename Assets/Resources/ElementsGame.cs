using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleJSON;
using System.Linq;

/// <summary>
/// Root, parses all game states. (TURNS)
/// Contains all game states.
/// </summary>
public class ElementsGame
{
    List<BonzaiTurnState> gameStates;
    public TextAsset source;
    public BonzaiTurnState currentTurn;
    public Dictionary<string, Object> materials;
    private Dictionary<int, ElementsGameObject> persistingTiles = new Dictionary<int, ElementsGameObject>();
    private Dictionary<int, ElementsGameObject> instantiatedObjects = new Dictionary<int, ElementsGameObject>();

    public static ElementsGame create(TextAsset source) {
        return new ElementsGame(source.text);
    }

    public BonzaiTurnState getTurn(int turnNumber)
    {
        return gameStates[turnNumber];
    }

    /// <summary>
    /// Attemp to destroy a GameObject mapped to id.
    /// </summary>
    /// <param name="id"></param>
    private void removeGameObjectById(ElementsGameObject obj)
    {
        Debug.Log("destroying game object: ");
        GameObject.Destroy(obj.getGameObject());
    }

    public void removeOutdatedObjects(Dictionary<int, ElementsGameObject> updatedObjects)
    {   
        // Get all the entries that exist for the previous turn, but are gone now, and destroy them.
        foreach(KeyValuePair<int, ElementsGameObject> pair in instantiatedObjects.Except(updatedObjects))
        {
            removeGameObjectById(pair.Value);
        }
    }

    public void renderTurn(int turnNumber)
    {
        // Get the game state
        BonzaiTurnState turnState = getTurn(turnNumber);
        Tile[][] nextTiles = turnState.getWorld().getTiles();
        Dictionary<int, ElementsGameObject> newTiles = turnState.renderTiles(persistingTiles);
        persistingTiles = newTiles;
        // Draw any new objects.
        Dictionary<int, ElementsGameObject> existingObjects = turnState.renderGameObjects(instantiatedObjects);
        // Remove objects 
        removeOutdatedObjects(existingObjects);
        // Set the current list of accepted objects to be the objects that were alive during this turn.
        this.instantiatedObjects = existingObjects;
    }

    /// <summary>
    /// Parses a gamestate from data
    /// </summary>
    /// <param name="data">A line of JSON</param>
	public ElementsGame(string text)
    {
        string[] JSONLines = text.Split('\n');
        
        JSONObject data = (JSONObject)JSON.Parse(JSONLines[0]);
        int numberOfTurns = data["numberOfTurns"];
        gameStates = new List<BonzaiTurnState>();
        int numberOfStates = JSONLines.Length;
        for (int i = 0; i < numberOfStates; i++)
        {
            JSONObject turnData = (JSONObject)JSON.Parse(JSONLines[i]);
            if (turnData != null)
            {
                JSONNode node = turnData["gameState"];
                if(node != null)
                {
                    gameStates.Add(new BonzaiTurnState((JSONObject)turnData["gameState"]));
                }
            }
        }
    }
}
