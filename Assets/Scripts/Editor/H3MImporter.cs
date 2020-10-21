using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;
using UnityEditor;
using UnityEngine;
using System.Text;
using System;

public class H3MImporter : EditorWindow
{
    // Data structure used during importing to assist in determining draw order
    class ScenarioObjectSortData
    {
        public ScenarioObject Object;
        public int FileOrder;

        // Children meaning objects that need to render below this one
        public List<ScenarioObjectSortData> Children = new List<ScenarioObjectSortData>();

        public ScenarioObjectSortData(ScenarioObject a_Object, int a_FileOrder)
        {
            Object = a_Object;
            FileOrder = a_FileOrder;
        }
    }

    string m_InputFolder;
    string m_OutputFolder;
    bool m_Overwrite;

    [MenuItem("Window/H3M Importer")]
    static void Initialize()
    {
        H3MImporter window = (H3MImporter)EditorWindow.GetWindow(typeof(H3MImporter));
        window.Show();

        window.m_InputFolder = "../Maps/";
        window.m_OutputFolder = "Maps/";
        window.m_Overwrite = true;
    }

    void OnGUI()
    {
        m_InputFolder = EditorGUILayout.TextField(new GUIContent("Input Folder", "Path starts from within the assets folder"), m_InputFolder);
        m_OutputFolder = EditorGUILayout.TextField(new GUIContent("Output Folder", "Path starts from within the assets folder"), m_OutputFolder);
        m_Overwrite = EditorGUILayout.Toggle("Overwrite", m_Overwrite);

        EditorGUILayout.Space();

        if (GUILayout.Button("Import"))
        {
            Import();
        }
    }

    void Import()
    {
        List<string> _InputFiles = new List<string>(Directory.GetFiles(Application.dataPath + "/" + m_InputFolder));

        _InputFiles = _InputFiles.Where((a_File) => a_File.EndsWith(".h3m")).ToList();

        int _StartIndex = _InputFiles[0].Replace('\\', '/').LastIndexOf('/') + 1;

        if (!m_Overwrite)
        {
            string[] _ExistingMaps = Directory.GetFiles(Application.dataPath + "/" + m_OutputFolder, "*.asset");

            if (_ExistingMaps.Length > 0)
            {
                int _ExistingStartIndex = _ExistingMaps[0].Replace('\\', '/').LastIndexOf('/') + 1;

                for (int i = 0; i < _ExistingMaps.Length; i++)
                {
                    _ExistingMaps[i] = _ExistingMaps[i].Substring(_ExistingStartIndex, _ExistingMaps[i].Length - _ExistingStartIndex - 6);
                }

                for (int i = _InputFiles.Count - 1; i >= 0; i--)
                {
                    int _Length = _InputFiles[i].Length - 4 - _StartIndex;
                    string _Name = _InputFiles[i].Substring(_StartIndex, _Length);

                    for (int j = 0; j < _ExistingMaps.Length; j++)
                    {
                        if (_ExistingMaps[j] == _Name)
                        {
                            _InputFiles.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        for (int i = 0; i < _InputFiles.Count; i++)
        {
            Scenario _Scenario = ScriptableObject.CreateInstance<Scenario>();

            int _Length = _InputFiles[i].Length - 4 - _StartIndex;
            _Scenario.name = _InputFiles[i].Substring(_StartIndex, _Length);

            List<byte> _BytesList = new List<byte>();

            using (FileStream _FileStream = new FileStream(_InputFiles[i], FileMode.Open))
            {
                using (GZipStream _GZipStream = new GZipStream(_FileStream, CompressionMode.Decompress))
                {
                    while (true)
                    {
                        int _Byte = _GZipStream.ReadByte();

                        if (_Byte == -1)
                        {
                            break;
                        }

                        _BytesList.Add((byte)_Byte);
                    }
                }
            }

            // <><><><><> Basic Scenario Data

            byte[] _Bytes = _BytesList.ToArray();

            _Scenario.Version = BitConverter.ToInt32(_Bytes, 0);

            switch (_Scenario.Version)
            {
                case Scenario.RESTORATION_OF_ERATHIA: ReadRoE(ref _Scenario, _Bytes); break;
                case Scenario.ARMAGEDDONS_BLADE: ReadAB(ref _Scenario, _Bytes); break;
                case Scenario.SHADOW_OF_DEATH: ReadSoD(ref _Scenario, _Bytes); break;
            }

            int _ComputerCount = 0;
            int _PlayerCount = 0;

            for (int j = 0; j < _Scenario.PlayerInfo.Count; j++)
            {
                if (_Scenario.PlayerInfo[j].ComputerPlayable)
                {
                    _ComputerCount++;
                }

                if (_Scenario.PlayerInfo[j].HumanPlayable)
                {
                    _PlayerCount++;
                }
            }

            _Scenario.PlayerCount = _PlayerCount;
            _Scenario.ComputerCount = _ComputerCount;
            
            AssetDatabase.CreateAsset(_Scenario, "Assets/" + m_OutputFolder + _Scenario.name + ".asset");
        }
    }

    void ReadRoE(ref Scenario a_Scenario, Byte[] a_Bytes)
    {
        try
        {
            int _CurrentByte = 0;

            _CurrentByte += 4;

            _CurrentByte += 1; // Skip unknown byte

            a_Scenario.Size = BitConverter.ToInt32(a_Bytes, _CurrentByte);

            _CurrentByte += 4;

            a_Scenario.HasUnderground = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

            _CurrentByte += 1;

            // Set Name and Description
            int _NameLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);

            _CurrentByte += 4;

            a_Scenario.Name = Encoding.UTF8.GetString(a_Bytes, 14, _NameLength);

            _CurrentByte += _NameLength;

            int _DescLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);

            _CurrentByte += 4;

            a_Scenario.Description = Encoding.UTF8.GetString(a_Bytes, _CurrentByte, _DescLength);

            _CurrentByte += _DescLength;

            a_Scenario.Difficulty = a_Bytes[_CurrentByte];

            _CurrentByte += 1;

            // <><><><><> Player Specs

            a_Scenario.PlayerInfo = new List<PlayerInfo>(8);

            for (int j = 0; j < 8; j++)
            {
                PlayerInfo _PlayerInfo = new PlayerInfo();
                a_Scenario.PlayerInfo.Add(_PlayerInfo);

                _PlayerInfo.HumanPlayable = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

                _CurrentByte += 1;

                _PlayerInfo.ComputerPlayable = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

                _CurrentByte += 1;

                _PlayerInfo.AIBehaviour = a_Bytes[_CurrentByte];

                _CurrentByte += 1;

                _PlayerInfo.AllowedTowns = BitConverter.ToUInt16(a_Bytes, _CurrentByte);

                _CurrentByte += 2;

                _PlayerInfo.HasMainTown = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

                _CurrentByte += 1;

                if (_PlayerInfo.HasMainTown)
                {
                    _PlayerInfo.MainTownXCoord = a_Bytes[_CurrentByte];

                    _CurrentByte += 1;

                    _PlayerInfo.MainTownYCoord = a_Bytes[_CurrentByte];

                    _CurrentByte += 1;

                    _PlayerInfo.IsMainTownUnderground = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

                    _CurrentByte += 1;
                }

                _CurrentByte += 1; // Unknown - 00

                _PlayerInfo.MainHeroType = a_Bytes[_CurrentByte];

                _CurrentByte += 1;

                if (_PlayerInfo.MainHeroType != 255)
                {
                    _CurrentByte += 1; // Skip hero portrait

                    int _HeroNameLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);

                    _CurrentByte += 4;

                    _PlayerInfo.MainHeroName = Encoding.UTF8.GetString(a_Bytes, _CurrentByte, _NameLength);

                    _CurrentByte += _HeroNameLength;
                }
            }

            // <><><><><> Win Condition

            a_Scenario.WinCondition = a_Bytes[_CurrentByte];

            _CurrentByte += 1;

            switch (a_Scenario.WinCondition)
            {
                case 0: _CurrentByte += 4; break;
                case 1: _CurrentByte += 8; break;
                case 2: _CurrentByte += 7; break;
                case 3: _CurrentByte += 7; break;
                case 4: _CurrentByte += 5; break;
                case 5: _CurrentByte += 5; break;
                case 6: _CurrentByte += 5; break;
                case 7: _CurrentByte += 5; break;
                case 8: _CurrentByte += 2; break;
                case 9: _CurrentByte += 2; break;
                case 10: _CurrentByte += 6; break;
            }

            // <><><><><> Loss Condition

            a_Scenario.LossCondition = a_Bytes[_CurrentByte];

            _CurrentByte += 1;

            switch (a_Scenario.LossCondition)
            {
                case 0: _CurrentByte += 3; break;
                case 1: _CurrentByte += 3; break;
                case 2: _CurrentByte += 2; break;
            }

            // <><><><><> Teams

            int _NumberOfTeams = a_Bytes[_CurrentByte];

            _CurrentByte += 1;

            a_Scenario.HasTeams = false;

            if (_NumberOfTeams > 0)
            {
                byte _Team = a_Scenario.PlayerInfo[0].Team;

                for (int i = 0; i < 8; i++)
                {
                    a_Scenario.PlayerInfo[i].Team = a_Bytes[_CurrentByte];

                    _CurrentByte += 1;
                }
                
                for (int i = 0; i < 8; i++)
                {
                    if (a_Scenario.PlayerInfo[i].ComputerPlayable)
                    {
                        _Team = a_Scenario.PlayerInfo[i].Team;
                        break;
                    }
                }
                
                for (int i = 0; i < 8; i++)
                {
                    if (a_Scenario.PlayerInfo[i].ComputerPlayable &&
                        a_Scenario.PlayerInfo[i].Team != _Team)
                    {
                        a_Scenario.HasTeams = true;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Conversion of RoE map '{a_Scenario.Name}' failed\n{e}");
        }
    }
    void ReadAB(ref Scenario a_Scenario, Byte[] a_Bytes)
    {
        try
        {
            int _CurrentByte = 0;

            _CurrentByte += 4;

            _CurrentByte += 1; // Skip unknown byte

            a_Scenario.Size = BitConverter.ToInt32(a_Bytes, _CurrentByte);

            _CurrentByte += 4;

            a_Scenario.HasUnderground = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

            _CurrentByte += 1;

            // Set Name and Description
            int _NameLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);

            _CurrentByte += 4;

            a_Scenario.Name = Encoding.UTF8.GetString(a_Bytes, 14, _NameLength);

            _CurrentByte += _NameLength;

            int _DescLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);

            _CurrentByte += 4;

            a_Scenario.Description = Encoding.UTF8.GetString(a_Bytes, _CurrentByte, _DescLength);

            _CurrentByte += _DescLength;

            a_Scenario.Difficulty = a_Bytes[_CurrentByte];

            _CurrentByte += 1;

            _CurrentByte += 1; // Skipping level limit

            // <><><><><> Player Specs

            a_Scenario.PlayerInfo = new List<PlayerInfo>(8);

            for (int j = 0; j < 8; j++)
            {
                PlayerInfo _PlayerInfo = new PlayerInfo();
                a_Scenario.PlayerInfo.Add(_PlayerInfo);

                _PlayerInfo.HumanPlayable = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

                _CurrentByte += 1;

                _PlayerInfo.ComputerPlayable = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

                _CurrentByte += 1;

                _PlayerInfo.AIBehaviour = a_Bytes[_CurrentByte];

                _CurrentByte += 1;

                _PlayerInfo.AllowedTowns = BitConverter.ToUInt16(a_Bytes, _CurrentByte);

                _CurrentByte += 2;

                _PlayerInfo.IsTownChoosable = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

                _CurrentByte += 1;

                _PlayerInfo.HasMainTown = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

                _CurrentByte += 1;

                if (_PlayerInfo.HasMainTown)
                {
                    _PlayerInfo.GenerateHeroAtMainTown = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

                    _CurrentByte += 1;

                    _PlayerInfo.MainTownType = a_Bytes[_CurrentByte];

                    _CurrentByte += 1;

                    _PlayerInfo.MainTownXCoord = a_Bytes[_CurrentByte];

                    _CurrentByte += 1;

                    _PlayerInfo.MainTownYCoord = a_Bytes[_CurrentByte];

                    _CurrentByte += 1;

                    _PlayerInfo.IsMainTownUnderground = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

                    _CurrentByte += 1;
                }

                _CurrentByte += 1; // Unknown - 00

                _PlayerInfo.MainHeroType = a_Bytes[_CurrentByte];

                _CurrentByte += 1;

                if (_PlayerInfo.MainHeroType != 255)
                {
                    _CurrentByte += 1; // Skip hero portrait

                    int _HeroNameLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);

                    _CurrentByte += 4;

                    _PlayerInfo.MainHeroName = Encoding.UTF8.GetString(a_Bytes, _CurrentByte, _NameLength);

                    _CurrentByte += _HeroNameLength;
                }

                _CurrentByte += 1; // Skip unknown byte

                int _HeroCount = BitConverter.ToInt32(a_Bytes, _CurrentByte);

                _CurrentByte += 4;

                _PlayerInfo.HeroNames = new List<string>();

                for (int k = 0; k < _HeroCount; k++)
                {
                    _CurrentByte += 1; // Skip hero portrait

                    int _HeroNameLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);

                    _CurrentByte += 4;

                    _PlayerInfo.HeroNames.Add(Encoding.UTF8.GetString(a_Bytes, _CurrentByte, _NameLength));

                    _CurrentByte += _HeroNameLength;
                }
            }

            // <><><><><> Win Condition

            a_Scenario.WinCondition = a_Bytes[_CurrentByte];

            _CurrentByte += 1;

            switch (a_Scenario.WinCondition)
            {
                case 0: _CurrentByte += 4; break;
                case 1: _CurrentByte += 8; break;
                case 2: _CurrentByte += 7; break;
                case 3: _CurrentByte += 7; break;
                case 4: _CurrentByte += 5; break;
                case 5: _CurrentByte += 5; break;
                case 6: _CurrentByte += 5; break;
                case 7: _CurrentByte += 5; break;
                case 8: _CurrentByte += 2; break;
                case 9: _CurrentByte += 2; break;
                case 10: _CurrentByte += 6; break;
            }

            // <><><><><> Loss Condition

            a_Scenario.LossCondition = a_Bytes[_CurrentByte];

            _CurrentByte += 1;

            switch (a_Scenario.LossCondition)
            {
                case 0: _CurrentByte += 3; break;
                case 1: _CurrentByte += 3; break;
                case 2: _CurrentByte += 2; break;
            }

            // <><><><><> Teams

            int _NumberOfTeams = a_Bytes[_CurrentByte];

            _CurrentByte += 1;

            a_Scenario.HasTeams = false;

            if (_NumberOfTeams > 0)
            {
                byte _Team = a_Scenario.PlayerInfo[0].Team;

                for (int i = 0; i < 8; i++)
                {
                    a_Scenario.PlayerInfo[i].Team = a_Bytes[_CurrentByte];

                    _CurrentByte += 1;
                }
                
                for (int i = 0; i < 8; i++)
                {
                    if (a_Scenario.PlayerInfo[i].ComputerPlayable)
                    {
                        _Team = a_Scenario.PlayerInfo[i].Team;
                        break;
                    }
                }
                
                for (int i = 0; i < 8; i++)
                {
                    if (a_Scenario.PlayerInfo[i].ComputerPlayable &&
                        a_Scenario.PlayerInfo[i].Team != _Team)
                    {
                        a_Scenario.HasTeams = true;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Conversion of AB map '{a_Scenario.Name}' failed\n{e}");
        }
    }

    void ReadSoD(ref Scenario a_Scenario, Byte[] a_Bytes)
    {
        try
        {
            int _StringLength;

            int _CurrentByte = 0;

            _CurrentByte += 4;

            _CurrentByte += 1; // Skip unknown byte

            a_Scenario.Size = BitConverter.ToInt32(a_Bytes, _CurrentByte);

            _CurrentByte += 4;

            a_Scenario.HasUnderground = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

            _CurrentByte += 1;

            // Set Name and Description
            _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);

            _CurrentByte += 4;

            a_Scenario.Name = Encoding.UTF8.GetString(a_Bytes, 14, _StringLength);

            _CurrentByte += _StringLength;

            _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);

            _CurrentByte += 4;

            a_Scenario.Description = Encoding.UTF8.GetString(a_Bytes, _CurrentByte, _StringLength);

            _CurrentByte += _StringLength;

            a_Scenario.Difficulty = a_Bytes[_CurrentByte];

            _CurrentByte += 1;

            _CurrentByte += 1; // Skipping level limit

            // <><><><><> Player Specs

            a_Scenario.PlayerInfo = new List<PlayerInfo>(8);

            for (int j = 0; j < 8; j++)
            {
                PlayerInfo _PlayerInfo = new PlayerInfo();
                a_Scenario.PlayerInfo.Add(_PlayerInfo);

                _PlayerInfo.HumanPlayable = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

                _CurrentByte += 1;

                _PlayerInfo.ComputerPlayable = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

                _CurrentByte += 1;

                _PlayerInfo.AIBehaviour = a_Bytes[_CurrentByte];

                _CurrentByte += 1;

                _PlayerInfo.LimitedTownChoice = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

                _CurrentByte += 1;

                _PlayerInfo.AllowedTowns = BitConverter.ToUInt16(a_Bytes, _CurrentByte);

                _CurrentByte += 2;

                _PlayerInfo.IsTownChoosable = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

                _CurrentByte += 1;

                _PlayerInfo.HasMainTown = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

                _CurrentByte += 1;

                if (_PlayerInfo.HasMainTown)
                {
                    _PlayerInfo.GenerateHeroAtMainTown = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

                    _CurrentByte += 1;

                    _PlayerInfo.MainTownType = a_Bytes[_CurrentByte];

                    _CurrentByte += 1;

                    _PlayerInfo.MainTownXCoord = a_Bytes[_CurrentByte];

                    _CurrentByte += 1;

                    _PlayerInfo.MainTownYCoord = a_Bytes[_CurrentByte];

                    _CurrentByte += 1;

                    _PlayerInfo.IsMainTownUnderground = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

                    _CurrentByte += 1;
                }

                _PlayerInfo.IsMainHeroRandom = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

                _CurrentByte += 1;

                _PlayerInfo.MainHeroType = a_Bytes[_CurrentByte];

                _CurrentByte += 1;

                if (_PlayerInfo.MainHeroType != 255)
                {
                    _CurrentByte += 1; // Skip hero portrait

                    _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);

                    _CurrentByte += 4;

                    _PlayerInfo.MainHeroName = Encoding.UTF8.GetString(a_Bytes, _CurrentByte, _StringLength);

                    _CurrentByte += _StringLength;
                }

                _CurrentByte += 1; // Skip unknown byte

                int _HeroCount = BitConverter.ToInt32(a_Bytes, _CurrentByte);

                _CurrentByte += 4;

                _PlayerInfo.HeroNames = new List<string>();

                for (int k = 0; k < _HeroCount; k++)
                {
                    _CurrentByte += 1; // Skip hero portrait

                    _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);

                    _CurrentByte += 4;

                    _PlayerInfo.HeroNames.Add(Encoding.UTF8.GetString(a_Bytes, _CurrentByte, _StringLength));

                    _CurrentByte += _StringLength;
                }
            }

            // <><><><><> Win Condition

            a_Scenario.WinCondition = a_Bytes[_CurrentByte];

            _CurrentByte += 1;

            switch (a_Scenario.WinCondition)
            {
                case 0: _CurrentByte += 4; break;
                case 1: _CurrentByte += 8; break;
                case 2: _CurrentByte += 7; break;
                case 3: _CurrentByte += 7; break;
                case 4: _CurrentByte += 5; break;
                case 5: _CurrentByte += 5; break;
                case 6: _CurrentByte += 5; break;
                case 7: _CurrentByte += 5; break;
                case 8: _CurrentByte += 2; break;
                case 9: _CurrentByte += 2; break;
                case 10: _CurrentByte += 6; break;
            }

            // <><><><><> Loss Condition

            a_Scenario.LossCondition = a_Bytes[_CurrentByte];

            _CurrentByte += 1;

            switch (a_Scenario.LossCondition)
            {
                case 0: _CurrentByte += 3; break;
                case 1: _CurrentByte += 3; break;
                case 2: _CurrentByte += 2; break;
            }

            // <><><><><> Teams

            int _NumberOfTeams = a_Bytes[_CurrentByte];

            _CurrentByte += 1;

            a_Scenario.HasTeams = false;

            if (_NumberOfTeams > 0)
            {
                byte _Team = a_Scenario.PlayerInfo[0].Team;

                for (int i = 0; i < 8; i++)
                {
                    a_Scenario.PlayerInfo[i].Team = a_Bytes[_CurrentByte];

                    _CurrentByte += 1;
                }
                
                for (int i = 0; i < 8; i++)
                {
                    if (a_Scenario.PlayerInfo[i].ComputerPlayable)
                    {
                        _Team = a_Scenario.PlayerInfo[i].Team;
                        break;
                    }
                }
                
                for (int i = 0; i < 8; i++)
                {
                    if (a_Scenario.PlayerInfo[i].ComputerPlayable &&
                        a_Scenario.PlayerInfo[i].Team != _Team)
                    {
                        a_Scenario.HasTeams = true;
                    }
                }
            }

            // <><><><><> Unsupported Junk (for now)
            _CurrentByte += 24;

            int _Count = a_Bytes[_CurrentByte];
            _CurrentByte += 1;

            for (int i = 0; i < _Count; i++)
            {
                _CurrentByte += 2;
                
                _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                _CurrentByte += 4;

                _CurrentByte += _StringLength;
                _CurrentByte += 1;
            }

            _CurrentByte += 31;
            _CurrentByte += 18;
            _CurrentByte += 9;
            _CurrentByte += 4;

            _Count = BitConverter.ToInt32(a_Bytes, _CurrentByte);
            _CurrentByte += 4;

            for (int i = 0; i < _Count; i++)
            {
                _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                _CurrentByte += 4;
                _CurrentByte += _StringLength;

                _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                _CurrentByte += 4;
                _CurrentByte += _StringLength;
            }

            for (int i = 0; i < 156; i++)
            {
                bool _Bool = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                _CurrentByte += 1;

                if (_Bool)
                {
                    _Bool = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                    _CurrentByte += 1;

                    if (_Bool)
                    {
                        _CurrentByte += 4;
                    }

                    _Bool = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                    _CurrentByte += 1;

                    if (_Bool)
                    {
                        _Count = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                        _CurrentByte += 4;
                        _CurrentByte += _Count * 4;
                    }

                    _Bool = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                    _CurrentByte += 1;

                    if (_Bool)
                    {
                        _CurrentByte += 38;

                        _Count = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                        _CurrentByte += 4;
                        _CurrentByte += _Count * 2;
                    }

                    _Bool = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                    _CurrentByte += 1;

                    if (_Bool)
                    {
                        _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                        _CurrentByte += 4;
                        _CurrentByte += _StringLength;
                    }

                    _CurrentByte += 1;

                    _Bool = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                    _CurrentByte += 1;

                    if (_Bool)
                    {
                        _CurrentByte += 9;
                    }

                    _Bool = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                    _CurrentByte += 1;

                    if (_Bool)
                    {
                        _CurrentByte += 4;
                    }
                }
            }

            // <><><><><> TERRAIN

            a_Scenario.Terrain = new List<TerrainTile>(a_Scenario.Size * a_Scenario.Size);

            for (int i = 0; i < a_Scenario.Terrain.Capacity; i++)
            {
                TerrainTile _Tile = new TerrainTile();

                _Tile.TerrainType = a_Bytes[_CurrentByte];
                _Tile.TerrainSpriteID = a_Bytes[_CurrentByte + 1];
                _Tile.RiverType = a_Bytes[_CurrentByte + 2];
                _Tile.RiverSpriteID = a_Bytes[_CurrentByte + 3];
                _Tile.RoadType = a_Bytes[_CurrentByte + 4];
                _Tile.RoadSpriteID = a_Bytes[_CurrentByte + 5];
                _Tile.Mirrored = a_Bytes[_CurrentByte + 6];

                _CurrentByte += 7;

                a_Scenario.Terrain.Add(_Tile);
            }

            if (a_Scenario.HasUnderground)
            {
                a_Scenario.UndergroundTerrain = new List<TerrainTile>(a_Scenario.Size * a_Scenario.Size);

                for (int i = 0; i < a_Scenario.UndergroundTerrain.Capacity; i++)
                {
                    TerrainTile _Tile = new TerrainTile();

                    _Tile.TerrainType = a_Bytes[_CurrentByte];
                    _Tile.TerrainSpriteID = a_Bytes[_CurrentByte + 1];
                    _Tile.RiverType = a_Bytes[_CurrentByte + 2];
                    _Tile.RiverSpriteID = a_Bytes[_CurrentByte + 3];
                    _Tile.RoadType = a_Bytes[_CurrentByte + 4];
                    _Tile.RoadSpriteID = a_Bytes[_CurrentByte + 5];
                    _Tile.Mirrored = a_Bytes[_CurrentByte + 6];

                    _CurrentByte += 7;

                    a_Scenario.UndergroundTerrain.Add(_Tile);
                }
            }

            // <><><><><> OBJECTS

            int _ObjectCount = BitConverter.ToInt32(a_Bytes, _CurrentByte);
            _CurrentByte += 4;

            List<ScenarioObjectTemplate> _ObjectTemplates = new List<ScenarioObjectTemplate>();

            for (int i = 0; i < _ObjectCount; i++)
            {
                ScenarioObjectTemplate _Object = new ScenarioObjectTemplate();

                _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                _CurrentByte += 4;

                _Object.Name = Encoding.UTF8.GetString(a_Bytes, _CurrentByte, _StringLength);
                _Object.Name = _Object.Name.Substring(0, _Object.Name.Length - 4).ToLower();
                _CurrentByte += _StringLength;

                _Object.Passability = new byte[6];
                _Object.Passability[0] = a_Bytes[_CurrentByte];
                _Object.Passability[1] = a_Bytes[_CurrentByte + 1];
                _Object.Passability[2] = a_Bytes[_CurrentByte + 2];
                _Object.Passability[3] = a_Bytes[_CurrentByte + 3];
                _Object.Passability[4] = a_Bytes[_CurrentByte + 4];
                _Object.Passability[5] = a_Bytes[_CurrentByte + 5];

                _CurrentByte += 6;

                _Object.Interactability = new byte[6];
                _Object.Interactability[0] = a_Bytes[_CurrentByte];
                _Object.Interactability[1] = a_Bytes[_CurrentByte + 1];
                _Object.Interactability[2] = a_Bytes[_CurrentByte + 2];
                _Object.Interactability[3] = a_Bytes[_CurrentByte + 3];
                _Object.Interactability[4] = a_Bytes[_CurrentByte + 4];
                _Object.Interactability[5] = a_Bytes[_CurrentByte + 5];

                _CurrentByte += 6;

                // 2 bytes - Landscape?
                // 2 bytes - LandEditGroups?
                _CurrentByte += 4;

                int _Type = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                _Object.TypeDebug = _Type;
                _CurrentByte += 4;

                _Object.MineType = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                _CurrentByte += 4;


                switch (_Type)
                {
                    case 5:
                    case 65:
                    case 66:
                    case 67:
                    case 68:
                    case 69:
                        _Object.Type = ScenarioObjectType.Artifact;
                        break;

                    case 6:
                        _Object.Type = ScenarioObjectType.PandorasBox;
                        break;

                    case 17:
                    case 20:
                    case 42:
                    case 87:
                        _Object.Type = ScenarioObjectType.Dwelling;
                        break;

                    case 26:
                        _Object.Type = ScenarioObjectType.Event;
                        break;

                    case 33:
                    case 219:
                        _Object.Type = ScenarioObjectType.Garrison;
                        break;

                    case 34:
                    case 70:
                    case 62:
                        _Object.Type = ScenarioObjectType.Hero;
                        break;

                    case 36:
                        _Object.Type = ScenarioObjectType.Grail;
                        break;

                    case 53: // Mine
                        // Potentially some weird stuff with abandoned mines

                        if (_Object.MineType == 7)
                        {
                            Debug.LogError($"CODE RED @@@@@@@@@@@@@@@@@@@@@@@@@@");
                        }

                        _Object.Type = ScenarioObjectType.Dwelling;
                        break;

                    case 54:
                    case 71:
                    case 72:
                    case 73:
                    case 74:
                    case 75:
                    case 162:
                    case 163:
                    case 164:
                        _Object.Type = ScenarioObjectType.Monster;
                        break;
                    
                    case 76:
                    case 79:
                        _Object.Type = ScenarioObjectType.Resource;
                        break;

                    case 81:
                        _Object.Type = ScenarioObjectType.Scholar;
                        break;

                    case 83:
                        _Object.Type = ScenarioObjectType.Seer;
                        break;

                    case 88:
                    case 89:
                    case 99:
                        _Object.Type = ScenarioObjectType.Shrine;
                        break;

                    case 91:
                    case 59:
                        _Object.Type = ScenarioObjectType.Sign;
                        break;

                    case 93:
                        _Object.Type = ScenarioObjectType.Spell;
                        break;

                    case 98:
                    case 77:
                        _Object.Type = ScenarioObjectType.Town;
                        break;

                    case 113:
                        _Object.Type = ScenarioObjectType.WitchsHut;
                        break;

                    case 215:
                        _Object.Type = ScenarioObjectType.QuestionGuard;
                        break;

                    case 216:
                        _Object.Type = ScenarioObjectType.GeneralDwelling;
                        break;

                    case 217:
                        _Object.Type = ScenarioObjectType.LevelDwelling;
                        break;

                    case 218:
                        _Object.Type = ScenarioObjectType.TownDwelling;
                        break;

                    case 220:
                        _Object.Type = ScenarioObjectType.AbandonedMine;
                        break;

                    default:
                        _Object.Type = ScenarioObjectType.Unknown;
                        break;
                }

                // 1 byte - Object Group?
                _CurrentByte += 1;

                _Object.IsLowPrioritySortOrder = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                _CurrentByte += 1;

                // 16 bytes - Unknown
                _CurrentByte += 16;

                _ObjectTemplates.Add(_Object);
            }

            int _ObjectDataCount = BitConverter.ToInt32(a_Bytes, _CurrentByte);
            _CurrentByte += 4;

            a_Scenario.Objects = new List<ScenarioObject>();

            for (int i = 0; i < _ObjectDataCount; i++)
            {
                byte _XPos = a_Bytes[_CurrentByte];
                byte _YPos = a_Bytes[_CurrentByte + 1];
                bool _IsUnderground = BitConverter.ToBoolean(a_Bytes, _CurrentByte + 2);
                _CurrentByte += 3;

                int _ObjectBaseIndex = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                _CurrentByte += 4;

                _CurrentByte += 5; // Unknown Bytes


                if (_ObjectBaseIndex > _ObjectTemplates.Count)
                {
                    Debug.Log($"Position: {_CurrentByte.ToString("X")}");
                    Debug.Log($"Index out of range: {_ObjectBaseIndex} - Position: {_XPos}, {_YPos}, {_IsUnderground}");
                    break;
                }

                ScenarioObject _Object = new ScenarioObject();
                _Object.Template = _ObjectTemplates[_ObjectBaseIndex];

                //Debug.Log($"Object Base: {_Object.Template.Name} - {_ObjectBaseIndex} - {_Object.Template.Type}/{_Object.Template.TypeDebug} - {_CurrentByte.ToString("X")} - {i}/{_ObjectDataCount}");

                _Object.XPos = _XPos;
                _Object.YPos = _YPos;
                _Object.IsUnderground = _IsUnderground;

                switch (_Object.Template.Type)
                {
                    case ScenarioObjectType.Artifact:
                        bool _HasMessage = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                        _CurrentByte += 1;

                        if (_HasMessage)
                        {
                            _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                            _CurrentByte += 4;
                            _CurrentByte += _StringLength;

                            bool _HasGuards = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                            _CurrentByte += 1;

                            if (_HasGuards)
                            {
                                _CurrentByte += 28;
                            }
                        }
                        break;

                    case ScenarioObjectType.Dwelling:
                        _Object.DwellingOwner = BitConverter.ToUInt32(a_Bytes, _CurrentByte);
                        _CurrentByte += 4;
                        break;

                    case ScenarioObjectType.Garrison:
                        _CurrentByte += 41;
                        break;

                    case ScenarioObjectType.Hero:
                        _CurrentByte += 4;
                        _CurrentByte += 1;
                        _CurrentByte += 1;

                        bool _HasCustomName = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                        _CurrentByte += 1;

                        if (_HasCustomName)
                        {
                            _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                            _CurrentByte += 4;
                            _CurrentByte += _StringLength;
                        }

                        bool _HasCustomExp = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                        _CurrentByte += 1;

                        if (_HasCustomExp)
                        {
                            _CurrentByte += 4;
                        }

                        bool _HasCustomPortrait = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                        _CurrentByte += 1;

                        if (_HasCustomPortrait)
                        {
                            _CurrentByte += 1;
                        }

                        bool _HasCustomSecondarySkills = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                        _CurrentByte += 1;

                        if (_HasCustomSecondarySkills)
                        {
                            int _SecondarySkillCount = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                            _CurrentByte += 4;

                            _CurrentByte += _SecondarySkillCount * 2;
                        }

                        bool _HasCustomArmy = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                        _CurrentByte += 1;

                        if (_HasCustomArmy)
                        {
                            _CurrentByte += 28;
                        }

                        _CurrentByte += 1; // Monster formation

                        bool _HasCustomArtifacts = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                        _CurrentByte += 1;

                        if (_HasCustomArtifacts)
                        {
                            _CurrentByte += 38;

                            short _UnequippedArtifactCount = BitConverter.ToInt16(a_Bytes, _CurrentByte);
                            _CurrentByte += 2;

                            _CurrentByte += _UnequippedArtifactCount * 2;
                        }

                        _CurrentByte += 1; // Zone Radius?

                        bool _HasCustomBiography = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                        _CurrentByte += 1;

                        if (_HasCustomBiography)
                        {
                            _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                            _CurrentByte += 4;
                            _CurrentByte += _StringLength;
                        }

                        _CurrentByte += 1; // Gender

                        bool _HasCustomSpells = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                        _CurrentByte += 1;

                        if (_HasCustomSpells)
                        {
                            _CurrentByte += 9;
                        }

                        bool _hasCustomPrimarySkills = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                        _CurrentByte += 1;

                        if (_hasCustomPrimarySkills)
                        {
                            _CurrentByte += 4;
                        }

                        _CurrentByte += 16;

                        break;

                    case ScenarioObjectType.Monster:
                        _Object.Monster = new ScenarioObjectMonster();

                        _Object.Monster.Type = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                        _CurrentByte += 4;

                        _Object.Monster.Count = BitConverter.ToInt16(a_Bytes, _CurrentByte);
                        _CurrentByte += 2;

                        _Object.Monster.Mood = a_Bytes[_CurrentByte];
                        _CurrentByte += 1;

                        _Object.Monster.IsTreasureOrText = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                        _CurrentByte += 1;

                        if (_Object.Monster.IsTreasureOrText)
                        {
                            _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                            _CurrentByte += 4;

                            _Object.Monster.Text = Encoding.UTF8.GetString(a_Bytes, _CurrentByte, _StringLength);
                            _CurrentByte += _StringLength;

                            _Object.Monster.Wood = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                            _CurrentByte += 4;
                            _Object.Monster.Mercury = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                            _CurrentByte += 4;
                            _Object.Monster.Ore = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                            _CurrentByte += 4;
                            _Object.Monster.Sulfur = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                            _CurrentByte += 4;
                            _Object.Monster.Crystal = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                            _CurrentByte += 4;
                            _Object.Monster.Gem = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                            _CurrentByte += 4;
                            _Object.Monster.Gold = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                            _CurrentByte += 4;
                            _Object.Monster.ArtifactID = BitConverter.ToInt16(a_Bytes, _CurrentByte);
                            _CurrentByte += 2;
                        }

                        _Object.Monster.NeverRun = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                        _CurrentByte += 1;
                        _Object.Monster.DontGrow = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                        _CurrentByte += 1;

                        _CurrentByte += 2; // Unknown Bytes

                        break;

                    case ScenarioObjectType.Resource:
                        _HasMessage = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                        _CurrentByte += 1;

                        if (_HasMessage)
                        {
                            _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                            _CurrentByte += 4;
                            _CurrentByte += _StringLength;

                            bool _HasGuards = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                            _CurrentByte += 1;

                            if (_HasGuards)
                            {
                                _CurrentByte += 28;
                            }
                        }

                        _CurrentByte += 4;
                        _CurrentByte += 4; // Unknown Bytes

                        break;

                    case ScenarioObjectType.Seer:
                        byte _Quest = a_Bytes[_CurrentByte];
                        _CurrentByte += 1;

                        switch (_Quest)
                        {
                            case 1:
                                _CurrentByte += 4;
                                break;
                            
                            case 2:
                                _CurrentByte += 4;
                                break;

                            case 3:
                                _CurrentByte += 4;
                                break;

                            case 4:
                                _CurrentByte += 4;
                                break;

                            case 5:
                                byte _ArtifactCount = a_Bytes[_CurrentByte];
                                _CurrentByte += 1;
                                _CurrentByte += _ArtifactCount * 2;
                                break;

                            case 6:
                                byte _MonsterCount = a_Bytes[_CurrentByte];
                                _MonsterCount += 1;
                                _CurrentByte += _MonsterCount * 4;
                                break;

                            case 7:
                                _CurrentByte += 28;
                                break;

                            case 8:
                                _CurrentByte += 1;
                                break;

                            case 9:
                                _CurrentByte += 1;
                                break;
                        }

                        _CurrentByte += 4; // Time limit

                        _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                        _CurrentByte += 4;
                        _CurrentByte += _StringLength;

                        _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                        _CurrentByte += 4;
                        _CurrentByte += _StringLength;

                        _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                        _CurrentByte += 4;
                        _CurrentByte += _StringLength;

                        byte _Reward = a_Bytes[_CurrentByte];
                        _CurrentByte += 1;

                        switch (_Reward)
                        {
                            case 1:
                                _CurrentByte += 4;
                                break;

                            case 2:
                                _CurrentByte += 4;
                                break;

                            case 3:
                                _CurrentByte += 1;
                                break;

                            case 4:
                                _CurrentByte += 1;
                                break;

                            case 5:
                                _CurrentByte += 5;
                                break;

                            case 6:
                                _CurrentByte += 2;
                                break;

                            case 7:
                                _CurrentByte += 2;
                                break;

                            case 8:
                                _CurrentByte += 2;
                                break;

                            case 9:
                                _CurrentByte += 1;
                                break;

                            case 10:
                                _CurrentByte += 4;
                                break;
                        }

                        _CurrentByte += 2; // Unknown Bytes

                        break;

                    case ScenarioObjectType.Shrine:
                        _CurrentByte += 4; // Spell, TODO
                        break;

                    case ScenarioObjectType.Sign:
                        _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                        _CurrentByte += 4;
                        _CurrentByte += _StringLength;
                        _CurrentByte += 4; // Unknown Bytes
                        break;

                    case ScenarioObjectType.Spell:
                        _HasMessage = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                        _CurrentByte += 1;

                        if (_HasMessage)
                        {
                            _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                            _CurrentByte += 4;
                            _CurrentByte += _StringLength;

                            bool _HasGuards = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                            _CurrentByte += 1;

                            if (_HasGuards)
                            {
                                _CurrentByte += 28;
                            }
                        }

                        _CurrentByte += 4;
                        break;

                    case ScenarioObjectType.Town:
                        _Object.Town = new ScenarioObjectTown();

                        _CurrentByte += 4; // Unknown Bytes

                        _Object.Town.Owner = a_Bytes[_CurrentByte];
                        _CurrentByte += 1;

                        _Object.Town.IsNamed = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                        _CurrentByte += 1;

                        if (_Object.Town.IsNamed)
                        {
                            _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                            _CurrentByte += 4;
                            _Object.Town.Name = Encoding.UTF8.GetString(a_Bytes, _CurrentByte, _StringLength);
                            _CurrentByte += _StringLength;
                        }

                        _Object.Town.IsGuarded = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                        _CurrentByte += 1;

                        if (_Object.Town.IsGuarded)
                        {
                            _Object.Town.Guards = new List<MonsterStack>();

                            for (int j = 0; j < 7; j++)
                            {
                                MonsterStack _Stack = new MonsterStack();

                                _Stack.ID = BitConverter.ToUInt16(a_Bytes, _CurrentByte);
                                _CurrentByte += 2;

                                _Stack.Amount = BitConverter.ToUInt16(a_Bytes, _CurrentByte);
                                _CurrentByte += 2;

                                _Object.Town.Guards.Add(_Stack);
                            }
                        }

                        _Object.Town.IsGroupFormation = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                        _CurrentByte += 1;

                        _Object.Town.HasCustomBuildings = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                        _CurrentByte += 1;

                        if (_Object.Town.HasCustomBuildings)
                        {
                            _CurrentByte += 12;
                        }
                        else
                        {
                            _Object.Town.HasFort = BitConverter.ToBoolean(a_Bytes, _CurrentByte);
                            _CurrentByte += 1;
                        }

                        _CurrentByte += 18;

                        uint _EventQuantity = BitConverter.ToUInt32(a_Bytes, _CurrentByte);
                        _CurrentByte += 4;

                        _CurrentByte += 4; // Unknown Bytes

                        break;

                    case ScenarioObjectType.WitchsHut:
                        _CurrentByte += 4;
                        break;

                    case ScenarioObjectType.QuestionGuard:
                        _Quest = a_Bytes[_CurrentByte];
                        _CurrentByte += 1;

                        switch (_Quest)
                        {
                            case 1:
                                _CurrentByte += 4;
                                break;
                            
                            case 2:
                                _CurrentByte += 4;
                                break;

                            case 3:
                                _CurrentByte += 4;
                                break;

                            case 4:
                                _CurrentByte += 4;
                                break;

                            case 5:
                                byte _ArtifactCount = a_Bytes[_CurrentByte];
                                _CurrentByte += 1;
                                _CurrentByte += _ArtifactCount * 2;
                                break;

                            case 6:
                                byte _MonsterCount = a_Bytes[_CurrentByte];
                                _MonsterCount += 1;
                                _CurrentByte += _MonsterCount * 4;
                                break;

                            case 7:
                                _CurrentByte += 28;
                                break;

                            case 8:
                                _CurrentByte += 1;
                                break;

                            case 9:
                                _CurrentByte += 1;
                                break;
                        }

                        _CurrentByte += 4; // Time limit

                        _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                        _CurrentByte += 4;
                        _CurrentByte += _StringLength;

                        _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                        _CurrentByte += 4;
                        _CurrentByte += _StringLength;

                        _StringLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                        _CurrentByte += 4;
                        _CurrentByte += _StringLength;
                        break;

                    case ScenarioObjectType.GeneralDwelling:
                        _CurrentByte += 4; // Owner
                        int _Unknown = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                        _CurrentByte += 4;

                        if (_Unknown == 0)
                        {
                            _CurrentByte += 2; // Towns
                        }

                        _CurrentByte += 2;

                        break;

                    case ScenarioObjectType.LevelDwelling:
                        _CurrentByte += 4; // Owner
                        _Unknown = BitConverter.ToInt32(a_Bytes, _CurrentByte);
                        _CurrentByte += 4;

                        if (_Unknown == 0)
                        {
                            _CurrentByte += 2; // Towns
                        }

                        break;
                    
                    case ScenarioObjectType.TownDwelling:
                        _CurrentByte += 4; // Owner
                        _CurrentByte += 2;
                        break;
                    
                    case ScenarioObjectType.AbandonedMine:
                        _CurrentByte += 4;
                        break;
                }

                a_Scenario.Objects.Add(_Object);
            }

            // <><><><><> SORTING OBJECTS

            // Sort order priority is complicated
            // Some objects are oddly shaped and will go above their Y pos in certain columns
            // At those points, an object with the same base Y pos, but displaying on their source Y pos should display in front
            // This is determined by looking at the colliders of the objects
            // However, because this is a collision check, and not a convenient numerical check, it makes sorting these objects very complex
            // If an object is rendered above a bunch of objects, but is determined to be rendered below one specific object,
            // the system then needs to shift all of the other objects this object is above, below the object this object is below

            // The system implemented here first creates a list of objects, along with references to all the objects that need to be rendered below each object
            // Then, the objects are sorted in an insertion style sort, where when an object is placed, all of its referenced objects are then placed below it
            // If one of its referenced objects is already placed, and is rendering above the object, it will recursively put all of those objects below this object

            // For the sorting, to prevent always being object count squared operations, the list is sorted by Y value,
            // and each set of collision checks is only done among the elements of the list in that Y range

            // KNOWN ISSUES
            // Some objects don't adhere to the regular sort order behaviour, and simply use the file order
            // Need to figure out if this is determined by any of the values stored in objects
            // There are cases of circular referencing, where 3 sprites think they're on top of one and below the other, causing infinite loops
            // The real game seems to split sprites up in this case, so part of the sprite displays on top, and part doesn't

            // This function is called once for above ground, and once for underground if present
            void SortObjects(List<ScenarioObject> a_Objects)
            {
                List<ScenarioObjectSortData> _ObjectSortData = new List<ScenarioObjectSortData>();

                for (int i = 0; i < a_Objects.Count; i++)
                {
                    if (!a_Objects[i].Template.IsLowPrioritySortOrder)
                    {
                        _ObjectSortData.Add(new ScenarioObjectSortData(a_Objects[i], i));
                    }
                }

                _ObjectSortData = _ObjectSortData.OrderBy((a_Object) => a_Object.Object.YPos).ToList();

                if (_ObjectSortData.Count > 0)
                {
                    int _HigherBound = 0;
                    int _ListCount = _ObjectSortData.Count;

                    bool _ReachedEnd = false;

                    while (!_ReachedEnd)
                    {
                        int _LowerBound = _HigherBound;
                        int _CurrentY = _ObjectSortData[_LowerBound].Object.YPos;

                        // Determine the range of objects that share the same Y object
                        while (_ObjectSortData[_HigherBound].Object.YPos == _CurrentY)
                        {
                            _HigherBound++;

                            if (_ListCount == _HigherBound)
                            {
                                _ReachedEnd = true;
                                break;
                            }
                        }

                        bool CheckIsAbove(ScenarioObjectSortData a_Object, ScenarioObjectSortData a_OtherObject)
                        {
                            if (a_Object == a_OtherObject)
                            {
                                return false;
                            }

                            int _XDifference = Mathf.Abs(a_Object.Object.XPos - a_OtherObject.Object.XPos);

                            if (_XDifference > 7)
                            {
                                return false;
                            }

                            bool _ObjectsShareColumns = false;

                            bool _ObjectIsLeftmost = true;

                            ScenarioObject _LeftmostObject = a_Object.Object;
                            ScenarioObject _RightmostObject = a_OtherObject.Object;

                            if (a_Object.Object.XPos > a_OtherObject.Object.XPos)
                            {
                                _ObjectIsLeftmost = false;
                                _LeftmostObject = a_OtherObject.Object;
                                _RightmostObject = a_Object.Object;
                            }

                            for (int x = _XDifference; x < 8; x++)
                            {
                                byte _LeftmostBitwiseIndex;
                                byte _RightmostBitwiseIndex;

                                if (x + _XDifference == 0)
                                {
                                    _LeftmostBitwiseIndex = 1;
                                }
                                else
                                {
                                    _LeftmostBitwiseIndex = (byte)Mathf.Pow(2, x + _XDifference);
                                }

                                if (x == 0)
                                {
                                    _RightmostBitwiseIndex = 1;
                                }
                                else
                                {
                                    _RightmostBitwiseIndex = (byte)Mathf.Pow(2, x);
                                }

                                int _LeftmostCollisionIndex = 0;
                                int _RightmostCollisionIndex = 0;

                                for (int y = 5; y > 2; y--)
                                {
                                    if (_LeftmostCollisionIndex == 0)
                                    {
                                        if (!((_LeftmostObject.Template.Passability[y] & _LeftmostBitwiseIndex) == _LeftmostBitwiseIndex))
                                        {
                                            _LeftmostCollisionIndex = y;
                                        }
                                    }

                                    if (_RightmostCollisionIndex == 0)
                                    {
                                        if (!((_RightmostObject.Template.Passability[y] & _RightmostBitwiseIndex) == _RightmostBitwiseIndex))
                                        {
                                            _RightmostCollisionIndex = y;
                                        }
                                    }
                                }

                                if (_LeftmostCollisionIndex != 0 &&
                                    _RightmostCollisionIndex != 0)
                                {
                                    _ObjectsShareColumns = true;
                                    if (_LeftmostCollisionIndex > _RightmostCollisionIndex)
                                    {
                                        if (_ObjectIsLeftmost)
                                        {
                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else if (_LeftmostCollisionIndex < _RightmostCollisionIndex)
                                    {
                                        if (_ObjectIsLeftmost)
                                        {
                                            return false;
                                        }
                                        else
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }

                            if (!_ObjectsShareColumns)
                            {
                                return false;
                            }

                            if (a_Object.FileOrder > a_OtherObject.FileOrder)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }

                        // Determine children
                        for (int i = _LowerBound; i < _HigherBound; i++)
                        {
                            ScenarioObjectSortData _Object = _ObjectSortData[i];

                            for (int j = _LowerBound; j < _HigherBound; j++)
                            {
                                if (CheckIsAbove(_Object, _ObjectSortData[j]))
                                {
                                    _Object.Children.Add(_ObjectSortData[j]);
                                }
                            }
                        }
                    }
                }

                // Sort based on children
                void MoveChildrenBelow(ScenarioObjectSortData a_Object, int a_Recursion)
                {
                    if (a_Recursion > 4)
                    {
                        return;
                    }

                    for (int i = 0; i < a_Object.Children.Count; i++)
                    {
                        int _ObjectIndex = _ObjectSortData.IndexOf(a_Object);
                        int _ChildIndex = _ObjectSortData.IndexOf(a_Object.Children[i]);
                        if (_ChildIndex > _ObjectIndex)
                        {
                            _ObjectSortData.RemoveAt(_ChildIndex);
                            _ObjectSortData.Insert(_ObjectIndex, a_Object.Children[i]);
                            MoveChildrenBelow(a_Object.Children[i], a_Recursion + 1);
                        }
                    }
                }

                for (int i = 0; i < _ObjectSortData.Count; i++)
                {
                    MoveChildrenBelow(_ObjectSortData[i], 0);
                }

                List<(ScenarioObject Object, int FileOrder)> _LowPriorityObjects = new List<(ScenarioObject, int)>();

                for (int i = 0; i < a_Objects.Count; i++)
                {
                    if (a_Objects[i].Template.IsLowPrioritySortOrder)
                    {
                        _LowPriorityObjects.Add((a_Objects[i], i));
                    }
                }

                _LowPriorityObjects = _LowPriorityObjects.OrderBy((a_Object) => a_Object.FileOrder).ToList();

                // Apply sort order
                for (int i = 0; i < _LowPriorityObjects.Count; i++)
                {
                    _LowPriorityObjects[i].Object.SortOrder = i;
                }

                for (int i = 0; i < _ObjectSortData.Count; i++)
                {
                    _ObjectSortData[i].Object.SortOrder = i;
                }
            }

            SortObjects(a_Scenario.Objects.Where((a_Object) => !a_Object.IsUnderground).ToList());

            if (a_Scenario.HasUnderground)
            {
                SortObjects(a_Scenario.Objects.Where((a_Object) => a_Object.IsUnderground).ToList());
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Conversion of SoD map '{a_Scenario.Name}' failed\n{e}");
        }
    }
}
