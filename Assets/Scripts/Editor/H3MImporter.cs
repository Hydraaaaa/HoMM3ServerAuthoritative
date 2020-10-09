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

                // 6 bytes - Passability?
                // 6 bytes - Actions?
                // 2 bytes - Landscape?
                // 2 bytes - LandEditGroups?
                _CurrentByte += 16;

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
                        _Object.Type = ScenarioObjectType.Scientist;
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
                // 1 byte - Is Overlay?
                // 16 bytes - Unknown
                _CurrentByte += 18;

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

                Debug.Log($"Object Base: {_Object.Template.Name} - {_ObjectBaseIndex} - {_Object.Template.Type}/{_Object.Template.TypeDebug} - {_CurrentByte.ToString("X")} - {i}/{_ObjectDataCount}");

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
                }

                a_Scenario.Objects.Add(_Object);
            }

            List<(ScenarioObject, int)> _Objects = new List<(ScenarioObject, int)>();

            for (int i = 0; i < a_Scenario.Objects.Count; i++)
            {
                _Objects.Add((a_Scenario.Objects[i], i + a_Scenario.Objects[i].YPos * a_Scenario.Objects.Count));
            }

            _Objects = _Objects.OrderBy((a_Pair) => a_Pair.Item2).ToList();

            for (int i = 0; i < _Objects.Count; i++)
            {
                _Objects[i].Item1.SortOrder = i;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Conversion of SoD map '{a_Scenario.Name}' failed\n{e}");
        }
    }
}
