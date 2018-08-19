using UnityEngine;

[System.Serializable]
public class ElementsGameObject {
	public string name;
	public int id;
	public Team team;
	public Position position;
	public Position previousPosition;

	public bool hasFire;
	public bool hasWater;
	public bool hasCrystal;
	public int stun;

	public int damageTaken;

	public Command command;
    private GameObject gameObject;
    BonzaiGameObject bonzaiGameObject;

    public GameObject instantiateObject(float z)
    {
        GameObject referenceObject = GameLoader.getGameObject(name);
        GameObject actual = GameObject.Instantiate(referenceObject);
        actual.transform.position = new Vector3(position.x, position.y * -1, z);
        gameObject = actual;
        gameObject.SetActive(true);
        bonzaiGameObject = gameObject.GetComponent<BonzaiGameObject>();
        bonzaiGameObject.name = name;
        return actual;
    }

    public void updatePosition(Vector3 newPosition)
    {
        Vector3 targetPosition = new Vector3(newPosition.x, newPosition.y * -1, newPosition.z);
        bonzaiGameObject.setTargetPosition(targetPosition);
    }

    public int getId()
    {
        return this.id;
    }

    public GameObject getGameObject()
    {
        return gameObject;
    }
}
