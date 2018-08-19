using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameLoader : MonoBehaviour {

    private static string NON_ANIMATED_TEXTURE_FOLDER = "artwork/stockbois";
    public TextAsset source;
    public static Dictionary<string, GameObject> loadedObjects;
    public ElementsGame game;
    public static List<GameObject> instantiatedObjects = new List<GameObject>();
    int turn = 0;
    PLAYBACK_STATE currentState = PLAYBACK_STATE.PAUSED;
    public enum PLAYBACK_STATE
    {
        PLAYING, PAUSED
    }
    private static string PLAY = "Play";
    private static string PAUSE = "Pause";
    public static float playbackSpeedMs = 0.250f;
    private float elapsedTime;
    public static Dictionary<string, Material> materials = new Dictionary<string, Material>();
    public static Dictionary<string, GameObject> gameObjects = new Dictionary<string, GameObject>();
    // Use this for initialization
    void Start()
    {
        elapsedTime = playbackSpeedMs;

        loadTextures("blaze");
        loadTextures("earth");
        loadTextures("air");
        loadTextures("typhoon");
        loadTextures("mud_object");
        loadTextures("magma");
        loadTextures("stockbois");
        game = ElementsGame.create(source);
    }

    public Object[] loadTexturesFromFolder(string folder)
    {
        string path = "artwork/" + folder;
        Object[] textures;
        textures = Resources.LoadAll(path, typeof(Texture2D));
        return textures;
    }

    private void loadAnimationTextures(string folder)
    {
        Object[] textures = loadTexturesFromFolder(folder);
        Dictionary<string, Material> animationMaterials = new Dictionary<string, Material>();
        foreach (var t in textures)
        {
            Material mat = loadTransparentMaterial(t);
            animationMaterials.Add(folder + "`" + t.name, mat);
        }
        
    }

    private void loadTextures(string folder)
    {
        Object[] textures = loadTexturesFromFolder(folder);
        // Load a texture from artwork into a material.
        string path = "artwork/" + folder;
        foreach (var t in textures)
        {
            string fullPath = path + "/" + t.name;
            Material material = loadTransparentMaterial(t);
            GameObject quadWithMaterial = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quadWithMaterial.GetComponent<MeshRenderer>().material = material;
            quadWithMaterial.AddComponent<BonzaiGameObject>();
            quadWithMaterial.SetActive(false);
            gameObjects.Add(fullPath, quadWithMaterial);
        }
    }

    private Material loadTransparentMaterial(Object t)
    {
        Texture2D pleaseWork = (Texture2D)t;
        //pleaseWork.LoadImage(File.ReadAllBytes(fullPath));
        //Color[] pix = pleaseWork.GetPixels();
        //for (int i = 0; i < pix.Length; i++)
        //{
        //    pix[i].a = pix[i].grayscale;
        //}
        //pleaseWork.SetPixels(pix);
        //pleaseWork.Apply();
        //Debug.Log("texture name" + pleaseWork.name);
        Material original = (Material)Resources.Load("artwork/stockbois/transparent", typeof(Material));
        Material material = new Material(original);
        material.SetTexture("_MainTex", pleaseWork);
        //Debug.Log("was this texture successfully added? " + material.GetTexture("_MainTex"));
        //StandardShaderUtils.ChangeRenderMode(material, StandardShaderUtils.BlendMode.Cutout);
        return material;
    }

    public static Material getTileMaterial(string tileName)
    {
        Material material;
        materials.TryGetValue(NON_ANIMATED_TEXTURE_FOLDER+"/"+tileName, out material);
        return material;
    }

    public static GameObject getGameObject(string name)
    {
        GameObject obj;
        gameObjects.TryGetValue(NON_ANIMATED_TEXTURE_FOLDER + "/" + name, out obj);
        return obj;
    }

    private void loadPrefab(string key)
    {
        GameObject loadedObject = Resources.Load("prefabs/"+key.ToLower()) as GameObject;
        loadedObjects.Add(key, loadedObject);
        GameObject value;
        loadedObjects.TryGetValue(key, out value);
        if(loadedObject == null)
        {
            Debug.Log("failed to instantiate though thanks unity");
        }
    }

	// Update is called once per frame
	void Update () {
        if (currentState.Equals(PLAYBACK_STATE.PLAYING))
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= playbackSpeedMs)
            {
                nextTurn();
                elapsedTime = 0;
            }
        }
	}

    public void nextTurn()
    {
        game.renderTurn(turn++);
    }

    public static void registerObject(GameObject obj)
    {
        instantiatedObjects.Add(obj);
    }

    public void setButtonText(Button button, string text)
    {
        button.GetComponentInChildren<Text>().text = text;
    }

    public void togglePlayingState(Button playButton)
    {
        Text playButtonTextObject = playButton.GetComponentInChildren<Text>();
        string playButtonText = playButtonTextObject.text.ToLower().Trim();
        if (playButtonText.Equals("play"))
        {
            Debug.Log("playing");
            // play
            this.currentState = PLAYBACK_STATE.PLAYING;
            setButtonText(playButton, PAUSE);
        }
        else
        {
            // pause
            this.currentState = PLAYBACK_STATE.PAUSED;
            setButtonText(playButton, PLAY);
        }
    }
}
