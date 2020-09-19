using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Text;
using System;

public class H3MReader : MonoBehaviour
{
    [Tooltip("Path starts from within the assets folder")]
    [SerializeField] string m_InputFolder;
    [Tooltip("Path starts from within the assets folder")]
    [SerializeField] string m_OutputFolder;
    [SerializeField] bool m_Overwrite;

#if UNITY_EDITOR
    void Awake()
    {
        object[] _ExistingObjects = AssetDatabase.LoadAllAssetsAtPath(m_OutputFolder);

        List<Map> _ExistingMaps = _ExistingObjects.Cast<Map>().ToList();

        List<string> _InputFiles = new List<string>(Directory.GetFiles(Application.dataPath + "/" + m_InputFolder));

        _InputFiles = _InputFiles.Where((a_File) => a_File.EndsWith(".h3m")).ToList();

        if (!m_Overwrite)
        {
            for (int i = _InputFiles.Count - 1; i >= 0; i--)
            {
                int _StartIndex = _InputFiles[i].LastIndexOf('\\');
                int _Length = _InputFiles[i].Length - 4 - _StartIndex;
                string _Name = _InputFiles[i].Substring(_StartIndex, _Length);

                for (int j = 0; j < _ExistingMaps.Count; j++)
                {
                    if (_ExistingMaps[j].name == _Name)
                    {
                        _InputFiles.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < _InputFiles.Count; i++)
        {
            Map _Map = new Map();

            int _StartIndex = _InputFiles[i].LastIndexOf('\\');
            int _Length = _InputFiles[i].Length - 4 - _StartIndex;
            _Map.name = _InputFiles[i].Substring(_StartIndex, _Length);

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

            // <><><><><> Basic Map Data

            byte[] _Bytes = _BytesList.ToArray();

            _Map.Version = BitConverter.ToInt32(_Bytes, 0);

            switch (_Map.Version)
            {
                case Map.RESTORATION_OF_ERATHIA: ReadRoE(ref _Map, _Bytes); break;
                case Map.ARMAGEDDONS_BLADE: ReadAB(ref _Map, _Bytes); break;
                case Map.SHADOW_OF_DEATH: ReadSoD(ref _Map, _Bytes); break;
            }

            int _ComputerCount = 0;
            int _PlayerCount = 0;

            for (int j = 0; j < _Map.PlayerInfo.Count; j++)
            {
                if (_Map.PlayerInfo[j].ComputerPlayable)
                {
                    _ComputerCount++;
                }

                if (_Map.PlayerInfo[j].HumanPlayable)
                {
                    _PlayerCount++;
                }
            }

            _Map.PlayerCount = _PlayerCount;
            _Map.ComputerCount = _ComputerCount;
            
            AssetDatabase.CreateAsset(_Map, "Assets/" + m_OutputFolder + _Map.name + ".asset");
        }
    }

    void ReadRoE(ref Map a_Map, Byte[] a_Bytes)
    {
        try
        {
            int _CurrentByte = 0;

            _CurrentByte += 4;

            _CurrentByte += 1; // Skip unknown byte

            a_Map.Size = BitConverter.ToInt32(a_Bytes, _CurrentByte);

            _CurrentByte += 4;

            a_Map.HasUnderground = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

            _CurrentByte += 1;

            // Set Name and Description
            int _NameLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);

            _CurrentByte += 4;

            a_Map.Name = Encoding.UTF8.GetString(a_Bytes, 14, _NameLength);

            _CurrentByte += _NameLength;

            int _DescLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);

            _CurrentByte += 4;

            a_Map.Description = Encoding.UTF8.GetString(a_Bytes, _CurrentByte, _DescLength);

            _CurrentByte += _DescLength;

            a_Map.Difficulty = a_Bytes[_CurrentByte];

            _CurrentByte += 1;

            // <><><><><> Player Specs

            a_Map.PlayerInfo = new List<PlayerInfo>(8);

            for (int j = 0; j < 8; j++)
            {
                PlayerInfo _PlayerInfo = new PlayerInfo();
                a_Map.PlayerInfo.Add(_PlayerInfo);

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
        }
        catch (Exception e)
        {
            Debug.LogError($"Conversion of RoE map '{a_Map.Name}' failed\n{e}");
        }
    }

    void ReadAB(ref Map a_Map, Byte[] a_Bytes)
    {
        try
        {
            int _CurrentByte = 0;

            _CurrentByte += 4;

            _CurrentByte += 1; // Skip unknown byte

            a_Map.Size = BitConverter.ToInt32(a_Bytes, _CurrentByte);

            _CurrentByte += 4;

            a_Map.HasUnderground = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

            _CurrentByte += 1;

            // Set Name and Description
            int _NameLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);

            _CurrentByte += 4;

            a_Map.Name = Encoding.UTF8.GetString(a_Bytes, 14, _NameLength);

            _CurrentByte += _NameLength;

            int _DescLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);

            _CurrentByte += 4;

            a_Map.Description = Encoding.UTF8.GetString(a_Bytes, _CurrentByte, _DescLength);

            _CurrentByte += _DescLength;

            a_Map.Difficulty = a_Bytes[_CurrentByte];

            _CurrentByte += 1;

            _CurrentByte += 1; // Skipping level limit

            // <><><><><> Player Specs

            a_Map.PlayerInfo = new List<PlayerInfo>(8);

            for (int j = 0; j < 8; j++)
            {
                PlayerInfo _PlayerInfo = new PlayerInfo();
                a_Map.PlayerInfo.Add(_PlayerInfo);

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
        }
        catch (Exception e)
        {
            Debug.LogError($"Conversion of AB map '{a_Map.Name}' failed\n{e}");
        }
    }

    void ReadSoD(ref Map a_Map, Byte[] a_Bytes)
    {
        try
        {
            int _CurrentByte = 0;

            _CurrentByte += 4;

            _CurrentByte += 1; // Skip unknown byte

            a_Map.Size = BitConverter.ToInt32(a_Bytes, _CurrentByte);

            _CurrentByte += 4;

            a_Map.HasUnderground = BitConverter.ToBoolean(a_Bytes, _CurrentByte);

            _CurrentByte += 1;

            // Set Name and Description
            int _NameLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);

            _CurrentByte += 4;

            a_Map.Name = Encoding.UTF8.GetString(a_Bytes, 14, _NameLength);

            _CurrentByte += _NameLength;

            int _DescLength = BitConverter.ToInt32(a_Bytes, _CurrentByte);

            _CurrentByte += 4;

            a_Map.Description = Encoding.UTF8.GetString(a_Bytes, _CurrentByte, _DescLength);

            _CurrentByte += _DescLength;

            a_Map.Difficulty = a_Bytes[_CurrentByte];

            _CurrentByte += 1;

            _CurrentByte += 1; // Skipping level limit

            // <><><><><> Player Specs

            a_Map.PlayerInfo = new List<PlayerInfo>(8);

            for (int j = 0; j < 8; j++)
            {
                PlayerInfo _PlayerInfo = new PlayerInfo();
                a_Map.PlayerInfo.Add(_PlayerInfo);

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
        }
        catch (Exception e)
        {
            Debug.LogError($"Conversion of SoD map '{a_Map.Name}' failed\n{e}");
        }
    }
#endif
}
