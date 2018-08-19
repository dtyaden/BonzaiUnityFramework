using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

[System.Serializable]
public class Tile : ElementsGameObject {
    //public string name;
    //public int id;
    //public Position position;
    public ArrayList elementsGameObjects;

    public GameObject tileObject;
    
    /// <summary>
    /// Parses a json array of gameobjects and stores them in an array list in this tile
    /// </summary>
    /// <param name="gameObjects"></param>
    public void parseGameObjectJSON(JSONArray gameObjects, Dictionary<int, ElementsGameObject> idsToObject) 
    {
        if (gameObjects != null)
        {
            this.elementsGameObjects = new ArrayList();
            foreach (JSONObject elementJSON in gameObjects)
            {
                ElementsGameObject elementsGameObject = JsonUtility.FromJson<ElementsGameObject>(elementJSON.ToString());
                elementsGameObjects.Add(elementsGameObject);
                idsToObject.Add(elementsGameObject.getId(), elementsGameObject);
                try
                {
                    JSONObject commandJson = (JSONObject)elementJSON["command"];
                    if (commandJson != null)
                    {
                        Command command = JsonUtility.FromJson<Command>(commandJson.ToString());
                        if (command != null)
                        {
                            elementsGameObject.command = command;
                            elementsGameObject.command.name = command.name;
                        }
                    }
                }
                catch(System.InvalidCastException e)
                {

                }
            }
        }
    }
}
