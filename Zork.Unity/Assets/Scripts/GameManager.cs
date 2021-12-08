using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using Zork;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI CurrentLocationText;

    [SerializeField]
    private TextMeshProUGUI MovesText;

    [SerializeField]
    private TextMeshProUGUI ScoreText;

    [SerializeField]
    private TextMeshProUGUI ItemsText;

    [SerializeField]
    private UnityInputService InputService;

    [SerializeField]
    private UnityOutputService OutputService;

    private Game _game;

    void Awake()
    {
        TextAsset gameTextAsset = Resources.Load<TextAsset>("Zork");
        _game = JsonConvert.DeserializeObject<Game>(gameTextAsset.text);
        _game.Player.LocationChanged += (sender, newLocation) => CurrentLocationText.text = newLocation.ToString();
        _game.Player.MovesChanged += (sender, moves) => MovesText.text = ($"Moves: {moves}");
        _game.Player.ScoreChanged += (sender, score) => ScoreText.text = ($"Score: {score}");
        _game.Player.InventoryChanged += (sender, items) => ItemsText.text = items.ToString();

        _game.Start(InputService, OutputService);

        CurrentLocationText.text = _game.Player.Location.ToString();
        
    }

    private void Update()
    {
        DisplayInventory();

        if (_game.IsRunning == false)
        {
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    void DisplayInventory()
    {
        foreach(Item items in _game.Player.Inventory)
        {
            ItemsText.text = items.Name.ToString();
        }
    }

}
