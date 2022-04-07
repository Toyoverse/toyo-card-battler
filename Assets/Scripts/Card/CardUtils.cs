using System;

namespace Card
{
    public static class CardUtils
    {
        public static void ValidateCard(this ICard card)
        {
            if (card == null) throw new ArgumentNullException("Card is null");
        }
    }
}