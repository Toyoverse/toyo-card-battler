using System;

namespace Card
{
    public class CardUtils
    {
        public static bool ValidateCard(ICard card)
        {
            if (card == null) throw new ArgumentNullException("Card is null");
            return true;
        }
    }
}