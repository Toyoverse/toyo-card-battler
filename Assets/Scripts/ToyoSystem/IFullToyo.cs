using System.Collections.Generic;
using Scriptable_Objects;

namespace ToyoSystem
{
    public interface IFullToyo
    {
        Dictionary<TOYO_PIECE, IToyoPart> ToyoParts { get; set; }

        Dictionary<TOYO_STAT, float> ToyoStats { get; set; }

        ListToyoIDData ToyoIDs { get; }
    }
}