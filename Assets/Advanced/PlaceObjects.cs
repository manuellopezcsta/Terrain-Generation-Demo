using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(GenerateMesh))]
public class PlaceObjects : MonoBehaviour {
    // Porcentajes
    int PArbol;
    int PPiedras;
    int PTronquito;

    void SetSpawnValues()
    {
        PArbol = PlayerPrefs.GetInt("percentTrees",65);
        PPiedras = PlayerPrefs.GetInt("percentRocks",30);
        PTronquito = PlayerPrefs.GetInt("percentOthers",5);
    }    

    public TerrainController TerrainController { get; set; }


    public void Place() {
        
        SetSpawnValues();
        int numObjects = Random.Range(TerrainController.MinObjectsPerTile, TerrainController.MaxObjectsPerTile);
        for (int i = 0; i < numObjects; i++) {
            //int prefabType = Random.Range(0, TerrainController.PlaceableObjects.Length);
            int prefabType = GetIndexOfItemToSpawn(); // Nueva linea con funcion de porcentajes.
            Vector3 startPoint = RandomPointAboveTerrain();

            RaycastHit hit;
            if (Physics.Raycast(startPoint, Vector3.down, out hit) && hit.point.y > TerrainController.Water.transform.position.y && hit.collider.CompareTag("Terrain")) {
                Quaternion orientation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
                RaycastHit boxHit;
                if (Physics.BoxCast(startPoint, TerrainController.PlaceableObjectSizes[prefabType], Vector3.down, out boxHit, orientation) && boxHit.collider.CompareTag("Terrain")) {
                    Instantiate(TerrainController.PlaceableObjects[prefabType], new Vector3(startPoint.x, hit.point.y, startPoint.z), orientation, transform);
                }
                //Debug code. To use, uncomment the giant thingy below
                //Debug.DrawRay(startPoint, Vector3.down * 10000, Color.blue);
                //DrawBoxCastBox(startPoint, TerrainController.PlaceableObjectSizes[prefabType], orientation, Vector3.down, 10000, Color.red);
                //UnityEditor.EditorApplication.isPaused = true;
            }

        }
    }


    private int GetIndexOfItemToSpawn()
    {
        // Calcular ratios que quiero
        // Arbol 80%, Piedras 18%, Tronquito 2%
        int diceRoll = Random.Range(1,101);
        string caseIndex = "";     

        // Variables min max Calculadas.
        int minArbol = 1;    
        int maxArbol = PArbol + minArbol -1;
        int minPiedras = maxArbol + 1;
        int maxPiedras = minPiedras + PPiedras -1;
        int minTronquito = maxPiedras + 1;
        int maxTronquito = minTronquito + PTronquito -1; // = 100

        // Casos
        if(diceRoll >= minArbol && diceRoll <= maxArbol){   // 1 18
            caseIndex = "arbol";
        }
        else if(diceRoll >= minPiedras && diceRoll <= maxPiedras){  // 19 21
            caseIndex = "piedra";
        }
        else if(diceRoll >= minTronquito && diceRoll <= maxTronquito){ // 22 100
            caseIndex = "tronquito";
        }

        // Como actuar dependiendo el caso.
        switch(caseIndex){
            case "arbol":
                return Random.Range(0,4);
            case "tronquito":
                return 7;
            case "piedra":
                return Random.Range(4,7);
            default:
                Debug.Log("Error on GetIndexOfItemToSpawn, case is default");
                return 0;
        }
    }

    private Vector3 RandomPointAboveTerrain() {
        return new Vector3(
            Random.Range(transform.position.x - TerrainController.TerrainSize.x / 2, transform.position.x + TerrainController.TerrainSize.x / 2),
            transform.position.y + TerrainController.TerrainSize.y * 2,
            Random.Range(transform.position.z - TerrainController.TerrainSize.z / 2, transform.position.z + TerrainController.TerrainSize.z / 2)
        );
    }

    //code to help visualize the boxcast
    //source: https://answers.unity.com/questions/1156087/how-can-you-visualize-a-boxcast-boxcheck-etc.html
    /*
    //Draws just the box at where it is currently hitting.
    public static void DrawBoxCastOnHit(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float hitInfoDistance, Color color) {
        origin = CastCenterOnCollision(origin, direction, hitInfoDistance);
        DrawBox(origin, halfExtents, orientation, color);
    }

    //Draws the full box from start of cast to its end distance. Can also pass in hitInfoDistance instead of full distance
    public static void DrawBoxCastBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float distance, Color color) {
        direction.Normalize();
        Box bottomBox = new Box(origin, halfExtents, orientation);
        Box topBox = new Box(origin + (direction * distance), halfExtents, orientation);

        Debug.DrawLine(bottomBox.backBottomLeft, topBox.backBottomLeft, color);
        Debug.DrawLine(bottomBox.backBottomRight, topBox.backBottomRight, color);
        Debug.DrawLine(bottomBox.backTopLeft, topBox.backTopLeft, color);
        Debug.DrawLine(bottomBox.backTopRight, topBox.backTopRight, color);
        Debug.DrawLine(bottomBox.frontTopLeft, topBox.frontTopLeft, color);
        Debug.DrawLine(bottomBox.frontTopRight, topBox.frontTopRight, color);
        Debug.DrawLine(bottomBox.frontBottomLeft, topBox.frontBottomLeft, color);
        Debug.DrawLine(bottomBox.frontBottomRight, topBox.frontBottomRight, color);

        DrawBox(bottomBox, color);
        DrawBox(topBox, color);
    }

    public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color) {
        DrawBox(new Box(origin, halfExtents, orientation), color);
    }
    public static void DrawBox(Box box, Color color) {
        Debug.DrawLine(box.frontTopLeft, box.frontTopRight, color);
        Debug.DrawLine(box.frontTopRight, box.frontBottomRight, color);
        Debug.DrawLine(box.frontBottomRight, box.frontBottomLeft, color);
        Debug.DrawLine(box.frontBottomLeft, box.frontTopLeft, color);

        Debug.DrawLine(box.backTopLeft, box.backTopRight, color);
        Debug.DrawLine(box.backTopRight, box.backBottomRight, color);
        Debug.DrawLine(box.backBottomRight, box.backBottomLeft, color);
        Debug.DrawLine(box.backBottomLeft, box.backTopLeft, color);

        Debug.DrawLine(box.frontTopLeft, box.backTopLeft, color);
        Debug.DrawLine(box.frontTopRight, box.backTopRight, color);
        Debug.DrawLine(box.frontBottomRight, box.backBottomRight, color);
        Debug.DrawLine(box.frontBottomLeft, box.backBottomLeft, color);
    }

    public struct Box {
        public Vector3 localFrontTopLeft { get; private set; }
        public Vector3 localFrontTopRight { get; private set; }
        public Vector3 localFrontBottomLeft { get; private set; }
        public Vector3 localFrontBottomRight { get; private set; }
        public Vector3 localBackTopLeft { get { return -localFrontBottomRight; } }
        public Vector3 localBackTopRight { get { return -localFrontBottomLeft; } }
        public Vector3 localBackBottomLeft { get { return -localFrontTopRight; } }
        public Vector3 localBackBottomRight { get { return -localFrontTopLeft; } }

        public Vector3 frontTopLeft { get { return localFrontTopLeft + origin; } }
        public Vector3 frontTopRight { get { return localFrontTopRight + origin; } }
        public Vector3 frontBottomLeft { get { return localFrontBottomLeft + origin; } }
        public Vector3 frontBottomRight { get { return localFrontBottomRight + origin; } }
        public Vector3 backTopLeft { get { return localBackTopLeft + origin; } }
        public Vector3 backTopRight { get { return localBackTopRight + origin; } }
        public Vector3 backBottomLeft { get { return localBackBottomLeft + origin; } }
        public Vector3 backBottomRight { get { return localBackBottomRight + origin; } }

        public Vector3 origin { get; private set; }

        public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents) {
            Rotate(orientation);
        }
        public Box(Vector3 origin, Vector3 halfExtents) {
            this.localFrontTopLeft = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
            this.localFrontTopRight = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
            this.localFrontBottomLeft = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
            this.localFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);

            this.origin = origin;
        }


        public void Rotate(Quaternion orientation) {
            localFrontTopLeft = RotatePointAroundPivot(localFrontTopLeft, Vector3.zero, orientation);
            localFrontTopRight = RotatePointAroundPivot(localFrontTopRight, Vector3.zero, orientation);
            localFrontBottomLeft = RotatePointAroundPivot(localFrontBottomLeft, Vector3.zero, orientation);
            localFrontBottomRight = RotatePointAroundPivot(localFrontBottomRight, Vector3.zero, orientation);
        }
    }

    //This should work for all cast types
    static Vector3 CastCenterOnCollision(Vector3 origin, Vector3 direction, float hitInfoDistance) {
        return origin + (direction.normalized * hitInfoDistance);
    }

    static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation) {
        Vector3 direction = point - pivot;
        return pivot + rotation * direction;
    }
    */
    // CODIGO DE SPAWN VIEJO
    /*
    if(diceRoll >= 1 && diceRoll <= 18){   // 1 18
            caseIndex = "piedra";
        }
        else if(diceRoll >= 19 && diceRoll <= 21){  // 19 21
            caseIndex = "tronquito";
        }
        else if(diceRoll >= 22 && diceRoll <= 100){ // 22 100
            caseIndex = "arbol";
        }
        */
}