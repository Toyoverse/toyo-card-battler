using System.Linq;
using UnityEngine;

namespace Infrastructure
{
    public class DatabaseManager : Singleton<DatabaseManager>
    {
        public FakeDatabase FakeDatabase;

        private int playerTemporaryIndex = 0; 

        public FullToyoSO GetFullToyoFromFakeID(int _fakeId)
        {

            /*
             * Todo Remove
             */
            _fakeId = 0; 
            
            
            var _players = FakeDatabase.PlayerRegistered;

            return _players.First(_player => _fakeId == _player.PlayerDatabaseID).FullToyoData;
        }
        
        //Todo: Update this for infrastructure 
        public int GetPlayerDatabaseID() => playerTemporaryIndex++;
        
    }
}