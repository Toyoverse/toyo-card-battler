using System;
using System.Collections.Generic;
using System.Linq;
using Card.CardPile;
using Card.DeckSystem;
using UnityEngine;

namespace ToyoSystem
{
    public class FullToyo : MonoBehaviour, IFullToyo
    {
                
        public const bool IsDebug = true;
        
        public Dictionary<TOYO_PIECE, IToyoPart> ToyoParts { get; set; }
        public Dictionary<TOYO_STAT, float> ToyoStats { get; set; }
        public Dictionary<TOYO_STAT, float> ToyoBonusStats { get; set; }
        public ListToyoIDData ToyoIDs { get; set; }


        private void Awake()
        {
            ToyoParts = new Dictionary<TOYO_PIECE, IToyoPart>();
            ToyoStats = new Dictionary<TOYO_STAT, float>();
            ToyoBonusStats = new Dictionary<TOYO_STAT, float>();

        }
        
        public void InitializeToyo(ICardPile handler)
        {
            if (IsDebug)
                GetDebugToyo(handler);
            else
                GetToyoFromWeb(handler); 
        }

        void GetDebugToyo(ICardPile handler)
        {
            FullToyoSO _fullToyoSo = Resources.Load<FullToyoSO>("FullToyoData");
            CreateToyoObject(_fullToyoSo, handler);
        }
        
        void GetToyoFromWeb(ICardPile handler)
        {
            //FullToyoSO _fullToyoSo = Resources.Load<FullToyoSO>("/FullToyo");
            //CreateToyoObject(_fullToyoSo);
        }

        void CreateToyoObject(FullToyoSO fullToyo, ICardPile handler)
        {
            var _toyoParts = fullToyo.ToyoParts;
            foreach (var _part in _toyoParts)
            {
                ToyoParts.Add(_part.ToyoPiece, new ToyoPart(_part, handler));
                CalculateBaseStats(_part);
                CalculateBonusStats(_part);
            }
            ApplyBonusMultiplierStats();
        }
        
        void CalculateBonusStats(ToyoPartSO part)
        {
            foreach (var _stat in part.ToyoBonusStat)
            {
                if (ToyoBonusStats.ContainsKey(_stat.ToyoStat))
                    ToyoBonusStats[_stat.ToyoStat] += _stat.StatValue;
                else
                    ToyoBonusStats.Add(_stat.ToyoStat, _stat.StatValue);
            }      
        }
        
        void CalculateBaseStats(ToyoPartSO part)
        {
            foreach (var _stat in part.ToyoPartStat)
            {
                if (ToyoStats.ContainsKey(_stat.ToyoStat))
                    ToyoStats[_stat.ToyoStat] += _stat.StatValue;
                else
                    ToyoStats.Add(_stat.ToyoStat, _stat.StatValue);
            }   
        }
        
        void ApplyBonusMultiplierStats()
        {
            foreach (var (_key, _value) in ToyoStats.Where(_stat => ToyoBonusStats.ContainsKey(_stat.Key)))
                ToyoStats[_key] *= ToyoBonusStats[_key];
        }
        
        public Dictionary<string, int> CountEachPartToyo()
        {
            Dictionary<string, int> CountEachPart = new Dictionary<string, int>();

            var _listToyoData = ToyoIDs.ListToyoData;
            
            foreach (var _toyoIDData in _listToyoData)
            {
                if (CountEachPart.ContainsKey(_toyoIDData.Id))
                {
                    var _value = CountEachPart[_toyoIDData.Id];
                    _value++;
                    CountEachPart[_toyoIDData.Id] = _value;
                }
                else
                {
                    CountEachPart.Add(_toyoIDData.Id, 1);
                }
            }

            return CountEachPart;
        }
    }
}