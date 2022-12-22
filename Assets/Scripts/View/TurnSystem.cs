using System.Threading.Tasks;
using UnityEngine;

namespace View
{
    public class TurnSystem : MonoBehaviour
    {
        [SerializeField] private BattleSystem battleSystem;
        [SerializeField] private GameObject popup;
        [SerializeField] private GameObject turnInfo;
        [SerializeField] private GameObject winPopup;
        [SerializeField] private GameObject losePopup;

        private bool _gameOver;

        private void Awake()
        {
            battleSystem.OnWin += OnWin;
            battleSystem.OnLose += OnLose;
            turnInfo.SetActive(true);
        }
        
        private void OnWin() => 
            winPopup.SetActive(true);

        private void OnLose() => 
            losePopup.SetActive(true);

        public async void OnTurnButton()
        {
            turnInfo.SetActive(false);
            
            if (battleSystem.NextTurn)
            {
                if (_gameOver) return;
                battleSystem.FriendsTurn();
                
                await Task.Delay(3000);

                if (_gameOver) return;
                battleSystem.EnemiesTurn(() => turnInfo.SetActive(true));
            }
            
            else
                ShowPopup();
        }
        
        private void ShowPopup()
        {
            _gameOver = true;
            turnInfo.SetActive(false);
            popup.SetActive(true);
        }
    }
}
