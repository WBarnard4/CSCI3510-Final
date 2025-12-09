using UnityEngine;
using Unity.AI.Navigation;

public class GenerateLevel : MonoBehaviour
{
    public NavMeshSurface surface;
    public GameObject houseCornerNoHouse;
    public GameObject houseCornerYesHouse;
    public GameObject lakeCorner;
    public GameObject forestCorner;
    public GameObject bottomWallCornerNoTruck;
    public GameObject bottomWallCornerYesTruck;
    public GameObject topWallCorner;

    void Start()
    {
        surface = surface.GetComponent<NavMeshSurface>();
        // Generate Level
        Generate();

        // Update Navmesh
        surface.BuildNavMesh();
    }

    void Generate()
    {
        // Randomly Pick Corner Pieces
        // House Corner
        if (Random.Range(0, 2) == 1)
        {
            houseCornerYesHouse.SetActive(true);
        }
        else
        {
            houseCornerNoHouse.SetActive(true);
        }

        // Forest/Lake Corner
        if (Random.Range(0, 2) == 1)
        {
            lakeCorner.SetActive(true);
        }
        else
        {
            forestCorner.SetActive(true);
        }

        // Truck Corner
        if (Random.Range(0, 2) == 1)
        {
            bottomWallCornerYesTruck.SetActive(true);
        }
        else
        {
            bottomWallCornerNoTruck.SetActive(true);
        }

        topWallCorner.SetActive(true);

    }
}
