#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ObjectPlacer : MonoBehaviour
{
    public GameObject prefabToPlace;
    private Vector3 lastPlacedPosition;
    private bool isActive = false;

    void Update()
    {
        // Check if the script should be active and the user has clicked the left mouse button
        if (isActive && Event.current != null && Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            // Construct a ray from the current mouse position
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // We hit something in the world, place the prefab here
                lastPlacedPosition = hit.point;
                PlaceObject(lastPlacedPosition, Quaternion.identity, hit);
            }

            // Use the event so it doesn't propagate further
            Event.current.Use();
        }
    }

    public void PlaceObject(Vector3 position, Quaternion rotation, RaycastHit hit)
    {
        if (prefabToPlace == null)
        {
            Debug.LogError("PrefabToPlace is not assigned!");
            return;
        }

        // Instantiate the prefab at the specified position and rotation
        GameObject createdObject = (GameObject)PrefabUtility.InstantiatePrefab(prefabToPlace, hit.transform);
        Undo.RegisterCreatedObjectUndo(createdObject, "Place " + createdObject.name);
        createdObject.transform.position = position;
        createdObject.transform.rotation = rotation;
    }

    // Call this method to toggle the active state
    public void ToggleActive()
    {
        isActive = !isActive;
    }

    // Add menu item to toggle the ObjectPlacer active state
    [MenuItem("Tools/Toggle Object Placer")]
    private static void ToggleObjectPlacer()
    {
        ObjectPlacer placer = FindObjectOfType<ObjectPlacer>();
        if (placer != null)
        {
            placer.ToggleActive();
        }
    }
}

#endif