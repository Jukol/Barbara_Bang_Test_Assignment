using System;
using System.Threading.Tasks;
using UnityEngine;

namespace View
{
    public class TurnSystem : MonoBehaviour
    {
        [SerializeField] private BattleSystem battleSystem;
        [SerializeField] private GameObject popup;
        [SerializeField] private GameObject turnInfo;

        private void Awake()
        {
            turnInfo.SetActive(true);
        }


        public async void OnTurnButton()
        {
            turnInfo.SetActive(false);
            
            if (battleSystem.NextTurn)
            {
                battleSystem.FriendsTurn();
                
                await Task.Delay(3000);
                
                battleSystem.EnemiesTurn(() => turnInfo.SetActive(true));
            }
            
            else
                ShowPopup();
        }
        
        private void ShowPopup() => 
            popup.SetActive(true);
    }
}
