using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MeshFilter))]
public class MeshGenerator : MonoBehaviour {
    public SquareGrid squareGrid;
    List<Vector3> vertices;
    List<int> triangles;

    List<GameObject> trees = new List<GameObject> ();

    // Object to instantiate
    public GameObject tree;

    int[, ] map;

    public float wallHeight = 1;
    public float radius = 1;
    private float squareSize = 1;

    void OnValidate () {
        if (map != null) {
            GenerateMesh (map, squareSize);
        }
    }

    //TODO: Add floor mesh
    public void GenerateMesh (int[, ] map, float squareSize) {
        // Move this object up by the wall height
        // Its easier to make a floor than a ceiling
        transform.position = new Vector3 (0, wallHeight, 0);

        var watch = System.Diagnostics.Stopwatch.StartNew ();
        this.map = map;
        this.squareSize = squareSize;

        ResetTrees ();

        squareGrid = new SquareGrid (map, squareSize);
        vertices = new List<Vector3> ();
        triangles = new List<int> ();

        for (int x = 0; x < squareGrid.squares.GetLength (0); x++)
            for (int y = 0; y < squareGrid.squares.GetLength (1); y++)
                TriangulateSquare (squareGrid.squares[x, y]);

        DistributeTrees ();

        Mesh mesh = new Mesh ();
        GetComponent<MeshFilter> ().mesh = mesh;

        mesh.vertices = vertices.ToArray ();
        mesh.triangles = triangles.ToArray ();
        mesh.RecalculateNormals ();

        watch.Stop ();
        Debug.Log ($"Generated mesh with {vertices.Count} vertices and {triangles.Count / 3} triangles in {watch.ElapsedMilliseconds}ms");

    }

    void ResetTrees () {
        foreach (GameObject tree in trees) {
            Destroy (tree);
        }

        trees.Clear ();
    }

    void TriangulateSquare (Square square) {
        switch (square.configuration) {
            case 0:
                break;

                // 1 points:
            case 1:
                MeshFromPoints (square.centerBottom, square.bottomLeft, square.centerLeft);
                CreateWall (square.centerLeft, square.centerBottom);
                break;
            case 2:
                MeshFromPoints (square.centerRight, square.bottomRight, square.centerBottom);
                CreateWall (square.centerBottom, square.centerRight);
                break;
            case 4:
                MeshFromPoints (square.centerTop, square.topRight, square.centerRight);
                CreateWall (square.centerRight, square.centerTop);
                break;
            case 8:
                MeshFromPoints (square.topLeft, square.centerTop, square.centerLeft);
                CreateWall (square.centerTop, square.centerLeft);
                break;

                // 2 points:
            case 3:
                MeshFromPoints (square.centerRight, square.bottomRight, square.bottomLeft, square.centerLeft);
                CreateWall (square.centerLeft, square.centerRight);
                break;
            case 6:
                MeshFromPoints (square.centerTop, square.topRight, square.bottomRight, square.centerBottom);
                CreateWall (square.centerBottom, square.centerTop);
                break;
            case 9:
                MeshFromPoints (square.topLeft, square.centerTop, square.centerBottom, square.bottomLeft);
                CreateWall (square.centerTop, square.centerBottom);
                break;
            case 12:
                MeshFromPoints (square.topLeft, square.topRight, square.centerRight, square.centerLeft);
                CreateWall (square.centerRight, square.centerLeft);
                break;
            case 5:
                MeshFromPoints (square.centerTop, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft, square.centerLeft);
                CreateWall (square.centerLeft, square.centerTop);
                CreateWall (square.centerRight, square.centerBottom);
                break;
            case 10:
                MeshFromPoints (square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.centerBottom, square.centerLeft);
                CreateWall (square.centerBottom, square.centerLeft);
                CreateWall (square.centerRight, square.centerTop);
                break;

                // 3 point:
            case 7:
                MeshFromPoints (square.centerTop, square.topRight, square.bottomRight, square.bottomLeft, square.centerLeft);
                CreateWall (square.centerLeft, square.centerTop);
                break;
            case 11:
                MeshFromPoints (square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.bottomLeft);
                CreateWall (square.centerTop, square.centerRight);
                break;
            case 13:
                MeshFromPoints (square.topLeft, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft);
                CreateWall (square.centerRight, square.centerBottom);
                break;
            case 14:
                MeshFromPoints (square.topLeft, square.topRight, square.bottomRight, square.centerBottom, square.centerLeft);
                CreateWall (square.centerBottom, square.centerLeft);
                break;

                // 4 point:
            case 15:
                MeshFromPoints (square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
                break;
        }
    }

    void DistributeTrees () {
        List<Vector2> points = PoissonDiscSampling (30);

        foreach (Vector2 point in points) {
            Vector3 pos = new Vector3 (point.x, 0, point.y);
            pos *= squareSize;
            // pos += Vector3.up * 0.5f;

            // Center on map
            pos.x -= map.GetLength (0) * squareSize / 2f;
            pos.z -= map.GetLength (1) * squareSize / 2f;
            // pos.y -= wallHeight;

            GameObject treeInstance = Instantiate (tree, pos, Quaternion.identity);
            treeInstance.transform.parent = transform;
            treeInstance.transform.localScale = Vector3.one * squareSize;

            trees.Add (treeInstance);
        }
    }

    List<Vector2> PoissonDiscSampling (int attempts = 30) {
        float cellSize = radius / Mathf.Sqrt (2);
        Debug.Log ($"cellSize: {cellSize}");

        int[, ] grid = new int[Mathf.CeilToInt (squareGrid.squares.GetLength (0) / cellSize), Mathf.CeilToInt (squareGrid.squares.GetLength (1) / cellSize)];

        Debug.Log ($"grid: {grid.GetLength(0)}, {grid.GetLength(1)}");

        List<Vector2> points = new List<Vector2> ();
        List<Vector2> spawnPoints = new List<Vector2> ();

        spawnPoints.Add (new Vector2 (squareGrid.squares.GetLength (0) / 2, squareGrid.squares.GetLength (1) / 2));
        while (spawnPoints.Count > 0) {
            int spawnIndex = Random.Range (0, spawnPoints.Count);
            Vector2 spawnCentre = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            for (int i = 0; i < attempts; i++) {
                float angle = Random.value * Mathf.PI * 2;
                Vector2 dir = new Vector2 (Mathf.Sin (angle), Mathf.Cos (angle));
                Vector2 candidate = spawnCentre + dir * Random.Range (radius, 2 * radius);

                if (IsValid (candidate, cellSize, radius, points, grid)) {
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

    bool IsValid (Vector2 candidate, float cellSize, float radius, List<Vector2> points, int[, ] grid) {
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
                if (map[mapX, mapY] == 1) {
                    return false;
                }
            }

            for (int x = searchStartX; x <= searchEndX; x++) {
                for (int y = searchStartY; y <= searchEndY; y++) {
                    int pointIndex = grid[x, y] - 1;
                    if (pointIndex != -1) {
                        float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                        if (sqrDst < radius * radius) {
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
        // Create a node above either node
        Node c = new Node (a.position + Vector3.down * wallHeight);
        Node d = new Node (b.position + Vector3.down * wallHeight);

        // There is some really wierd bug when assigning vertices on large sizes
        AssignVertices (a, b, c, d);
        CreateTriangle (b, a, c);
        CreateTriangle (b, c, d);
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

        public SquareGrid (int[, ] map, float squareSize) {
            int nodeCountX = map.GetLength (0);
            int nodeCountY = map.GetLength (1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControlNode[, ] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; x++) {
                for (int y = 0; y < nodeCountY; y++) {
                    Vector3 pos = new Vector3 (-mapWidth / 2 + x * squareSize + squareSize / 2, 0, -mapHeight / 2 + y * squareSize + squareSize / 2);
                    controlNodes[x, y] = new ControlNode (pos, map[x, y] == 1, squareSize);
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

        public ControlNode (Vector3 _pos, bool _active, float squareSize) : base (_pos) {
            active = _active;
            above = new Node (position + Vector3.forward * squareSize / 2f);
            right = new Node (position + Vector3.right * squareSize / 2f);
        }
    }
}