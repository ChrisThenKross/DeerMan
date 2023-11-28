using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.AI.Navigation;
using UnityEngine;

[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]
[RequireComponent (typeof (MeshCollider))]
public class MeshGenerator : MonoBehaviour {
    public SquareGrid squareGrid;
    List<Vector3> vertices;
    List<int> triangles;

    public List<GameObject> environmentObjects = new();

    // Object to instantiate
    public GameObject floor;
    public GameObject exit;
    public GameObject wall;

    TileType[, ] map;

    public float wallHeight = 1;
    public float PoissonRadius = 10;
    private float squareSize = 1;

    private Vector3 exitPos;

    void OnValidate () {
        if (map != null) {
            GenerateMesh (map, squareSize);
        }
    }

    public void GenerateMesh (TileType[, ] map, float squareSize) {
        // Move this object up by the wall height
        // Its easier to make a floor than a ceiling
        transform.position = new Vector3 (0, wallHeight, 0);

        var watch = System.Diagnostics.Stopwatch.StartNew ();
        this.map = map;
        this.squareSize = squareSize;

        GenerateWalls();
        ReserVertexIndecies();
        GenerateFloor();

        // Place exit
        for(int i = 0; i < map.GetLength(0); i++){
            for(int j = 0; j < map.GetLength(1); j++){
                if(map[i,j] == TileType.Exit){
                    exitPos = new Vector3 (i, 0, j);
                    exitPos *= squareSize;
                    exitPos.x -= map.GetLength (0) * squareSize / 2f;
                    exitPos.z -= map.GetLength (1) * squareSize / 2f;

                    exit.transform.position = exitPos;
                    exit.transform.parent = transform;
                    exit.transform.localScale = Vector3.one * squareSize;
                    break;
                }
            }
        }


        DistributeEnvironment ();

        watch.Stop ();
        Debug.Log ($"Generated mesh with {vertices.Count} vertices and {triangles.Count / 3} triangles in {watch.ElapsedMilliseconds}ms");

        // Put player somewhere not in a wall
        Vector2 playerPos = new Vector2 (0, 0);
        while (!IsValidPlayerPosition (playerPos)) {
            playerPos = new Vector2 (UnityEngine.Random.Range (0, map.GetLength (0)), UnityEngine.Random.Range (0, map.GetLength (1)));
        }

        Vector3 playerpos3 = new Vector3 (playerPos.x, .5f, playerPos.y);
        playerpos3 *= squareSize;
        playerpos3.x -= map.GetLength (0) * squareSize / 2f;
        playerpos3.z -= map.GetLength (1) * squareSize / 2f;

        GameObject player = GameObject.Find ("Player");
        player.transform.position = playerpos3;

        // Bake NavMesh
        NavMeshSurface navMesh = floor.GetComponent<NavMeshSurface> ();
        navMesh.BuildNavMesh ();
    }

    public void GenerateWalls(){
        squareGrid = new SquareGrid (map, squareSize);
        vertices = new List<Vector3> ();
        triangles = new List<int> ();

        for (int x = 0; x < squareGrid.squares.GetLength (0); x++)
            for (int y = 0; y < squareGrid.squares.GetLength (1); y++)
                TriangulateSquare (squareGrid.squares[x, y], false);

        Mesh mesh = new()
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray()
        };
        mesh.RecalculateNormals ();

        GetComponent<MeshFilter> ().mesh = mesh;
        GenerateMeshCollider (mesh);
    }

    public void GenerateFloor(){
        vertices = new List<Vector3> ();
        triangles = new List<int> ();

        for (int x = 0; x < squareGrid.squares.GetLength (0); x++)
            for (int y = 0; y < squareGrid.squares.GetLength (1); y++)
                TriangulateSquare (squareGrid.squares[x, y], true);

        Mesh mesh = new()
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray()
        };

        mesh.RecalculateNormals ();
        floor.GetComponent<MeshFilter> ().mesh = mesh;
        floor.GetComponent<MeshCollider> ().sharedMesh = mesh;
    }

    private void ReserVertexIndecies () {
        foreach (Square square in squareGrid.squares) {
            square.topLeft.vertexIndex = -1;
            square.topRight.vertexIndex = -1;
            square.bottomRight.vertexIndex = -1;
            square.bottomLeft.vertexIndex = -1;

            square.centerTop.vertexIndex = -1;
            square.centerRight.vertexIndex = -1;
            square.centerBottom.vertexIndex = -1;
            square.centerLeft.vertexIndex = -1;
        }
    }

    void GenerateMeshCollider (Mesh mesh) {
        MeshCollider meshCollider = GetComponent<MeshCollider> ();
        if (meshCollider == null) {
            meshCollider = gameObject.AddComponent<MeshCollider> ();
        }
        meshCollider.sharedMesh = mesh;
    }

    bool IsValidPlayerPosition (Vector2 pos) {
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                int x = (int) pos.x + i;
                int y = (int) pos.y + j;
                if (x >= 0 && x < map.GetLength (0) && y >= 0 && y < map.GetLength (1)) {
                    if (map[x, y] == TileType.Wall) {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    void TriangulateSquare (Square square, bool invert) {
        // Use this lambda so that we can optionally generate walls
        // without the use of an if statement for each call
        Action<Node, Node> TryCreateWall = (Node a, Node b) => {
            if (!invert) {
                CreateWall (a, b);
            }
        };

        int configuration = square.configuration;
        if (invert) {
            // Negate state of each node and mask last 4 bits
            configuration = ~configuration & 15;
        }

        switch (configuration) {
            case 0:
                break;

                // 1 points:
            case 1:
                MeshFromPoints (square.centerBottom, square.bottomLeft, square.centerLeft);
                TryCreateWall (square.centerLeft, square.centerBottom);
                break;
            case 2:
                MeshFromPoints (square.centerRight, square.bottomRight, square.centerBottom);
                TryCreateWall (square.centerBottom, square.centerRight);
                break;
            case 4:
                MeshFromPoints (square.centerTop, square.topRight, square.centerRight);
                TryCreateWall (square.centerRight, square.centerTop);
                break;
            case 8:
                MeshFromPoints (square.topLeft, square.centerTop, square.centerLeft);
                TryCreateWall (square.centerTop, square.centerLeft);
                break;

                // 2 points:
            case 3:
                MeshFromPoints (square.centerRight, square.bottomRight, square.bottomLeft, square.centerLeft);
                TryCreateWall (square.centerLeft, square.centerRight);
                break;
            case 6:
                MeshFromPoints (square.centerTop, square.topRight, square.bottomRight, square.centerBottom);
                TryCreateWall (square.centerBottom, square.centerTop);
                break;
            case 9:
                MeshFromPoints (square.topLeft, square.centerTop, square.centerBottom, square.bottomLeft);
                TryCreateWall (square.centerTop, square.centerBottom);
                break;
            case 12:
                MeshFromPoints (square.topLeft, square.topRight, square.centerRight, square.centerLeft);
                TryCreateWall (square.centerRight, square.centerLeft);
                break;
            case 5:
                MeshFromPoints (square.centerTop, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft, square.centerLeft);
                TryCreateWall (square.centerLeft, square.centerTop);
                TryCreateWall (square.centerRight, square.centerBottom);
                break;
            case 10:
                MeshFromPoints (square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.centerBottom, square.centerLeft);
                TryCreateWall (square.centerBottom, square.centerLeft);
                TryCreateWall (square.centerRight, square.centerTop);
                break;

                // 3 point:
            case 7:
                MeshFromPoints (square.centerTop, square.topRight, square.bottomRight, square.bottomLeft, square.centerLeft);
                TryCreateWall (square.centerLeft, square.centerTop);
                break;
            case 11:
                MeshFromPoints (square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.bottomLeft);
                TryCreateWall (square.centerTop, square.centerRight);
                break;
            case 13:
                MeshFromPoints (square.topLeft, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft);
                TryCreateWall (square.centerRight, square.centerBottom);
                break;
            case 14:
                MeshFromPoints (square.topLeft, square.topRight, square.bottomRight, square.centerBottom, square.centerLeft);
                TryCreateWall (square.centerBottom, square.centerLeft);
                break;

                // 4 point:
            case 15:
                MeshFromPoints (square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
                break;
        }
    }

    void DistributeEnvironment () {
        List<Vector2> points = PoissonDiscSampling (100);

        foreach (Vector2 point in points) {
            Vector3 pos = new Vector3 (point.x, 0, point.y);
            List<GameObject> possibleObjects = new();

            // Add all objects that can be spawned
            foreach (GameObject obj in environmentObjects) {
                // Calculate 4 points on bounding box
                Vector3[] pointsOnBox = new Vector3[4];
                pointsOnBox[0] = pos + new Vector3 (obj.transform.localScale.x / 2f, 0, obj.transform.localScale.z / 2f);
                pointsOnBox[1] = pos + new Vector3 (-obj.transform.localScale.x / 2f, 0, obj.transform.localScale.z / 2f);
                pointsOnBox[2] = pos + new Vector3 (obj.transform.localScale.x / 2f, 0, -obj.transform.localScale.z / 2f);
                pointsOnBox[3] = pos + new Vector3 (-obj.transform.localScale.x / 2f, 0, -obj.transform.localScale.z / 2f);

                // Make sure exit is not in bounding box
                if (Vector3.Distance (pos, exitPos) < obj.transform.localScale.x) {
                    continue;
                }

                // Make sure no points are in walls or near walls
                bool valid = true;
                foreach (Vector3 pointOnBox in pointsOnBox) {
                    int x = (int) pointOnBox.x;
                    int y = (int) pointOnBox.z;

                    for(int xx = -1; xx <= 1; xx++){
                        for(int yy = -1; yy <= 1; yy++){
                            if (x + xx >= 0 && x + xx < map.GetLength (0) && y + yy >= 0 && y + yy < map.GetLength (1)) {
                                if (map[x + xx, y + yy] == TileType.Wall || map[x + xx, y + yy] == TileType.Exit) {
                                    valid = false;
                                    break;
                                }
                            }
                        }
                    }
                }

                if(valid){
                    possibleObjects.Add(obj);
                }
            }

            // Adjust to map parameters
            pos *= squareSize;
            pos.x -= map.GetLength (0) * squareSize / 2f;
            pos.z -= map.GetLength (1) * squareSize / 2f;

            Color c = possibleObjects.Count > 0 ? Color.green : Color.red;
            Debug.DrawLine (pos, pos + Vector3.up * 5, c, 100f);

            // Choose from possible objects
            if(possibleObjects.Count == 0){
                continue;
            }

            GameObject objectToSpawn = possibleObjects[UnityEngine.Random.Range (0, possibleObjects.Count)];

            // Rotate randomly in 90 degree increments
            int rot = UnityEngine.Random.Range (0, 4);

            GameObject instance = Instantiate (objectToSpawn, pos, Quaternion.Euler (0, rot * 90, 0));
            instance.transform.parent = transform;
            instance.transform.localScale = Vector3.one * squareSize;
        }
    }

    List<Vector2> PoissonDiscSampling (int attempts = 30) {
        float cellSize = PoissonRadius / Mathf.Sqrt (2);
        int[, ] grid = new int[Mathf.CeilToInt (squareGrid.squares.GetLength (0) / cellSize), Mathf.CeilToInt (squareGrid.squares.GetLength (1) / cellSize)];

        List<Vector2> points = new List<Vector2> ();
        List<Vector2> spawnPoints = new List<Vector2> ();

        spawnPoints.Add (new Vector2 (squareGrid.squares.GetLength (0) / 2, squareGrid.squares.GetLength (1) / 2));
        while (spawnPoints.Count > 0) {
            int spawnIndex = UnityEngine.Random.Range (0, spawnPoints.Count);
            Vector2 spawnCentre = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            for (int i = 0; i < attempts; i++) {
                float angle = UnityEngine.Random.value * Mathf.PI * 2;
                Vector2 dir = new Vector2 (Mathf.Sin (angle), Mathf.Cos (angle));
                Vector2 candidate = spawnCentre + dir * UnityEngine.Random.Range (PoissonRadius, 2 * PoissonRadius);

                if (IsValid (candidate, cellSize, points, grid)) {
                    points.Add (candidate);
                    spawnPoints.Add (candidate);
                    grid[(int) (candidate.x / cellSize), (int) (candidate.y / cellSize)] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }

            if (!candidateAccepted) {
                spawnPoints.RemoveAt (spawnIndex);
            }
        }

        return points;
    }

    bool IsValid (Vector2 candidate, float cellSize, List<Vector2> points, int[, ] grid) {
        if (candidate.x >= 0 && candidate.x < squareGrid.squares.GetLength (0) && candidate.y >= 0 && candidate.y < squareGrid.squares.GetLength (1)) {
            int cellX = (int) (candidate.x / cellSize);
            int cellY = (int) (candidate.y / cellSize);

            int searchStartX = Mathf.Max (0, cellX - 2);
            int searchEndX = Mathf.Min (cellX + 2, grid.GetLength (0) - 1);
            int searchStartY = Mathf.Max (0, cellY - 2);
            int searchEndY = Mathf.Min (cellY + 2, grid.GetLength (1) - 1);

            // Calculate nearest distance to wall in map
            int mapX = (int) candidate.x;
            int mapY = (int) candidate.y;
            if (mapX >= 0 && mapX < map.GetLength (0) && mapY >= 0 && mapY < map.GetLength (1)) {
                if (map[mapX, mapY] == TileType.Wall || map[mapX, mapY] == TileType.Exit) {
                    return false;
                }
            }

            for (int x = searchStartX; x <= searchEndX; x++) {
                for (int y = searchStartY; y <= searchEndY; y++) {
                    int pointIndex = grid[x, y] - 1;
                    if (pointIndex != -1) {
                        float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                        if (sqrDst < PoissonRadius * PoissonRadius) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        return false;
    }

    void CreateWall (Node a, Node b) {
        Vector3 pos = (a.position + b.position) / 2f;
        pos.y -= .01f; // Eliminate z-fighting
        GameObject instance = Instantiate (wall, pos, Quaternion.identity);
        instance.transform.parent = transform;

        // Rotate wall to face correct direction
        Vector3 c = a.position + Vector3.down * wallHeight;
        // The plane it needs to be on is the plane that is the plane a,b,c
        // So we can use the cross product to get the normal of that plane
        Vector3 normal = Vector3.Cross (b.position - a.position, c - a.position);
        // The wall should be facing the inverse of the normal
        instance.transform.rotation = Quaternion.LookRotation (-normal);

        // Scale wall to correct size with wall height
        instance.transform.localScale = new Vector3 (.25f, 1, 1);

    }

    void MeshFromPoints (params Node[] points) {
        AssignVertices (points);

        if (points.Length >= 3)
            CreateTriangle (points[0], points[1], points[2]);
        if (points.Length >= 4)
            CreateTriangle (points[0], points[2], points[3]);
        if (points.Length >= 5)
            CreateTriangle (points[0], points[3], points[4]);
        if (points.Length >= 6)
            CreateTriangle (points[0], points[4], points[5]);
    }

    void AssignVertices (params Node[] points) {
        foreach (Node n in points) {
            if (n.vertexIndex == -1) {
                n.vertexIndex = vertices.Count;
                vertices.Add (n.position);
            }
        }
    }

    void CreateTriangle (Node a, Node b, Node c) {
        triangles.Add (a.vertexIndex);
        triangles.Add (b.vertexIndex);
        triangles.Add (c.vertexIndex);
    }

    public class SquareGrid {
        public Square[, ] squares;
        private float squareSize;

        public SquareGrid () { }

        public SquareGrid (TileType[, ] map, float squareSize) {
            int nodeCountX = map.GetLength (0);
            int nodeCountY = map.GetLength (1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControlNode[, ] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; x++) {
                for (int y = 0; y < nodeCountY; y++) {
                    Vector3 pos = new Vector3 (-mapWidth / 2 + x * squareSize + squareSize / 2, 0, -mapHeight / 2 + y * squareSize + squareSize / 2);
                    controlNodes[x, y] = new ControlNode (pos, map[x,y] == TileType.Wall, squareSize);
                }
            }

            squares = new Square[nodeCountX - 1, nodeCountY - 1];
            for (int x = 0; x < nodeCountX - 1; x++) {
                for (int y = 0; y < nodeCountY - 1; y++) {
                    squares[x, y] = new Square (controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
                }
            }
        }
    }

    public class Square {
        public ControlNode topLeft, topRight, bottomRight, bottomLeft;
        public Node centerTop, centerRight, centerBottom, centerLeft;
        public int configuration;

        public Square (ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft) {
            topLeft = _topLeft;
            topRight = _topRight;
            bottomRight = _bottomRight;
            bottomLeft = _bottomLeft;

            centerTop = topLeft.right;
            centerRight = bottomRight.above;
            centerBottom = bottomLeft.right;
            centerLeft = bottomLeft.above;

            if (topLeft.active)
                configuration += 8;
            if (topRight.active)
                configuration += 4;
            if (bottomRight.active)
                configuration += 2;
            if (bottomLeft.active)
                configuration += 1;
        }
    }

    public class Node {
        public Vector3 position;
        public int vertexIndex = -1;

        public Node (Vector3 _pos) {
            position = _pos;
        }
    }

    public class ControlNode : Node {
        public bool active;
        public Node above, right;
        private float squareSize;

        public ControlNode (Vector3 _pos, bool _active, float squareSize) : base (_pos) {
            active = _active;
            above = new Node (position + Vector3.forward * squareSize / 2f);
            right = new Node (position + Vector3.right * squareSize / 2f);
        }
    }
}