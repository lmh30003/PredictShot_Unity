using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class Map : MonoBehaviour {
    string mapName;
    public List<GameMapTile> mapTileData;
    List<GameObject> tileList;
    //public List<GameMapEvent> mapEvents { get; private set; }
    //public List<GeneralCard> mapDeck { get; private set; }
    public int width, height;
    
    public GameObject tile0, tile1, tile2, tile3, tile4, tile5, tile6, tile7;
    public PlayerVision mask;
    //private bool isDataSet = false;//when all data is set, set this value true. It will prevent other class accessing to setxxx method

    void readMap(string filename)
    {
        XmlTextReader reader = new XmlTextReader(filename);
        //read file
        string xmlpointer = null;
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    {
                        xmlpointer = reader.Name;
                        Console.WriteLine(xmlpointer);
                        break;
                    }
                case XmlNodeType.Text:
                    {

                        switch (xmlpointer)
                        {
                            case "Name":
                                mapName = reader.Value;
                                break;
                            case "X":
                                width = (byte)int.Parse(reader.Value);
                                break;
                            case "Y":
                                height = (byte)int.Parse(reader.Value);
                                break;
                            case "MapData":
                                int count = 0;
                                char[] temp;
                                short data;
                                for (int i = 0; i < reader.Value.Length; i += 2)
                                {
                                    temp = reader.Value.Substring(i, 2).ToCharArray();
                                    data = (short)((temp[0] << 8) | temp[1]);
                                    
                                    mapTileData.Add(new GameMapTile(data, count % width, count / width));

                                    count++;
                                }
                                Console.WriteLine("Total count : {0}", count);
                                reader.Read();
                                xmlpointer = null;
                                reader.Read();
                                break;
                            case "mapdeck":
                                //before implement this, make card database

                                break;
                            case "mapevent":
                                //before implement this, make mapevent database
                                break;
                            default:
                                break;
                        }
                        break;
                    }
                case XmlNodeType.EndElement:

                default:
                    break;
            }
        }
        //end reading file
        reader.Close();
        Console.WriteLine(ToString());
    }

    // Use this for initialization
    void Start () {
        mapTileData = new List<GameMapTile>();
        readMap("Assets/Maps/TestMap.xml");
        tileList = new List<GameObject>(8);
        tileList.Add(tile0);
        tileList.Add(tile1);
        tileList.Add(tile2);
        tileList.Add(tile3);
        tileList.Add(tile4);
        tileList.Add(tile5);
        tileList.Add(tile6);
        tileList.Add(tile7);
        foreach (GameMapTile gm in mapTileData)
        {
            Instantiate(tileList[gm.height], new Vector3(gm.xloc, gm.yloc, 0f), Quaternion.identity);
        }

	}
	public void updateMap()
    {
        mask.updateMap();
    }
	// Update is called once per frame
	void Update () {
		
	}
    public class GameMapTile
    {
        public const byte MOVABLE_BIT = 1 << 3;//if this is set, this tile is movable
        public const byte FLOW_BIT = 1 << 4;//if this is set, this tile is part of big flow
        public const byte VISIBLE_BIT = 1 << 5;//if this is set, player can see through this tile
        public const byte BREAKABLE_BIT = 1 << 6;//if this is set, player can destroy this tile. destroyed tile will be movable, visible, and flowable.
        public const byte attrib2 = 1 << 7;//maybe used later
        public bool isMovable;
        public bool isVisible;
        public bool isFlowing;
        public bool isBreakable;
        public byte height;//0~7
        public byte HP;
        public int xloc, yloc;
        public GameMapTile() { }
        public GameMapTile(short data)
        {
            height = ((byte)(data & 7));
            isMovable = ((data & MOVABLE_BIT) != 0);
            isVisible = ((data & VISIBLE_BIT) != 0);
            isFlowing = ((data & FLOW_BIT) != 0);
            isBreakable = ((data & BREAKABLE_BIT) != 0);
            HP = (byte)((data & 127 << 8) >> 8);
        }
        public GameMapTile(short data, int _xloc, int _yloc) : this(data)
        {
            xloc = _xloc;
            yloc = _yloc;
        }
        public short toShort()
        {
            short i = height;
            if (isMovable) i |= MOVABLE_BIT;
            if (isVisible) i |= VISIBLE_BIT;
            if (isFlowing) i |= FLOW_BIT;
            if (isBreakable) i |= BREAKABLE_BIT;
            i |= (short)(HP << 8);
            return i;
        }
    }
}
