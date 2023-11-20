using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum TileType {
    Wall = 0,
    Floor = 1,
    Passage = 2
}

public class MapGenerator : MonoBehaviour {
    public int width = 70;
    public int height = 40;
    public string seed = "0";
    public bool useRandomSeed;
    public int borderSize = 5;
    public int smoothSteps = 5;
    public float squareSize = .5f;
    public int passageWidth = 1;

    [Range (0, 100)]
    public int randomFillPercent = 50;

    TileType[, ] map;

    void Start () {
        GenerateMap ();
    }

    void Update () {
        if (Input.GetMouseButtonDown (0)) {
            // Start timer
            var watch = System.Diagnostics.Stopwatch.StartNew ();
            GenerateMap ();
            // Stop timer
            watch.Stop ();
            Debug.Log ($"Generated map in {watch.ElapsedMilliseconds}ms");
        }

        // Keep enemies spawned
        MainMapGameManager gameManager = GetComponent<MainMapGameManager> ();
        gameManager.UpdateEnemies (map, squareSize);
    }

    // void OnValidate () {
    //     GenerateMap ();
    // }

    void GenerateMap () {
        map = new TileType[width, height];
        RandomFillMap ();

        for (int i = 0; i < smoothSteps; i++) {
            SmoothMap ();
        }

        TileType[, ] borderedMap = new TileType[width + borderSize * 2, height + borderSize * 2];
        for (int i = 0; i < borderedMap.GetLength (0); i++) {
            for (int j = 0; j < borderedMap.GetLength (1); j++) {
                if (i >= borderSize && i < width + borderSize && j >= borderSize && j < height + borderSize) {
                    borderedMap[i, j] = map[i - borderSize, j - borderSize];
                    continue;
                }

                borderedMap[i, j] = TileType.Wall;
            }
        }

        ConnectRegions (borderedMap);

        MeshGenerator meshGen = GetComponent<MeshGenerator> ();
        meshGen.GenerateMesh (borderedMap, squareSize);
    }

    void RandomFillMap () {
        if (useRandomSeed) {
            seed = Time.time.ToString ();
        }

        System.Random pseudoRandom = new System.Random (seed.GetHashCode ());

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
                    map[x, y] = TileType.Wall;
                } else {
                    map[x, y] = (pseudoRandom.Next (0, 100) < randomFillPercent) ? TileType.Wall : TileType.Floor;
                }
            }
        }
    }

    int GetSurroundingWallCount (int gridX, int gridY) {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
                    if (neighbourX != gridX || neighbourY != gridY) {
                        wallCount += map[neighbourX, neighbourY] == TileType.Wall ? 1 : 0;
                    }
                } else {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    void SmoothMap () {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {

                int neighbourWallTiles = GetSurroundingWallCount (x, y);

                if (neighbourWallTiles > 4)
                    map[x, y] = TileType.Wall;
                else if (neighbourWallTiles < 4)
                    map[x, y] = TileType.Floor;

            }
        }
    }

    void ConnectRegions (TileType[, ] map) {
        // Use flood fill to find all the regions
        bool[, ] visited = new bool[map.GetLength (0), map.GetLength (1)];
        List<Region> regions = new List<Region> ();

        var watch = System.Diagnostics.Stopwatch.StartNew ();
        for (int x = 0; x < map.GetLength (0); x++) {
            for (int y = 0; y < map.GetLength (1); y++) {
                if (!visited[x, y] && map[x, y] != TileType.Wall) {
                    Region region = new Region (regions.Count + 1);
                    FloodFillQueue (map, visited, x, y, region);
                    regions.Add (region);
                }
            }
        }
        watch.Stop ();

        Debug.Log ($"Found {regions.Count} regions in {watch.ElapsedMilliseconds}ms");
        if (regions.Count <= 1)
            return;

        watch = System.Diagnostics.Stopwatch.StartNew ();
        RegionConnection[, ] dists = GetDists (regions);
        watch.Stop ();
        Debug.Log ($"Generated dists in {watch.ElapsedMilliseconds}ms");

        List<RegionConnection> mst = new List<RegionConnection> ();
        HashSet<int> visitedRegions = new HashSet<int> { 1 };

        // Generate MST
        // Prim's algorithm
        watch = System.Diagnostics.Stopwatch.StartNew ();
        while (visitedRegions.Count < regions.Count) {
            int minDist = int.MaxValue;
            int minI = -1;
            int minJ = -1;

            foreach (int i in visitedRegions) {
                for (int j = 0; j < regions.Count; j++) {
                    if (visitedRegions.Contains (j + 1)) continue;

                    if (dists[i - 1, j].dist < minDist) {
                        minDist = dists[i - 1, j].dist;
                        minI = i;
                        minJ = j + 1;
                    }
                }
            }

            visitedRegions.Add (minJ);
            mst.Add (dists[minI - 1, minJ - 1]);

            // Draw debug line to start and end of path
            // var rc = dists[minI - 1, minJ - 1];
            // Debug.DrawLine (TileWorldPos (rc.aTile), TileWorldPos (rc.bTile), Color.red, 10000f);
        }
        watch.Stop ();
        Debug.Log ($"Generated MST in {watch.ElapsedMilliseconds}ms");

        // Draw MST
        foreach (RegionConnection rc in mst) {
            rc.Connect (map, passageWidth, width + borderSize * 2, height + borderSize * 2);
        }
    }

    void FloodFillQueue(TileType[, ] map, bool[, ] visited, int x, int y, Region region){
        Queue<(int, int)> q = new Queue<(int, int)>();
        q.Enqueue((x, y));

        while(q.Count > 0){
            (int, int) pos = q.Dequeue();
            x = pos.Item1;
            y = pos.Item2;

            if (x < 0 || y < 0 || x >= map.GetLength (0) || y >= map.GetLength (1)) {
                continue;
            }

            // Add all non wall tiles to this region
            if (visited[x, y] || map[x, y] == TileType.Wall) {
                continue;
            }

            visited[x, y] = true;
            region.AddTile (new Tile (x, y, region.id), map);

            q.Enqueue((x - 1, y));
            q.Enqueue((x + 1, y));
            q.Enqueue((x, y - 1));
            q.Enqueue((x, y + 1));
        }
    }

    RegionConnection[, ] GetDists (List<Region> regions) {
        RegionConnection[, ] dists = new RegionConnection[regions.Count, regions.Count];
        for (int i = 0; i < regions.Count; i++) {
            for (int j = i + 1; j < regions.Count; j++) {
                Region a = regions[i];
                Region b = regions[j];

                if (a.id == b.id) continue;

                RegionConnection rc = new RegionConnection {
                    a = a,
                    b = b
                };

                double minDist = double.MaxValue;

                // This is awful
                foreach (Tile aTile in a.edgeTiles) {
                    foreach (Tile bTile in b.edgeTiles) {
                        double distSquared = (aTile.x - bTile.x) * (aTile.x - bTile.x) + (aTile.y - bTile.y) * (aTile.y - bTile.y);
                        if (distSquared < minDist) {
                            minDist = distSquared;
                            rc.aTile = aTile;
                            rc.bTile = bTile;
                        }
                    }
                }

                rc.dist = (int) minDist;
                dists[a.id - 1, b.id - 1] = rc;
                dists[b.id - 1, a.id - 1] = rc;
            }
        }

        return dists;
    }

    Vector3 TileWorldPos (Tile t) {
        return new Vector3 (-(width + borderSize * 2) / 2 + .5f + t.x, 2, -(height + borderSize * 2) / 2 + .5f + t.y) * squareSize;
    }

}

class Region {
    public int id;
    public List<Tile> tiles;
    public List<Tile> edgeTiles;

    public Region (int id) {
        this.id = id;
        tiles = new List<Tile> ();
        edgeTiles = new List<Tile> ();
    }

    public void AddTile (Tile tile, TileType[, ] map) {
        tiles.Add (tile);

        int x = tile.x;
        int y = tile.y;

        bool upWall = y < map.GetLength (1) - 1 && map[x, y + 1] == TileType.Wall;
        bool downWall = y > 0 && map[x, y - 1] == TileType.Wall;
        bool leftWall = x > 0 && map[x - 1, y] == TileType.Wall;
        bool rightWall = x < map.GetLength (0) - 1 && map[x + 1, y] == TileType.Wall;

        if (upWall || downWall || leftWall || rightWall) {
            edgeTiles.Add (tile);
        }
    }
}

class Tile {
    public int x;
    public int y;
    public int region;

    public Tile (int x, int y, int region) {
        this.x = x;
        this.y = y;
        this.region = region;
    }
}

class RegionConnection {
    public Region a;
    public Region b;
    public Tile aTile;
    public Tile bTile;
    public int dist;

    public void Connect (TileType[, ] map, int r, int width, int height) {
        List<Tile> line = GetLine ();

        // Make a wider path
        foreach (Tile tile in line) {
            for (int x = -r; x <= r; x++) {
                for (int y = -r; y <= r; y++) {
                    int xx = tile.x + x;
                    int yy = tile.y + y;

                    if (xx <= 0 || yy <= 0 || xx >= width || yy >= height) continue;

                    map[xx, yy] = TileType.Passage;
                }
            }
        }
    }

    List<Tile> GetLine () {
        List<Tile> line = new List<Tile> ();

        int x = aTile.x;
        int y = aTile.y;

        int dx = bTile.x - aTile.x;
        int dy = bTile.y - aTile.y;

        int step = Math.Sign (dx);
        int gradientStep = Math.Sign (dy);

        int longest = Mathf.Abs (dx);
        int shortest = Mathf.Abs (dy);

        bool inverted = false;
        if (longest < shortest) {
            inverted = true;
            step = Math.Sign (dy);
            gradientStep = Math.Sign (dx);
            longest = Mathf.Abs (dy);
            shortest = Mathf.Abs (dx);
        }

        int gradientAccumulation = longest / 2;
        for (int i = 0; i < longest; i++) {
            line.Add (new Tile (x, y, -1));

            if (inverted) {
                y += step;
            } else {
                x += step;
            }

            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest) {
                if (inverted) {
                    x += gradientStep;
                } else {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }

        return line;
    }
}