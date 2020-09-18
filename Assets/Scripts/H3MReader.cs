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

            int _CurrentByte = 10;

            byte[] _Bytes = _BytesList.ToArray();

            // Set Name and Description
            int _NameLength = BitConverter.ToInt32(_Bytes, _CurrentByte);

            _CurrentByte += 4;

            _Map.Name = Encoding.UTF8.GetString(_Bytes, 14, _NameLength);

            _CurrentByte += _NameLength;

            int _DescLength = BitConverter.ToInt32(_Bytes, _CurrentByte);

            _CurrentByte += 4;

            _Map.Description = Encoding.UTF8.GetString(_Bytes, _CurrentByte, _DescLength);

            _CurrentByte += 4;

            AssetDatabase.CreateAsset(_Map, "Assets/" + m_OutputFolder + _Map.name + ".asset");
        }
    }
#endif
}
