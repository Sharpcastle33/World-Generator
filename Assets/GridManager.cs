using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public Sprite grass;
    public Sprite grass_hills;
    public Sprite desert;
    public Sprite desert_hills;
    public Sprite boreal;
    public Sprite tundra;
    public Sprite steppe;
    public Sprite forest;
    public Sprite ocean;
    public Sprite arid;
    public Sprite mediterranean;
    public Sprite tropical;

    public int speed = 10; //camera speed

    public float[,] Grid;
    int Vertical, Horizontal, Columns, Rows;
    public int worldx = 100;
    public int worldy = 160;

    public int[,] world;
    public Sprite polar;
    public Sprite mountain;
    public Sprite bigMountain;
    public Sprite littleMountain;
    public Sprite[] coasts;

    public Army?[,] units;
    public Sprite[] unit_sprites;


    int[] example_layout_large_brushstroke = new int[]
    {4,2,2,1,0,0,1,0,0,0,0,0,
    4,4,2,2,1,0,1,2,1,0,0,0,
    4,4,2,1,0,0,0,1,2,1,0,0,
    4,4,2,1,0,0,0,1,2,1,2,2,
    4,4,2,2,1,0,0,0,0,1,2,2,
    4,2,1,2,0,0,0,0,0,0,1,2,
    4,2,0,0,0,0,0,1,1,2,2,4,
    4,2,2,1,0,0,1,1,1,2,2,4,
    4,2,2,1,0,0,0,0,0,1,1,2,
    4,2,2,2,1,0,0,0,0,0,0,1,
    4,2,2,1,1,0,1,1,0,1,1,2,
    4,2,2,2,2,2,2,2,2,2,2,2 };

    int[] example_layout_l = new int[]
    {0,0,0,0,0,
    0,0,3,0,0,
    0,2,4,3,0,
    0,0,0,0,0,
    0,0,0,0,0,};

    int[] example_layout_s = new int[]
    {0,0,0,0,0,
    0,0,2,0,0,
    0,2,3,2,0,
    0,0,0,0,0,
    0,0,0,0,0,};

    int[] example_layout_r = new int[]
    {1,1,1,1,1,
    1,1,0,1,1,
    1,0,0,0,1,
    1,1,1,1,1,
    1,1,1,1,1,};



    int[] example_layout_small_brushstroke = new int[]
    {1,1,1,1,1,0,1,0,0,0,0,0,
    1,1,1,1,0,0,0,1,1,0,0,0,
    1,1,1,1,0,0,0,1,1,1,0,0,
    1,1,1,1,0,0,0,1,1,1,1,1,
    1,1,1,1,0,1,0,0,0,1,1,1,
    1,1,1,0,0,0,0,0,0,0,1,1,
    1,1,0,0,0,0,0,1,1,1,1,1,
    1,1,1,0,0,0,1,1,1,1,1,1,
    1,1,1,1,0,0,0,0,0,1,1,1,
    1,1,1,1,1,0,0,0,0,0,0,1,
    1,1,1,1,0,0,1,1,0,1,1,1,
    1,1,1,1,1,1,1,1,1,1,1,1 };

    int[] example_layout_remove_brushstroke = new int[]
    {0,0,0,0,0,1,0,1,1,1,1,1,
    0,0,0,0,0,1,0,0,0,1,1,1,
    0,0,0,0,0,0,0,0,0,0,1,1,
    0,0,0,0,1,1,1,0,0,0,0,0,
    0,0,0,0,0,0,1,1,1,0,0,0,
    0,0,0,0,1,1,1,1,1,1,0,0,
    0,0,0,1,1,1,1,0,0,0,0,0,
    0,0,0,0,1,1,0,0,0,0,0,0,
    0,0,0,0,1,1,1,1,1,0,0,0,
    0,0,0,0,0,1,1,1,1,1,1,0,
    0,0,0,0,1,1,0,0,1,0,0,0,
    0,0,0,0,0,0,0,0,0,0,0,0 };

    public Army?[,] getArmies()
    {
        return units;
    }

    public Army? getArmy(int i, int j)
    {
        return units[i, j];
    }

    public void removeArmy(int i, int j, Army a)
    {
        GameObject army_obj = GameObject.Find(a.name);
        units[i, j] = null;
        Destroy(army_obj);
    }

    public void setArmy(int i, int j, Army a)
    {
        units[i, j] = a;
        SpawnArmies(i, j);
    }

    // Start is called before the first frame update
    void Start()


    {
        Console.WriteLine("Begin");

        world = new int[worldx, worldy];
        units = new Army?[worldx, worldy];

        Unit[] test_unit = new Unit[10];
        test_unit[0] = new Unit(10,10,2,1,"Warrior");
        units[0, 0] = new Army("Test Army", test_unit);

        int size = (UnityEngine.Random.Range(0,4) + 11);
        int[,] lar = new int[size,size];
        int[,] sma = new int[size,size];
        int[,] rems = new int[size,size];
        int conts = (UnityEngine.Random.Range(0,2) + 4);
        int contThickness = (UnityEngine.Random.Range(0,3) + 1);
        int islands = (UnityEngine.Random.Range(0,2) + 2);
        generateMapLayout(lar, sma, rems, conts, islands, contThickness);
        int[] rem = flattenArr(rems);
        int[] smalls = flattenArr(sma);
        int[] larges = flattenArr(lar);
        worldx = 100;
        worldy = 160;
        fill(world, 0);
        //printWorld("Blank World");

        //This function makes a map from randomly generated template layout
        // world = makeLandmapFromTemplate(world, larges, smalls, rem);

        //This function makes a map from premade Mediterranean layout
        world = makeLandmapFromTemplate(world, example_layout_l, example_layout_s, example_layout_r);

        smooth(4, 6,world);
       // printWorld("Continent Generation (Step 5)");
        removeCorners(world);
        //printWorld("Continent Generation (Step 6)");
        generateMountainRanges(world, (UnityEngine.Random.Range(0,8) + 13));
        placeMiniMountains(world, (UnityEngine.Random.Range(0,4) + 5));
        //printWorld("Heightmap Generation (Step 1)");
        fillHeightmap(world);
        //  printWorld("Heightmap Generation (Step 2)");
        generateHills(world, 80);
        //printWorld("Heightmap Generation (Step 3)");
        int[,] coastMap = createCoastMap(world);
        // printHeightmap(landmap, "Heightmap");
        // printHeightmap(coastMap, "Coast Map");
        int[,] rainShadow = generateRainShadowMap(world);
        // printHeightmap(rainShadow, "Rain Shadow");
        String[,] biomeMap = createBiomeMap(coastMap, world, rainShadow);
        // printBiomeMap(biomeMap,"Biome Generation (Step 1)");
       // int[,] contMap = generateContinentMap(world);
      //  printHeightmap(contMap, "Continent Map");
        printHeightmap(lar, "Larges");
        printHeightmap(sma, "Smalls");
        printHeightmap(rems, "Removes");
        int[,] rivers = generateRiverMap(world, coastMap, (UnityEngine.Random.Range(0,7) + 15));
        fixRivers(rivers);
        // printRivermap(rivers,landmap,"Rivers");
        printBiomeMapWithRivers(biomeMap, rivers, "Biomes");

        int[,] coastsRend = makeCoastSpriteMap(coastMap);




        Vertical = (int)Camera.main.orthographicSize;
        Horizontal = Vertical * (Screen.width / Screen.height);
        Columns = Horizontal * 2;
        Rows = Vertical * 2;

        //Grid = new float[Columns, Rows];

        //for (int i = 0; i < Columns; i++)
        //{
        //    for (int j = 0; j < Rows; j++)
        //    {
        //        Grid[i, j] = UnityEngine.Random.Range(0.0f, 1.0f);
        //        SpawnTile(i, j, Grid[i, j]);
        //    }
        //}

        for(int i = 0; i < world.GetLength(0); i++)
        {
            for(int j = 0; j < world.GetLength(1); j++)
            {
                SpawnTile(i, j, world[i, j], biomeMap[i, j]);
                SpawnMountain(i, j, world[i, j], biomeMap[i, j]);
                SpawnCoasts(i, j, coastsRend[i,j]);
                SpawnArmies(i, j);


            }
        }








    }

    private void SpawnArmies(int i, int j)
    {

        if (!(units[i,j] is null))
        {
            Army a = (Army) units[i, j];

            GameObject g = new GameObject(a.name);
            g.transform.position = new Vector3(j - (Horizontal - 0.5f), i - (Vertical - 0.5f));
            var s = g.AddComponent<SpriteRenderer>();
            s.sprite = unit_sprites[0];
            s.sortingLayerName = "Units";
            Debug.Log("added sprite");
        }

    }

    private void SpawnCoasts(int i, int j, int coast)
    {
        if(coast > 0)
        {
            GameObject g = new GameObject("Coast_X: " + j + "Y: " + i);
            g.transform.position = new Vector3(j - (Horizontal - 0.5f), i - (Vertical - 0.5f));
            var s = g.AddComponent<SpriteRenderer>();
            s.sortingOrder = 1;
            s.sprite = coasts[coast];
        }
    }

    private void SpawnMountain(int i, int j, int height, String biome)
    {

        if(height >= 6)
        {
            GameObject g = new GameObject("Feature_X: " + j + "Y: " + i);
            g.transform.position = new Vector3(j - (Horizontal - 0.5f), i - (Vertical - 0.5f));
            var s = g.AddComponent<SpriteRenderer>();
            s.sortingOrder = 1;
            s.sprite = littleMountain;
            if(height >= 7)
            {
                s.sprite = mountain;
            }
            if(height >= 8)
            {
                s.sprite = bigMountain;
            }
        }

       
    }

    private void SpawnTile(int i, int j, int height, String biome)
    {
        GameObject g = new GameObject("X: " + j + "Y: " + i);
        g.transform.position = new Vector3(j - (Horizontal - 0.5f), i - (Vertical - 0.5f));
        var boxCollider1 = g.AddComponent<BoxCollider>();
        var s = g.AddComponent<SpriteRenderer>();
        s.sortingOrder = 0;
        if (biome == "~")
        {
            s.sprite = ocean;
        }
        else if (biome == "T")
        {
            s.sprite = tropical;
        }
        else if (biome == "C")
        {
            if (height > 3)
            {
                s.sprite = grass_hills;
            }
            else s.sprite = grass;
        }
        else if (biome == "D")
        {
            if (height > 3)
            {
                s.sprite = desert_hills;
            }
            else s.sprite = desert;
        }
        else if (biome == "B")
        {
            s.sprite = boreal;
        }
        else if (biome == "U")
        {
            s.sprite = tundra;
        }
        else if (biome == "P")
        {
            s.sprite = polar;
        }else if(biome == "S")
        {
            s.sprite = steppe;
        }else if(biome == "M")
        {
            s.sprite = mediterranean;
        }else if(biome == "A")
        {
            s.sprite = arid;
        }else if(biome == "F")
        {
            s.sprite = forest;
        }
        else
        {
            s.sprite = grass;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public static int[,] makeCoastSpriteMap(int[,] coast)
    {
        int[,] ret = new int[coast.GetLength(0), coast.GetLength(1)];

        for(int i = 1; i < coast.GetLength(0)-1; i++)
        {
            for(int j = 1; j < coast.GetLength(1)-1; j++)
            {
                if(coast[i,j] > 0)
                {
                    ret[i, j] = 0;
                }
                else
                {
                    if(coast[i+1,j] > 0)
                    {
                        ret[i, j] = 1;
                        if(coast[i,j+1] > 0)
                        {
                            ret[i, j] = 2;
                            if(coast[i,j-1] > 0)
                            {
                                ret[i, j] = 13;
                                if(coast[i-1,j] > 0)
                                {
                                    ret[i, j] = 21;
                                }
                            }
                            if (coast[i - 1, j] > 0)
                            {
                                ret[i, j] = 14;
                            }
                        }
                        if (coast[i, j - 1] > 0)
                        {
                            ret[i, j] = 8;
                            if (coast[i, j + 1] > 0)
                            {
                                ret[i, j] = 13;
                            }
                            if(coast[i-1,j] > 0)
                            {
                                ret[i, j] = 16;
                            }
                        }
                    }

                    else if (coast[i - 1, j] > 0)
                    {
                        ret[i, j] = 5;
                        if (coast[i, j - 1] > 0)
                        {
                            ret[i, j] = 6;
                        }
                        if (coast[i, j + 1] > 0)
                        {
                            ret[i, j] = 4;
                            if (coast[i, j - 1] > 0)
                            {
                                ret[i, j] = 15;
                            }
                        }
                    }

                    else if (coast[i, j+1] > 0)
                    {
                        ret[i, j] = 3;
                        if (coast[i+1,j] > 0)
                        {
                            ret[i, j] = 4;
                            if (coast[i-1, j] > 0)
                            {
                                ret[i, j] = 14;
                            }
                        }
                        if(coast[i-1,j] > 0)
                        {
                            ret[i, j] = 6;
                        }
                    }

                    else if (coast[i, j - 1] > 0)
                    {
                        ret[i, j] = 7;
                        if (coast[i + 1, j] > 0)
                        {
                            ret[i, j] = 6;
                            if (coast[i - 1, j] > 0)
                            {
                                ret[i, j] = 16;
                            }
                        }
                    }

                    else if(coast[i+1,j+1] > 0)
                    {
                        ret[i, j] = 9;
                    }
                    else if(coast[i+1,j-1] > 0)
                    {
                        ret[i, j] = 12;
                    }
                    else if(coast[i-1,j+1] > 0)
                    {
                        ret[i, j] = 10;
                    }
                    else if(coast[i-1,j-1] > 0)
                    {
                        ret[i, j] = 11;
                    }
                }

            }
        }

        return ret;
    }

    public static void fixRivers(int[,] r)
    {
       
        for (int i = 1; (i
                    < (r.GetLength(0) - 1)); i++)
        {
            for (int j = 1; (j
                        < (r.GetLength(1) - 1)); j++)
            {
                if (((r[i,j] != 0)
                            && (r[i,j] != -1)))
                {
                    int amt = sumUnsafeAdjacent(i, j, r);
                    if ((amt
                                > (r[i,j] * 6)))
                    {
                        r[i,j] = 0;
                    }
                    else if (((amt
                                > (r[i,j] * 5))
                                && (UnityEngine.Random.Range(0,2) == 0)))
                    {
                        // r[i,j] = 0;
                    }
                    else if ((amt
                                == (r[i,j] * 2)))
                    {
                        r[i,j] = -1;
                    }

                }

            }

        }

    }

    public static int[,] generateRiverMap(int[,] h, int[,] c, int amt)
    {
        int[,] ret = new int [h.GetLength(0),h.GetLength(1)];
        int count = 0;
        fill(ret, 0);
        for (int x = 1; (x
                    < (h.GetLength(0) - 1)); x++)
        {
            for (int y = 1; (y
                        < (h.GetLength(1) - 1)); y++)
            {
                if (((h[x,y] >= 7)
                            && (UnityEngine.Random.Range(0,20) == 0)))
                {
                    if ((count < amt))
                    {
                        count++;
                        createRiver(ret, h, c, x, y, count);
                    }

                }

            }

        }

        return ret;
    }

    private static void createRiver(int[,] rivers, int[,] h, int[,] c, int x, int y, int num)
    {
        flow(rivers, h, c, x, y, "none", num, "none");
    }

    private static void flow(int[,] rivers, int[,] h, int[,] c, int x, int y, String last, int num, String original)
    {
        rivers[x,y] = num;
        if (((x == 0)
                    || ((x
                    >= (h.GetLength(0) - 1))
                    || ((y == 0)
                    || (y
                    >= (h.GetLength(1) - 1))))))
        {
            return;
        }

        int h_cur = h[x,y];
        if ((h_cur == 0))
        {
            return;
        }

        int h_north = h[(x + 1),y];
        int h_south = h[(x - 1),y];
        int h_east = h[x,(y + 1)];
        int h_west = h[x,(y - 1)];
        int r_north = rivers[(x + 1),y];
        int r_south = rivers[(x - 1),y];
        int r_east = rivers[x,(y + 1)];
        int r_west = rivers[x,(y - 1)];
        int c_north = c[(x + 1),y];
        int c_south = c[(x - 1),y];
        int c_east = c[x,(y + 1)];
        int c_west = c[x,(y - 1)];
        List<String> directions = new List<String>();
        if (((original == "none")
                    && (last != "none")))
        {
            original = last;
        }

        if (((h_north
                    <= (h_cur + 1))
                    && ((last != "south")
                    && (r_north != num))))
        {
            directions.Add("north");
            if ((h_north < h_cur))
            {
                for (int i = 0; (i < 5); i++)
                {
                    directions.Add("north");
                }

            }

            if ((c_north < 5))
            {
                for (int i = 0; (i < (6 * (5 - c_north))); i++)
                {
                    directions.Add("north");
                }

            }

            if ((h_north == 0))
            {
                for (int i = 0; (i < 15); i++)
                {
                    directions.Add("north");
                }

            }

        }

        if (((h_south
                    <= (h_cur + 1))
                    && ((last != "north")
                    && (r_south != num))))
        {
            directions.Add("south");
            if ((h_south < h_cur))
            {
                for (int i = 0; (i < 5); i++)
                {
                    directions.Add("south");
                }

            }

            if ((c_south < 5))
            {
                for (int i = 0; (i < (6 * (5 - c_south))); i++)
                {
                    directions.Add("south");
                }

            }

            if ((h_south == 0))
            {
                for (int i = 0; (i < 15); i++)
                {
                    directions.Add("south");
                }

            }

        }

        if (((h_east
                    <= (h_cur + 1))
                    && ((last != "west")
                    && (r_east != num))))
        {
            directions.Add("east");
            if ((h_east < h_cur))
            {
                for (int i = 0; (i < 10); i++)
                {
                    directions.Add("east");
                }

            }

            if ((h_east == 0))
            {
                for (int i = 0; (i < 15); i++)
                {
                    directions.Add("east");
                }

            }

            if ((c_east < 5))
            {
                for (int i = 0; (i < (6 * (5 - c_east))); i++)
                {
                    directions.Add("east");
                }

            }

        }

        if (((h_west
                    <= (h_cur + 1))
                    && ((last != "east")
                    && (r_west != num))))
        {
            if ((h_west < h_cur))
            {
                for (int i = 0; (i < 10); i++)
                {
                    directions.Add("west");
                }

            }

            directions.Add("west");
            if ((h_west == 0))
            {
                for (int i = 0; (i < 15); i++)
                {
                    directions.Add("west");
                }

            }

            if ((c_west < 5))
            {
                for (int i = 0; (i < (6 * (5 - c_west))); i++)
                {
                    directions.Add("west");
                }

            }

        }

        if ((!last.Equals("none")
                    && directions.Contains(last)))
        {
            for (int i = 0; (i < 15); i++)
            {
                directions.Add(last);
            }

        }

        if ((!original.Equals("none")
                    && directions.Contains(original)))
        {
            for (int i = 0; (i < 10); i++)
            {
                directions.Add(original);
            }

        }

        if ((directions.Count == 0))
        {
            rivers[x,y] = -1;
            return;
        }

        int select = UnityEngine.Random.Range(0,directions.Count);
        String direction = directions.ElementAt(select);
        if ((direction == "north"))
        {
            if ((rivers[(x + 1),y] == 0))
            {
                flow(rivers, h, c, (x + 1), y, direction, num, original);
            }
            else
            {
                rivers[(x + 1),y] = -1;
            }

            return;
        }

        if ((direction == "south"))
        {
            if ((rivers[(x - 1),y] == 0))
            {
                flow(rivers, h, c, (x - 1), y, direction, num, original);
            }
            else
            {
                rivers[(x - 1),y] = -1;
            }

        }

        if ((direction == "east"))
        {
            if ((rivers[x,(y + 1)] == 0))
            {
                flow(rivers, h, c, x, (y + 1), direction, num, original);
            }
            else
            {
                rivers[x,(y + 1)] = -1;
            }

            return;
        }

        if ((direction == "west"))
        {
            if ((rivers[x,(y - 1)] == 0))
            {
                flow(rivers, h, c, x, (y - 1), direction, num, original);
            }
            else
            {
                rivers[x,(y - 1)] = -1;
            }

            return;
        }

    }

    public static void generateMapLayout(int[,] l, int[,] s, int[,] r, int continents, int islands, int thickness)
    {
       
        fill(l, 0);
        fill(s, 0);
        fill(r, 0);
        for (int i = 0; (i < continents); i++)
        {
            for (int att = 0; (att < 10); att++)
            {
                int x = UnityEngine.Random.Range(0,l.GetLength(0));
                int y = UnityEngine.Random.Range(0,l.GetLength(1));
                if ((l[x,y] == 0))
                {
                    create(x, y, (UnityEngine.Random.Range(0,3) + 1), (UnityEngine.Random.Range(0,3) + 3), l);
                    for (int j = 0; (j < thickness); j++)
                    {
                        int modx = UnityEngine.Random.Range(0,2);
                        int mody = UnityEngine.Random.Range(0,2);
                        int dir1 = UnityEngine.Random.Range(0,1);
                        int dir2 = UnityEngine.Random.Range(0,1);
                        if ((dir1 == 0))
                        {
                            dir1 = -1;
                        }

                        if ((dir2 == 0))
                        {
                            dir2 = -1;
                        }

                        x = (x
                                    + (modx * dir1));
                        y = (y
                                    + (mody * dir2));
                        if (((x >= 0)
                                    && ((x < l.GetLength(0))
                                    && ((y >= 0)
                                    && (y < l.GetLength(1))))))
                        {
                            create(x, y, (UnityEngine.Random.Range(0,2) + 1), (UnityEngine.Random.Range(0,3) + 3), l);
                        }

                    }

                    break;
                }

            }

        }

        for (int i = 0; (i < islands); i++)
        {
            for (int att = 0; (att < 20); att++)
            {
                int x = UnityEngine.Random.Range(0,l.GetLength(0));
                int y = UnityEngine.Random.Range(0,l.GetLength(1));
                if (((x < 1)
                            || (x
                            >= (l.GetLength(0) - 1))))
                {
                    continue; // TODO: Warning!!! continue If
                }

                if (((y < 1)
                            || (y
                            >= (l.GetLength(1) - 1))))
                {
                    continue; // TODO: Warning!!! continue If
                }

                if (((l[x,y] == 0)
                            && ((l[(x + 1),y] == 0)
                            && ((l[x,(y + 1)] == 0)
                            && ((l[(x - 1),y] == 0)
                            && (l[x,(y - 1)] == 0))))))
                {
                    create(x, y, (UnityEngine.Random.Range(0,1) + 1), (UnityEngine.Random.Range(0,4) + 3), s);
                    break;
                }

            }

        }

        for (int i = 0; (i < l.GetLength(0)); i++)
        {
            for (int j = 0; (j < l.GetLength(1)); j++)
            {
                if ((l[i,j] == 0))
                {
                    if ((s[i,j] == 0))
                    {
                        r[i,j] = UnityEngine.Random.Range(0,5);
                    }
                    else
                    {
                        r[i,j] = UnityEngine.Random.Range(0,3);
                    }

                }

            }

        }

        erode(l, 6);
        erode(s, 4);
    }

    public static int[] flattenArr(int[,] arr)
    {
        int[] ret = new int[(arr.GetLength(0) * arr.GetLength(0))];
        for (int i = 0; (i < arr.GetLength(0)); i++)
        {
            for (int j = 0; (j < arr.GetLength(0)); j++)
            {
                ret[(i
                            + (j * arr.GetLength(0)))] = arr[i,j];
            }

        }

        return ret;
    }

    //public static void printContinentStatistics(int[,] con)
    //{
    //    Map<Integer, Integer> conts = new HashMap<Integer, Integer>();
    //    int mapSize = (con.GetLength(0) * con[0].GetLength(0));
    //    for (int i = 0; (i < con.GetLength(0)); i++)
    //    {
    //        for (int j = 0; (j < con.GetLength(1)); j++)
    //        {
    //            if (!conts.containsKey(con[i,j]))
    //            {
    //                conts.put(con[i,j], 1);
    //            }
    //            else
    //            {
    //                conts.put(con[i,j], (conts.get(con[i,j]) + 1));
    //            }

    //        }

    //    }

    //    Console.WriteLine("=== Continent Statistics ===");
    //    DecimalFormat df = new DecimalFormat("###.##");
    //    foreach (Integer i in conts.keySet())
    //    {
    //        if ((i == 0))
    //        {
    //            Console.WriteLine(("Ocean: "
    //                            + (conts.get(i) + (" tiles. Map percent: " + df.format((100
    //                                * (conts.get(i) / mapSize)))))));
    //        }
    //        else
    //        {
    //            Console.WriteLine(("Continent "
    //                            + (i + (": "
    //                            + (conts.get(i) + (" tiles. Map percent: " + df.format((100
    //                                * (conts.get(i) / mapSize)))))))));
    //        }

    //    }

    //}

    //public static void printBiomeStatistics(String[,] bio)
    //{
    //    Map<String, Integer> conts = new HashMap<String, Integer>();
    //    int mapSize = (bio.GetLength(0) * bio[0].GetLength(0));
    //    for (int i = 0; (i < bio.GetLength(0)); i++)
    //    {
    //        for (int j = 0; (j < bio.GetLength(1)); j++)
    //        {
    //            if (!conts.containsKey(bio[i,j]))
    //            {
    //                conts.put(bio[i,j], 1);
    //            }
    //            else
    //            {
    //                conts.put(bio[i,j], (conts.get(bio[i,j]) + 1));
    //            }

    //        }

    //    }

    //    Console.WriteLine("=== Continent Statistics ===");
    //    DecimalFormat df = new DecimalFormat("###.##");
    //    foreach (String i in conts.keySet())
    //    {
    //        Console.WriteLine(("Biome "
    //                        + (i + (": "
    //                        + (conts.get(i) + (" tiles. Map percent: " + df.format((100
    //                            * (conts.get(i) / mapSize)))))))));
    //    }

    //}

    // Takes in a blank map and 3 templates. Templates must be perfect square length. Divides the map into an X by X grid of map regions. Creates features according to the value of each template array at X by X. Templates are 1D arrays that are unwrapped.
    public static int[,] makeLandmapFromTemplate(int[,] w, int[] largeTemplate, int[] smallTemplate, int[] remTemplate)
    {
        fill(w, 0);
        if (((largeTemplate.GetLength(0) != smallTemplate.GetLength(0))
                    || ((smallTemplate.GetLength(0) != largeTemplate.GetLength(0))
                    || (largeTemplate.GetLength(0) != remTemplate.GetLength(0)))))
        {
            Console.WriteLine("ERROR! Templates must have the same length!");
        }

        if (((Math.Sqrt(largeTemplate.GetLength(0)) % 1)
                    != 0))
        {
            Console.WriteLine("ERROR! Templates must be a perfect square number!");
        }

        int size = ((int)(Math.Sqrt(largeTemplate.GetLength(0))));
        int worldXSize = w.GetLength(0);
        int worldYSize = w.GetLength(1);
       
        // apply large brush strokes
        for (int i = 0; (i < size); i++)
        {
            for (int j = 0; (j < size); j++)
            {
                for (int k = 0; (k < largeTemplate[(i
                            + (j * size))]); k++)
                {
                    int x = (UnityEngine.Random.Range(0,(worldXSize / size))
                                + (i
                                * (worldXSize / size)));
                    int y = (UnityEngine.Random.Range(0,(worldYSize / size))
                                + (j
                                * (worldYSize / size)));
                    int s = (UnityEngine.Random.Range(0,7) + 6);
                    create(x, y, s, 1, w);
                    // w[x,y] = 1;
                }

            }

        }

        // apply small brush strokes
        for (int i = 0; (i < size); i++)
        {
            for (int j = 0; (j < size); j++)
            {
                for (int k = 0; (k < smallTemplate[(i
                            + (j * size))]); k++)
                {
                    int x = (UnityEngine.Random.Range(0,(worldXSize / size))
                                + (i
                                * (worldXSize / size)));
                    int y = (UnityEngine.Random.Range(0,(worldYSize / size))
                                + (j
                                * (worldYSize / size)));
                    int s = (UnityEngine.Random.Range(0,4) + 2);
                    create(x, y, s, 1, w);
                    //  w[x,y] = 2;
                }

            }

        }

        // apply remove strokes / eraser tool
        for (int i = 0; (i < size); i++)
        {
            for (int j = 0; (j < size); j++)
            {
                for (int k = 0; (k < remTemplate[(i
                            + (j * size))]); k++)
                {
                    int x = (UnityEngine.Random.Range(0,(worldXSize / size))
                                + (i
                                * (worldXSize / size)));
                    int y = (UnityEngine.Random.Range(0,(worldYSize / size))
                                + (j
                                * (worldYSize / size)));
                    int s = (UnityEngine.Random.Range(0,4) + 2);
                    create(x, y, s, 0, w);
                    // w[x,y] = 3;
                }

            }

        }

        return w;
    }

    // Recursively Performs floodFill to fill in a continent, starting from i,j.
    public static void continentFloodFill(int[,] from, int[,] to, int i, int j, int fill)
    {
        if (((i < 0)
                    || ((j < 0)
                    || ((i >= from.GetLength(0))
                    || (j >= from.GetLength(1))))))
        {
            return;
        }

        if (((from[i,j] != 0)
                    && (to[i,j] != fill)))
        {
            to[i,j] = fill;
            continentFloodFill(from, to, (i + 1), j, fill);
            continentFloodFill(from, to, (i - 1), j, fill);
            continentFloodFill(from, to, i, (j + 1), fill);
            continentFloodFill(from, to, i, (j - 1), fill);
            return;
        }
        else
        {
            return;
        }

    }

    // Generates a rain shadow map from a heightmap
    public static int[,] generateRainShadowMap(int[,] h)
    {
        int[,] ret = new int[h.GetLength(0), h.GetLength(1)];
        ret = fill(ret, 0);
        int shadow = 0;
        for (int i = 0; (i < h.GetLength(0)); i++)
        {
            for (int j = 0; (j < h.GetLength(1)); j++)
            {
                if ((h[i,j] >= 7))
                {
                    shadow = h[i,j];
                }

                int lat = getLat(h, i);
                if (((lat >= 60)
                            || (lat <= 30)))
                {
                    // go backwards
                    for (int k = j; (k > Math.Max((j - 7), 0)); k--)
                    {
                        if ((shadow <= 0))
                        {
                            break;
                        }

                        if (((k < 0)
                                    || (k >= h.GetLength(1))))
                        {
                            shadow = 0;
                            break;
                        }
                        else
                        {
                            ret[i,k] = Math.Max(ret[i,k], shadow);
                            shadow--;
                        }

                    }

                }
                else
                {
                    for (int k = j; (k < Math.Max((j + 7), h.GetLength(1))); k++)
                    {
                        if ((shadow <= 0))
                        {
                            break;
                        }

                        if (((k < 0)
                                    || (k >= h.GetLength(1))))
                        {
                            shadow = 0;
                            break;
                        }
                        else
                        {
                            ret[i,k] = Math.Max(ret[i,k], shadow);
                            shadow--;
                        }

                    }

                }

            }

        }

        return ret;
    }

    // Generates a continent map using FloodFill. This implementation uses up a lot of memory.
    public static int[,] generateContinentMap(int[,] h)
    {
        int[,] ret = new int[h.GetLength(0), h.GetLength(1)];
        ret = fill(ret, 0);
        int n = 1;
        for (int i = 0; (i < h.GetLength(0)); i++)
        {
            for (int j = 0; (j < h.GetLength(1)); j++)
            {
                if (((h[i,j] != 0)
                            && (ret[i,j] == 0)))
                {
                    continentFloodFill(h, ret, i, j, n);
                    n++;
                }

            }

        }

        // continentFloodFill(h,ret,20,20,n);
        return ret;
    }

    // Creates a coastal map. That is, a heightmap where high values indicate an area is far from the coast.
    public static int[,] createCoastMap(int[,] h)
    {
        int[,] ret = deepCopy(h);
        for (int i = 0; (i < h.GetLength(0)); i++)
        {
            for (int j = 0; (j < h.GetLength(1)); j++)
            {
                if ((ret[i,j] != 0))
                {
                    ret[i,j] = 5;
                }

            }

        }

        for (int i = 0; (i < h.GetLength(0)); i++)
        {
            for (int j = 0; (j < h.GetLength(1)); j++)
            {
                if ((h[i,j] == 0))
                {
                    ret[i,j] = 0;
                    int val = 5;
                    for (int k = Math.Max((i - val), 0); (k < Math.Min((i + val), h.GetLength(0))); k++)
                    {
                        for (int l = Math.Max((j - val), 0); (l < Math.Min((j + val), h.GetLength(1))); l++)
                        {
                            if ((ret[k,l] > dist(i, j, k, l)))
                            {
                                ret[k,l] = dist(i, j, k, l);
                            }

                        }

                    }

                }

            }

        }

        return ret;
    }

    // Creates a biome map using the getBiome method. Inputs are a coastal map, a heightmap and a rainshadow map.
    public static String[,] createBiomeMap(int[,] w, int[,] height, int[,] rainShadow)
    {
        String[,] ret = new String[w.GetLength(0), w.GetLength(1)];
        for (int i = 0; (i < w.GetLength(0)); i++)
        {
            for (int j = 0; (j < w.GetLength(1)); j++)
            {
                ret[i,j] = getBiome(w, height, rainShadow, i, j);
            }

        }

        return ret;
    }

    public static String getBiome(int[,] w, int[,] heightmap, int[,] rainShadow, int i, int j)
    {
        int lat = Math.Abs(getLat(w, i));
        bool west = (j
                    < (w.GetLength(1) / 2));
        bool middle = ((j >= (2
                    * (w.GetLength(1) / 5)))
                    && (j <= (3
                    * (w.GetLength(1) / 5))));
        bool midEdge = (((j >= (1.7
                    * (w.GetLength(1) / 5)))
                    && (j <= (2
                    * (w.GetLength(1) / 5))))
                    || ((j >= (2.7
                    * (w.GetLength(1) / 5)))
                    && (j <= (3
                    * (w.GetLength(1) / 5)))));
        int height = heightmap[i,j];
        int coastal = w[i,j];
        int rainBlocked = rainShadow[i,j];
        // height factor
        lat = (lat + (2 * height));
        if ((height == 0))
        {
            return "~";
        }

        if ((midEdge
                    && (coastal >= 3)))
        {
            if ((UnityEngine.Random.Range(0,((int)((Math.Abs((j
                                - (w.GetLength(1) / 2))) / 3)))) == 0))
            {
                middle = false;
            }

        }

        if (((rainBlocked == 1)
                    && (UnityEngine.Random.Range(0,2) == 0)))
        {
            rainBlocked = 3;
        }

        if (((rainBlocked == 2)
                    && (UnityEngine.Random.Range(0,1) == 0)))
        {
            rainBlocked = 3;
        }

        // rain shadow
        if ((rainBlocked >= 3))
        {
            if (((lat >= 60)
                        && (lat <= 80)))
            {
                return "U";
                // tundra
            }

            if (((lat >= 55)
                        && (lat <= 60)))
            {
                return "F";
                // temperate forest
            }

            if (((lat >= 40)
                        && (lat <= 55)))
            {
                return "A";
                // arid
            }

            if (((lat >= 20)
                        && (lat <= 40)))
            {
                return "D";
                // desert
            }

            if ((lat <= 20))
            {
                return "C";
                // subtropical forest
            }

        }

        // transitional
        if ((middle
                    && (coastal >= 3)))
        {
            if ((lat >= 80))
            {
                return "P";
                // polar
            }

            if (((lat >= 70)
                        && (lat <= 80)))
            {
                return "U";
                // tundra
            }

            if (((lat >= 50)
                        && (lat <= 70)))
            {
                return "B";
                // boreal forest
            }

            if (((lat >= 30)
                        && (lat <= 50)))
            {
                return "F";
                // temperate forest
            }

            if (((lat >= 20)
                        && (lat <= 30)))
            {
                return "A";
                // arid
            }

            if (((lat >= 5)
                        && (lat <= 20)))
            {
                return "C";
                // subtropical forest
            }

            if ((lat <= 5))
            {
                return "T";
                // tropical forest
            }

        }

        // inland
        if ((coastal >= 5))
        {
            if ((lat >= 80))
            {
                return "P";
                // polar
            }

            if (((lat >= 70)
                        && (lat <= 80)))
            {
                return "U";
                // tundra
            }

            if (((lat >= 55)
                        && (lat <= 70)))
            {
                return "B";
                // boreal forest
            }

            if (((lat >= 50)
                        && (lat <= 55)))
            {
                return "F";
                // temperate forest
            }

            if (((lat >= 35)
                        && ((lat <= 50)
                        && (height <= 2))))
            {
                return "S";
                // steppe
            }

            if (((lat >= 35)
                        && ((lat <= 50)
                        && (height > 2))))
            {
                return "F";
                // forest
            }

            if (((lat >= 20)
                        && (lat <= 35)))
            {
                return "A";
                // arid
            }

            if (((lat >= 10)
                        && (lat <= 20)))
            {
                return "C";
                // subtropical forest
            }

            if ((lat <= 10))
            {
                return "T";
                // tropical forest
            }

        }

        if (((coastal <= 4)
                    && west))
        {
            // west coast
            if ((lat >= 80))
            {
                return "P";
                // polar
            }

            if (((lat >= 70)
                        && (lat <= 80)))
            {
                return "U";
                // tundra
            }

            if (((lat >= 60)
                        && (lat <= 70)))
            {
                return "B";
                // boreal forest
            }

            if (((lat >= 50)
                        && (lat <= 60)))
            {
                return "F";
                // temperate forest
            }

            if (((lat >= 35)
                        && (lat <= 50)))
            {
                return "M";
                // mediterranean
            }

            if (((lat >= 20)
                        && (lat <= 35)))
            {
                return "D";
                // desert
            }

            if (((lat >= 7)
                        && (lat <= 20)))
            {
                return "C";
                // subtropical forest
            }

            if ((lat <= 7))
            {
                return "T";
                // tropical forest
            }

        }

        if (((coastal <= 4)
                    && !west))
        {
            // east coast
            if ((lat >= 75))
            {
                return "P";
                // polar
            }

            if (((lat >= 65)
                        && (lat <= 75)))
            {
                return "U";
                // tundra
            }

            if (((lat >= 50)
                        && (lat <= 65)))
            {
                return "B";
                // boreal forest
            }

            if (((lat >= 25)
                        && (lat <= 50)))
            {
                return "C";
                // temperate forest
            }

            if ((lat <= 25))
            {
                return "T";
                // tropical
            }

        }

        return "?";
    }

    // Maps a value from two ranges. E.g. 5, 1-10, 100-1000 = 500.
    public static int map(int x, int in_min, int in_max, int out_min, int out_max)
    {
        return (((x - in_min)
                    * ((out_max - out_min)
                    / (in_max - in_min)))
                    + out_min);
    }

    // Gets the latitude of a position. Uses some preset values.
    public static int getLat(int[,] w, int i)
    {
        return (map(i, 0, (w.GetLength(0) - 1), 0, (70 + 50)) - 50);
    }

    // Translates a heightmap into human-readable text.
    public static String translate(int i)
    {
        if ((i == 0))
        {
            return "~";
        }

        if ((i == 1))
        {
            return "o";
        }

        if ((i >= 7))
        {
            return "X";
        }

        return "H";
    }

    // Generates random hills of size 5, 4, and 3.
    public static void generateHills(int[,] h, int n)
    {
     
        // Place 5 tile hills
        for (int amt = 0; (amt < n); amt++)
        {
            for (int attempts = 0; (attempts < 30); attempts++)
            {
                int x1 = UnityEngine.Random.Range(0,h.GetLength(0));
                int y1 = UnityEngine.Random.Range(0,h.GetLength(1));
                if ((h[x1,y1] != 1))
                {
                    // TODO: Warning!!! continue If
                }
                else
                {
                    h[x1,y1] = 10;
                    break;
                }

            }

        }

        // Place 4 tile hills
        for (int amt = 0; (amt < n); amt++)
        {
            for (int attempts = 0; (attempts < 30); attempts++)
            {
                int x1 = UnityEngine.Random.Range(0,h.GetLength(0));
                int y1 = UnityEngine.Random.Range(0,h.GetLength(1));
                if ((h[x1,y1] != 1))
                {
                    // TODO: Warning!!! continue If
                }
                else
                {
                    h[x1,y1] = 9;
                    break;
                }

            }

        }

        // Place 3 tile hills
        for (int amt = 0; (amt < n); amt++)
        {
            for (int attempts = 0; (attempts < 30); attempts++)
            {
                int x1 = UnityEngine.Random.Range(0,h.GetLength(0));
                int y1 = UnityEngine.Random.Range(0,h.GetLength(1));
                if ((h[x1,y1] != 1))
                {
                    // TODO: Warning!!! continue If
                }
                else
                {
                    h[x1,y1] = 11;
                    break;
                }

            }

        }

        int[,] copy = deepCopy(h);
        for (int i = 0; (i < h.GetLength(0)); i++)
        {
            for (int j = 0; (j < h.GetLength(1)); j++)
            {
                if ((copy[i,j] == 9))
                {
                    h[i,j] = 5;
                    raise(h, i, j);
                }

                if ((copy[i,j] == 10))
                {
                    h[i,j] = 4;
                    raise(h, i, j);
                }

                if ((copy[i,j] == 11))
                {
                    h[i,j] = 3;
                    raise(h, i, j);
                }

            }

        }

    }

    // Raises land around a hill/mountain.
    public static void raise(int[,] h, int i, int j)
    {
 
        int val = h[i,j];
        for (int k = Math.Max((i - val), 0); (k < Math.Min((i + val), h.GetLength(0))); k++)
        {
            for (int l = Math.Max((j - val), 0); (l < Math.Min((j + val), h.GetLength(1))); l++)
            {
                int dist = (manDist(i, j, k, l) + UnityEngine.Random.Range(0,1));
                if (((h[k,l] > 0)
                            && ((dist - val)
                            > h[k,l])))
                {
                    h[k,l] = Math.Min((val - 1), (dist - val));
                }

            }

        }

    }

    // Performs raise on every mountain to put in foothills.
    public static void fillHeightmap(int[,] h)
    {

        int[,] copy = deepCopy(h);
        for (int i = 0; (i < h.GetLength(0)); i++)
        {
            for (int j = 0; (j < h.GetLength(1)); j++)
            {
                if ((copy[i,j] == 7))
                {
                    for (int k = Math.Max((i - 7), 0); (k < Math.Min((i + 7), h.GetLength(0))); k++)
                    {
                        for (int l = Math.Max((j - 7), 0); (l < Math.Min((j + 7), h.GetLength(1))); l++)
                        {
                            int dist = manDist(i, j, k, l);
                            if (((dist < 7)
                                        && ((h[k,l] != 0)
                                        && (h[k,l] < (7 - dist)))))
                            {
                                h[k,l] = (7 - dist);
                            }

                        }

                    }

                }

            }

        }

    }

    // Generates a certain number of mountain ranges.
    public static void generateMountainRanges(int[,] h, int n)
    {
        for (int i = 0; (i < n); i++)
        {
            placeMountainRange(h);
        }

    }

    public static void placeMiniMountains(int[,] h, int n)
    {
        
        for (int k = 0; (k < n); k++)
        {
            for (int i = 0; (i < 40); i++)
            {
                int r1 = (UnityEngine.Random.Range(0,(h.GetLength(0) - 16)) + 16);
                int r2 = (UnityEngine.Random.Range(0,(h.GetLength(1) - 16)) + 16);
                if ((h[r1,r2] == 1))
                {
                    h[r1,r2] = 7;
                    if ((UnityEngine.Random.Range(0,1) > 0))
                    {
                        int dir1 = UnityEngine.Random.Range(0,1);
                        if ((dir1 == 0))
                        {
                            dir1 = -1;
                        }

                        int dir2 = UnityEngine.Random.Range(0,1);
                        if ((dir2 == 0))
                        {
                            dir2 = -1;
                        }

                        int mod1 = UnityEngine.Random.Range(0,1);
                        int mod2 = UnityEngine.Random.Range(0,1);
                        h[(r1
                                    + (mod1 * dir1)),(r2
                                    + (mod2 * dir2))] = 7;
                    }

                    if ((UnityEngine.Random.Range(0,2) == 0))
                    {
                        for (int j = 0; (j < 5); j++)
                        {
                            int dir1 = UnityEngine.Random.Range(0,1);
                            if ((dir1 == 0))
                            {
                                dir1 = -1;
                            }

                            int dir2 = UnityEngine.Random.Range(0,1);
                            if ((dir2 == 0))
                            {
                                dir2 = -1;
                            }

                            int mod1 = UnityEngine.Random.Range(0,1);
                            int mod2 = UnityEngine.Random.Range(0,1);
                            r1 = (r1
                                        + (mod1 * dir1));
                            r2 = (r2
                                        + (mod2 * dir2));
                            h[r1,r2] = 7;
                        }

                    }

                    break;
                }

            }

        }

    }

    // Generates a mountain range. It tries to place them on land and not on the world border.
    public static void placeMountainRange(int[,] h)
    {
        
        for (int i = 0; (i < 40); i++)
        {
            int r1 = (UnityEngine.Random.Range(0,(h.GetLength(0) - 16)) + 16);
            int r2 = (UnityEngine.Random.Range(0,(h.GetLength(1) - 16)) + 16);
            if ((h[r1,r2] == 1))
            {
                h[r1,r2] = 8;
                int size = (UnityEngine.Random.Range(0,6) + 5);
                int dir1 = UnityEngine.Random.Range(0,1);
                if ((dir1 == 0))
                {
                    dir1 = -1;
                }

                int dir2 = UnityEngine.Random.Range(0,1);
                if ((dir2 == 0))
                {
                    dir2 = -1;
                }

                int x1 = (UnityEngine.Random.Range(0,(size - 2)) + 1);
                int y1 = (UnityEngine.Random.Range(0,(size - x1)) + 1);
                int len1 = x1;
                int len2 = y1;
                x1 = (r1
                            + (x1 * dir1));
                y1 = (r2
                            + (y1 * dir2));
                for (int amt = 0; (amt < 30); amt++)
                {
                    if ((h[x1,y1] == 1))
                    {
                        break;
                    }

                    dir1 = UnityEngine.Random.Range(0,1);
                    if ((dir1 == 0))
                    {
                        dir1 = -1;
                    }

                    dir2 = UnityEngine.Random.Range(0,1);
                    if ((dir2 == 0))
                    {
                        dir2 = -1;
                    }

                    x1 = (UnityEngine.Random.Range(0,(size - 2)) + 1);
                    y1 = (UnityEngine.Random.Range(0,(size - x1)) + 1);
                }

                h[x1,y1] = 8;
                int thickness = (UnityEngine.Random.Range(0,3) + 4);
                size = (size - 1);
                for (int j = 0; (j
                            < (thickness * size)); j++)
                {
                    int a1 = UnityEngine.Random.Range(0,len1);
                    int b1 = UnityEngine.Random.Range(0,len2);
                    int lowx = Math.Min(r1, x1);
                    int lowy = Math.Min(r2, y1);
                    if ((h[(lowx + a1),(lowy + b1)] != 8))
                    {
                        h[(lowx + a1),(lowy + b1)] = 7;
                    }

                }

                for (int j = 0; (j
                            < ((thickness - 2)
                            * size)); j++)
                {
                    int a1 = UnityEngine.Random.Range(0,len1);
                    int b1 = UnityEngine.Random.Range(0,len2);
                    int lowx = Math.Min(r1, x1);
                    int lowy = Math.Min(r2, y1);
                    if ((((lowx - a1)
                                < 0)
                                || (((lowx - a1)
                                > h.GetLength(0))
                                || (((lowy - b1)
                                < 0)
                                || ((lowy - b1)
                                > h.GetLength(1))))))
                    {
                        continue; // TODO: Warning!!! continue If
                    }

                    if ((h[(lowx - a1),(lowy - b1)] != 8))
                    {
                        h[(lowx - a1),(lowy - b1)] = 7;
                    }

                }

                for (int j = 0; (j
                            < ((thickness - 2)
                            * size)); j++)
                {
                    int a1 = UnityEngine.Random.Range(0,len1);
                    int b1 = UnityEngine.Random.Range(0,len2);
                    int hix = Math.Max(r1, x1);
                    int hiy = Math.Max(r2, y1);
                    hix = hix + a1;
                    hiy = hiy + b1;
                    if ((hix < 0)
                                || ((hix
                                           >= h.GetLength(0)-1))
                                || ((hiy
                                           < 0))
                                || (hiy
                                         >= h.GetLength(1)-1))
                    {
                        continue; // TODO: Warning!!! continue If
                    }

                    if ((h[(hix),(hiy)] != 8))
                    {
                        h[(hix),(hiy)] = 7;
                    }

                }

                return;
            }

        }

    }

    // Removes odd configurations of tiles. E.G.
    //  ~F
    //  F~ 
    //       and
    // F~
    // ~F
    public static void removeCorners(int[,] w)
    {
        int[,] world = deepCopy(w);
        for (int i = 1; (i
                    < (world.GetLength(0) - 1)); i++)
        {
            for (int j = 1; (j
                        < (world.GetLength(1) - 1)); j++)
            {
                if (((world[i,j] == 1)
                            && ((world[i,(j + 1)] == 0)
                            && ((world[(i + 1),(j + 1)] == 1)
                            && (world[(i + 1),j] == 0)))))
                {
                    w[i,j] = 0;
                    w[(i + 1),(j + 1)] = 0;
                }
                else if (((world[i,j] == 0)
                            && ((world[i,(j + 1)] == 1)
                            && ((world[(i + 1),(j + 1)] == 0)
                            && (world[(i + 1),j] == 1)))))
                {
                    w[i,(j + 1)] = 0;
                    w[(i + 1),j] = 0;
                }

            }

        }

    }

    //// Weathers the map's coastlines a bit. 
    //public static void erode(int thresh)
    //{
    //    for (int i = 1; (i
    //                < (worldx - 1)); i++)
    //    {
    //        for (int j = 1; (j
    //                    < (worldy - 1)); j++)
    //        {
    //            if ((sumUnsafeAdjacent(i, j, world) <= thresh))
    //            {
    //                world[i,j] = 0;
    //            }

    //        }

    //    }

    //}

    // Weathers the map's coastlines a bit. 
    public static void erode(int[,] world, int thresh)
    {
        for (int i = 1; (i
                    < (world.GetLength(0) - 1)); i++)
        {
            for (int j = 1; (j
                        < (world.GetLength(1) - 1)); j++)
            {
                if ((sumUnsafeAdjacent(i, j, world) <= thresh))
                {
                    world[i,j] = 0;
                }

            }

        }

    }

    // Performs a deepCopy of an array.
    public static int[,] deepCopy(int[,] array)
    {

        int[,] copy = new int[array.GetLength(0), array.GetLength(1)];

        for(int i = 0; i < array.GetLength(0); i++)
        {
            for(int j = 0; j < array.GetLength(1); j++)
            {
                copy[i, j] = array[i, j];
            }
        }

        return copy;
    }

    public static int[] deepCopy(int[] array)
    {

        int[] copy = new int[array.GetLength(0)];

        for (int i = 0; i < array.GetLength(0); i++)
        {
                copy[i] = array[i];

        }

        return copy;
    }

    // "Smooths" the maps coastlines out a bit and makes them look naturally rough.
    public static void smooth(int tlow, int thigh, int[,] world)
    {
        int[,] copy = deepCopy(world);

        int worldx = world.GetLength(0);
        int worldy = world.GetLength(1);

        for (int i = 1; (i
                    < (worldx - 1)); i++)
        {
            for (int j = 1; (j
                        < (worldy - 1)); j++)
            {
                int temp = sumUnsafeAdjacent(i, j, copy);
                if ((temp <= tlow))
                {
                    if (((temp >= 2)
                                && (UnityEngine.Random.Range(0, 4) == 0)))
                    {
                        world[i, j] = 1;
                    }
                    else
                    {
                        world[i, j] = 0;
                    }

                }
                else if ((temp >= thigh))
                {
                    if ((UnityEngine.Random.Range(0, 2) == 0))
                    {
                        world[i, j] = 1;
                    }

                    if (((temp <= 7)
                                && (UnityEngine.Random.Range(0, 4) == 0)))
                    {
                        world[i, j] = 0;
                    }

                }

            }

        }

    }

    public static void smooth(int[,] world, int tlow, int thigh)
    {
        int[,] copy = deepCopy(world);
        
        for (int i = 1; (i
                    < (world.GetLength(0) - 1)); i++)
        {
            for (int j = 1; (j
                        < (world.GetLength(1) - 1)); j++)
            {
                int temp = sumUnsafeAdjacent(i, j, copy);
                if ((temp <= tlow))
                {
                    if (((temp >= 2)
                                && (UnityEngine.Random.Range(0,4) == 0)))
                    {
                        world[i,j] = 1;
                    }
                    else
                    {
                        world[i,j] = 0;
                    }

                }
                else if ((temp >= thigh))
                {
                    if ((UnityEngine.Random.Range(0,2) == 0))
                    {
                        world[i,j] = 1;
                    }

                    if (((temp <= 7)
                                && (UnityEngine.Random.Range(0,4) == 0)))
                    {
                        world[i,j] = 0;
                    }

                }

            }

        }

    }

    // Sums all the adjacent tiles. Does not care about map borders.
    public static int sumUnsafeAdjacent(int x, int y, int[,] world)
    {
        int ret = 0;
        ret = (ret + world[x,y]);
        ret = (ret + world[(x - 1),y]);
        ret = (ret + world[(x + 1),y]);
        ret = (ret + world[x,(y - 1)]);
        ret = (ret + world[(x - 1),(y - 1)]);
        ret = (ret + world[(x + 1),(y - 1)]);
        ret = (ret + world[x,(y + 1)]);
        ret = (ret + world[(x - 1),(y + 1)]);
        ret = (ret + world[(x + 1),(y + 1)]);
        return ret;
    }

    // Performs generateFeature(n) amt times.
    public static void generateFeatures(int n, int amt, int[,] world)
    {
        for (int i = 0; (i < amt); i++)
        {
            generateFeature(n,world);
        }

    }

    // Performs generateSmallFeatures(n) amt times.
    public static void generateSmallFeatures(int n, int amt, int[,] world)
    {
        for (int i = 0; (i < amt); i++)
        {
            generateSmallFeature(n,world);
        }

    }

    // Performs removeFeatures(n) amt times.
    public static void removeFeatures(int n, int amt, int[,]world)
    {
        for (int i = 0; (i < amt); i++)
        {
            removeFeature(n,world);
        }

    }

    // Removes land.
    public static void removeFeature(int n, int[,] world)
    {
        int worldx = world.GetLength(0);
        int worldy = world.GetLength(1);
        for (int i = 0; (i < 40); i++)
        {
            int r1 = UnityEngine.Random.Range(0,worldx);
            int r2 = UnityEngine.Random.Range(0,worldy);
            if ((world[r1,r2] == 0))
            {
                int size = (UnityEngine.Random.Range(0,5) + 2);
                create(r1, r2, size, n, world);
                return;
            }

        }

    }

    // Adds land
    public static void generateFeature(int n, int[,] world)
    {
        int worldx = world.GetLength(0);
        int worldy = world.GetLength(1);

        int r1 = UnityEngine.Random.Range(0,worldx);
        int r2 = UnityEngine.Random.Range(0,worldy);
        int size = (UnityEngine.Random.Range(0,15) + 5);
        create(r1, r2, size, n, world);
    }

    // Adds a small amount of land
    public static void generateSmallFeature(int n, int[,] world)
    {
        int worldx = world.GetLength(0);
        int worldy = world.GetLength(1);

        int r1 = UnityEngine.Random.Range(0,worldx);
        int r2 = UnityEngine.Random.Range(0,worldy);
        int size = (UnityEngine.Random.Range(0,4) + 4);
        create(r1, r2, size, n, world);
    }

    // Fills in tiles in a diamond shape.
    public static void create(int x, int y, int dist, int n, int[,] world)
    {
        for (int i = 0; (i < world.GetLength(0)); i++)
        {
            for (int j = 0; (j < world.GetLength(1)); j++)
            {
                if ((manDist(x, y, i, j) <= dist))
                {
                    world[i,j] = n;
                }

            }

        }

    }

    //// Fills in tiles in a diamond shape. Deprecated
    //public static void create(int x, int y, int dist, int n, int[,] world)
    //{
    //    for (int i = 0; (i < world.GetLength(0)); i++)
    //    {
    //        for (int j = 0; (j < world.GetLength(1)); j++)
    //        {
    //            if ((manDist(x, y, i, j) <= dist))
    //            {
    //                world[i,j] = n;
    //            }

    //        }

    //    }

    //}

    //// Prints out the world array
    //public static void printWorld(String header)
    //{
    //    Console.WriteLine(("=== "
    //                    + (header + " ===")));
    //    for (int i = 0; (i < world.GetLength(0)); i++)
    //    {
    //        String s = "";
    //        for (int j = 0; (j < world.GetLength(1)); j++)
    //        {
    //            s = (s + translate(world[i,j]));
    //        }

    //        Console.WriteLine(s);
    //    }

    //}

    // Prints out a given heightmap
    public static void printHeightmap(int[,] map, String header)
    {
        Console.WriteLine(("=== "
                        + (header + " ===")));
        for (int i = 0; (i < map.GetLength(0)); i++)
        {
            String s = "";
            for (int j = 0; (j < map.GetLength(1)); j++)
            {
                s = (s + map[i,j]);
            }

            Console.WriteLine(s);
        }

    }

    public static void printRivermap(int[,] map, int[,] h, String header)
    {
        Console.WriteLine(("=== "
                        + (header + " ===")));
        for (int i = 0; (i < map.GetLength(0)); i++)
        {
            String s = "";
            for (int j = 0; (j < map.GetLength(1)); j++)
            {
                if ((h[i,j] >= 7))
                {
                    if ((map[i,j] == 1))
                    {
                        s += "S";
                    }
                    else
                    {
                        s += "X";
                    }

                }
                else if ((h[i,j] == 0))
                {
                    s += "~";
                }
                else if ((map[i,j] == 0))
                {
                    s += "o";
                }
                else if ((map[i,j] == -1))
                {
                    s += "L";
                }
                else
                {
                    s += ".";
                }

            }

            Console.WriteLine(s);
        }

    }

    // Prints out a given String map
    public static void printBiomeMap(String[,] map, String header)
    {
        Console.WriteLine(("=== "
                        + (header + " ===")));
        for (int i = 0; (i < map.GetLength(0)); i++)
        {
            String s = "";
            for (int j = 0; (j < map.GetLength(1)); j++)
            {
                s = (s + map[i,j]);
            }

            Console.WriteLine(s);
        }

    }

    public static void printBiomeMapWithRivers(String[,] map, int[,] rivers, String header)
    {
        Console.WriteLine(("=== "
                        + (header + " ===")));
        for (int i = 0; (i < map.GetLength(0)); i++)
        {
            String s = "";
            for (int j = 0; (j < map.GetLength(1)); j++)
            {
                if (((rivers[i,j] == -1)
                            || (rivers[i,j] > 0)))
                {
                    s += ".";
                }
                else
                {
                    s = (s + map[i,j]);
                }

            }

            Console.WriteLine(s);
        }

    }

    // Manhattan distance
    public static int manDist(int x, int y, int a, int b)
    {
        return (Math.Abs((x - a)) + Math.Abs((y - b)));
        // return (int) (Math.sqrt(Math.pow(x-a, 2) + Math.pow(y-b, 2)) );
    }

    // Integer euclidean distance
    public static int dist(int x, int y, int a, int b)
    {
        // return Math.Abs((x-a)) + Math.Abs(y-b);
        return ((int)(Math.Sqrt((Math.Pow((x - a), 2) + Math.Pow((y - b), 2)))));
    }

    // Fills an area
    public static int[,] fill(int[,] w, int n)
    {
        for (int i = 0; (i < w.GetLength(0)); i++)
        {
            for (int j = 0; (j < w.GetLength(1)); j++)
            {
                w[i,j] = n;
            }

        }

        return w;
    }

    // The original map generation algorithm.
    //public static void generate50by80Map()
    //{
    //    Random r = new Random();
    //    worldx = 50;
    //    worldy = 80;
    //    world = new int[worldx];
    //    worldy;
    //    fill(world, 0);
    //    printWorld("Blank World");
    //    
    //    int r1 = UnityEngine.Random.Range(0,worldx);
    //    int r2 = UnityEngine.Random.Range(0,worldy);
    //    world[r1,r2] = 1;
    //    printWorld("Continent Generation (Step 1)");
    //    create(r1, r2, 10, 1);
    //    printWorld("Continent Generation (Step 2)");
    //    generateFeatures(1, 20);
    //    printWorld("Continent Generation (Step 3)");
    //    removeFeatures(0, 40);
    //    generateSmallFeatures(1, 25);
    //    removeFeatures(0, 25);
    //    printWorld("Continent Generation (Step 4)");
    //    smooth(4, 6);
    //    printWorld("Continent Generation (Step 5)");
    //    removeCorners(world);
    //    printWorld("Continent Generation (Step 6)");
    //    int[,] landmap = world.clone();
    //    generateMountainRanges(world, (UnityEngine.Random.Range(0,5) + 6));
    //    printWorld("Heightmap Generation (Step 1)");
    //    fillHeightmap(world);
    //    printWorld("Heightmap Generation (Step 2)");
    //    generateHills(world, 30);
    //    printWorld("Heightmap Generation (Step 3)");
    //    int[,] coastMap = createCoastMap(landmap);
    //    printHeightmap(landmap, "Heightmap");
    //    printHeightmap(coastMap, "Coast Map");
    //    int[,] rainShadow = generateRainShadowMap(landmap);
    //    printHeightmap(rainShadow, "Rain Shadow");
    //    String[,] biomeMap = createBiomeMap(coastMap, world, rainShadow);
    //    printBiomeMap(biomeMap, "Biome Generation (Step 1)");
    //    int[,] contMap = generateContinentMap(landmap);
    //    printHeightmap(contMap, "Continent Map");
    //}

}
