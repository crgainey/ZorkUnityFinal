using UnityEngine;
using Zork;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

public class UnityOutputService : MonoBehaviour, IOutputService
{
    [SerializeField]
    int MaxEntries = 60;
    
    [SerializeField]
    Transform OutputTextContainer;
    
    [SerializeField]
    TextMeshProUGUI TextLinePrefab;

    [SerializeField]
    Image NewLinePrefab;


    public UnityOutputService() => _entries = new List<GameObject>();

    public void Clear() => _entries.ForEach(entry => Destroy(entry));

    public void Write(object value) => Write(value.ToString());

    public void WriteLine(object value) => WriteLine(value.ToString());

    public void Write(string value) => ParseAndWriteLine(value);

    public void WriteLine(string value) =>ParseAndWriteLine(value);
    

    void ParseAndWriteLine(string value)
    {
        string[] delimiters = { "\n" };

        var lines = value.Split(delimiters, StringSplitOptions.None);
        foreach(var line in lines)
        {
            if(_entries.Count >= MaxEntries)
            {
                var entry = _entries.First();
                Destroy(entry);
                _entries.Remove(entry);
            }

            if (string.IsNullOrWhiteSpace(line))
            {
                WriteNewLine();
            }
            else
            {
                WriteTextLine(line);
            }
        }
    }

    void WriteNewLine()
    {
        var newLine = GameObject.Instantiate(NewLinePrefab);
        newLine.transform.SetParent(OutputTextContainer, false);
        _entries.Add(newLine.gameObject);
    }

    void WriteTextLine(string value)
    {
        var textLine = GameObject.Instantiate(TextLinePrefab);
        textLine.transform.SetParent(OutputTextContainer, false);
        textLine.text = value;
        _entries.Add(textLine.gameObject);
    }

    readonly List<GameObject> _entries;

}
