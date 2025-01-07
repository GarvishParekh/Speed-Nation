using UnityEngine;
using UnityEngine.UIElements;

public class ArrangeCars : MonoBehaviour
{
    public GameObject[] cars; // Array to hold all car GameObjects
    public int columns = 10; // Number of cars in a row
    public float spacing = 5f; // Spacing between each car
    public float rowOffset = 2.5f; // Optional offset for staggered row effect
    public float heightVariation = 0.5f; // Optional variation in height

    [ContextMenu ("ARANGE GRID")]
    void ArrangeInGrid()
    {
        int carCount = cars.Length; // Get the number of cars
        int rows = Mathf.CeilToInt(carCount / (float)columns); // Calculate number of rows

        for (int i = 0; i < carCount; i++)
        {
            // Calculate grid position
            int row = i / columns;
            int col = i % columns;

            // Calculate X and Z position based on row and column
            float x = col * spacing + (row % 2 == 0 ? 0 : rowOffset); // Optional stagger effect on rows
            float z = row * spacing;
            float y = Mathf.Sin(row * 0.5f) * heightVariation; // Optional height variation for dynamic effect

            // Set the car's position
            Vector3 position = new Vector3(x, y, z);
            cars[i].transform.position = position;

            // Rotate the car to face a specific direction (optional)
            cars[i].transform.rotation = Quaternion.identity; // Reset rotation to default
        }
    }
}
