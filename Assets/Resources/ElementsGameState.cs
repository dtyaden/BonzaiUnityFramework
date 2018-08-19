using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A bonzai turn container.
/// </summary>
public class BonzaiTurnState {

	public BonzaiTileAndObjects world;

	public BonzaiTurnState(JSONObject data) {
		this.world = new BonzaiTileAndObjects((JSONObject) data["world"], this);
	}

    public BonzaiTileAndObjects getWorld()
    {
        return world;
    }

    public Dictionary<int, T> renderGameObjects<T>(Dictionary<int, T> newObjects, Dictionary<int, T> existingElements, float height) where T : ElementsGameObject
    {
       return world.renderGameObjects(newObjects, existingElements, height);
    }

    public Dictionary<int, ElementsGameObject> renderTiles(Dictionary<int, ElementsGameObject> existingTiles)
    {
        Dictionary<int, ElementsGameObject> newTiles = world.getTilesAsDictionary();
        
        return renderGameObjects(newTiles, existingTiles, 0.0f);
    }

    public Dictionary<int, ElementsGameObject> renderGameObjects(Dictionary<int, ElementsGameObject> existingObjects)
    {
        return world.renderGameObjects(world.getIdsToObjects(), existingObjects, -.001f);
    }
}
