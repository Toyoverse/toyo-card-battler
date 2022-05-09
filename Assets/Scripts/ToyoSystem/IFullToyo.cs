using System.Collections.Generic;
using Card.CardPile;

namespace ToyoSystem
{
    public interface IFullToyo
    {
        Dictionary<TOYO_PIECE, IToyoPart> ToyoParts { get; set; }

        Dictionary<TOYO_STAT, float> ToyoStats { get; }
        
        public List<EffectData> Buffs { get; set; }

        ListToyoIDData ToyoIDs { get; }
        
        void InitializeToyo(ICardPile handler);

        Dictionary<string, int> CountEachPartToyo();
    }
}