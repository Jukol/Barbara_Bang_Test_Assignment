using System.Threading.Tasks;
using UnityEngine;

namespace View
{
    public class TurnSystem : MonoBehaviour
    {
        [SerializeField] private BattleSystem battleSystem;
        [SerializeField] private GameObject popup;
        
        public async void OnTurnButton()
        {
            if (battleSystem.NextTurn)
            {
                battleSystem.FriendsTurn();
                
                await Task.Delay(3000);
                
                battleSystem.EnemiesTurn();
            }
            
            else
                ShowPopup();
        }
        
        private void ShowPopup() => 
            popup.SetActive(true);
    }
}
