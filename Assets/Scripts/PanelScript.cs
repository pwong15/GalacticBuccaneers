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
            Debug.Log("Move Button Clicked");
            if (!unit.HasMoved) {
                EncounterUtils.Highlight(grid.SelectedPieceMoveRange, Color.blue);
                grid.SelectedPieceState = SelectedPieceState.Moving;
            }
            HidePanel();
        }
        public void Attack() {
            Debug.Log("Attck Button Clicked");
            if (!unit.HasActed) {
                EncounterUtils.Highlight(grid.SelectedPieceAttackRange, Color.red);
                grid.SelectedPieceState = SelectedPieceState.Attacking;
            }
            HidePanel();
        }
    }
}
