using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Views;

namespace Utilitys {
    public class PanelScript : MonoBehaviour {
        public Views.Grid grid;
        public Unit unit;
        public GameObject panel;
        public bool status;
        public void DisplayPanel() {
            panel.gameObject.SetActive(true);
            status = true;
        }
        public void HidePanel() {
            panel.gameObject.SetActive(false);
            status = false;
        }
        public bool PanelStatus() {
            return status;
        }

        public void Move() {
            
            if (!unit.HasMoved) {
                Debug.Log("Move Button Clicked");
                EncounterUtils.Highlight(grid.SelectedPieceMoveRange, Color.blue);
                grid.SelectedPieceState = SelectedPieceState.Moving;
            }
            HidePanel();
        }
        public void Attack() {
            
            if (!unit.HasActed) {
                Debug.Log("Attck Button Clicked");
                EncounterUtils.Highlight(grid.SelectedPieceAttackRange, Color.red);
                grid.SelectedPieceState = SelectedPieceState.Attacking;
            }
            HidePanel();
        }
    }
}
