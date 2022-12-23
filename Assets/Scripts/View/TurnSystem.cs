using System.Threading.Tasks;
using UnityEngine;

namespace View
{
    public class TurnSystem : MonoBehaviour
    {
        [SerializeField] private BattleSystem battleSystem;
        [SerializeField] private GameObject popup;
        [SerializeField] private GameObject turnInfo;
        [SerializeField] private Popup winPopup;
        [SerializeField] private Popup losePopup;

        private bool _gameOver;
        private bool _turnButtonPressed;

        private void Awake()
        {
            battleSystem.OnWin += OnWin;
            battleSystem.OnLose += OnLose;
            turnInfo.SetActive(true);
        }
        
        private void OnWin() => 
            winPopup.gameObject.SetActive(true);

        private void OnLose() => 
            losePopup.gameObject.SetActive(true);

        public async void OnTurnButton()
        {
            if (!_turnButtonPressed)
            {
                _turnButtonPressed = true;
                turnInfo.SetActive(false);
            
                if (battleSystem.NextTurn)
                {
                    if (_gameOver) return;
                    battleSystem.FriendAction();
                
                    await Task.Delay(3000);

                    if (_gameOver) return;
                    battleSystem.EnemyAction(() => turnInfo.SetActive(true));
                                    
                    _turnButtonPressed = false;
                }
            
                else
                    ShowPopup();
            }
        }
        
        private void ShowPopup()
        {
            _gameOver = true;
            turnInfo.SetActive(false);
            popup.SetActive(true);
        }
    }
}
