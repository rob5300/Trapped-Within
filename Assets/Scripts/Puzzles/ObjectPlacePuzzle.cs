using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entity;
using UnityEngine;

public class ObjectPlacePuzzle : MonoBehaviour
{
    public Candle[] Candles;
    public ItemPlaceZone[] PlaceZones;
    public Box WinBox;
    public bool Won = false;

    private bool[] _correctZones;

    public void Awake()
    {
        _correctZones = new bool[PlaceZones.Length];
        //for(int i=0;i<PlaceZones.Length; i++)
        //{
        //    _correctZones[i] = false;
        //}
    }

    public void Start()
    {
        foreach (Candle candle in Candles)
        {
            candle.TurnOff();
        }
    }

    public void OnItemPlace(ItemPlaceZone zone, MoveableEntity entity)
    {
        if (Won) return;
        PlacePuzzleObject puzzleObject = entity as PlacePuzzleObject;
        int? zoneId = GetZoneId(zone);
        if (puzzleObject && zoneId != null && zoneId.Value == puzzleObject.ID)
        {
            //This is in the correct position.
            _correctZones[zoneId.Value] = true;
            Candles[zoneId.Value].TurnOn();
        }
        WinCheck();
    }

    public void OnItemRemove(ItemPlaceZone zone, MoveableEntity entity)
    {
        if (Won) return;
        int? zoneId = GetZoneId(zone);
        if (zoneId != null)
        {
            _correctZones[zoneId.Value] = false;
            Candles[zoneId.Value].TurnOff();
        }
    }

    public int? GetZoneId(ItemPlaceZone zone)
    {
        for (int i = 0; i < PlaceZones.Length; i++)
        {
            if (PlaceZones[i] == zone) return i;
        }
        return null;
    }

    public void WinCheck()
    {
        if (_correctZones.Count(x => x == true) == PlaceZones.Length)
        {
            Won = true;
            WinBox.Open();
        }
    }
}
