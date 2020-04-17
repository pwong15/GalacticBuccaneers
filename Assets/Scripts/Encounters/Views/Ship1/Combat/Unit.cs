using UnityEngine;
using Encounter;
using Models;
using System.Collections.Generic;
using UnityEngine.UI;
using Utilitys;

namespace Views {

    public enum Team {
        Player,
        Enemy
    }

    public class Unit : MonoBehaviour, Effectable {


        private int DamageDealtModifier;
        private int DamageTakenModifier;
        public Character Character { get; set; }
        private Vector3 destination;
        private BarController healthBar;
        public PanelScript actionMenu;
        public Team Team { get; set; }
        public bool moving = false;
        public Tile destinationTile;
        List<Effect> TurnStartEffects;
        List<Effect> TurnEndEffects;
        private void Update()
        {
            if (moving)
            {
                float delta = MoveSpeed * Time.deltaTime;
                Vector3 currentPosition = this.transform.position;
                Vector3 nextPosition = Vector3.MoveTowards(currentPosition, destination, delta);

                this.transform.position = nextPosition;
            }

            if (destination == this.transform.position)
            {
                moving = false;
                Tile = destinationTile;
            }
        }
        private Tile _tile;
        public Tile Tile {
            get {
                return _tile;
            }
            set {
                _tile = value;
                if (_tile != null) {
                    Vector3 tileLocation = value.transform.position;
                    this.transform.position = new Vector3(tileLocation.x, tileLocation.y, tileLocation.z + 1);
                }
            }
        }

        public bool HasMoved { get; set; }

        private bool _hasActed;

        public bool HasDied { get; set; }

        private Vector3 screenPoint;
        private Vector3 offset;
        private Vector3 scanPos;

        void OnMouseHover() {
            Debug.Log("Mouseover");
        }
        void OnMouseDown() {
            if (Tile.gameBoard.TurnCounter == 0) {
                Unit unit = Tile.gameBoard.SelectedDeploymentUnit;
                Debug.Log("Clicked " + this);
                Debug.Log("Deployed Unit is " + unit);
                if (unit != null) {
                    Debug.Log("Swap");
                    EncounterUtils.SwapUnits(this, unit);
                    Tile.gameBoard.SelectedDeploymentUnit = null;
                } else {
                    Tile.gameBoard.SelectedDeploymentUnit = this;
                }
            } else if (!HasActed) {
                GameObject selectedPiece = Tile.gameBoard.selectedPiece;
                if (selectedPiece == null) {
                    actionMenu.DisplayPanel();
                    Tile.gameBoard.selectedPiece = this.gameObject;
                }
            }
        }

       
        public bool HasActed {
            get { return _hasActed; }
            set {
                HasMoved = value;
                _hasActed = value;
            }
        }

        public int MoveSpeed { get; set; }

        public void Initialize(Character character, Tile tile) {
            this.Character = character;
            tile.BoardPiece = this.gameObject;
            this.gameObject.name = this.ToString();
            this.MoveSpeed = character.MoveSpeed;
            this.Tile = tile;
            this.Team = Character.Team;
            HasDied = false;
            _hasActed = true;
            HasMoved = true;
            TurnStartEffects = new List<Effect>();
            TurnEndEffects = new List<Effect>();
            healthBar = gameObject.GetComponentInChildren<BarController>();
            healthBar.SetMaxValue(character.MaxHealth);
            healthBar.SetMinValue(0);
            healthBar.SetValue(character.MaxHealth);
            actionMenu = gameObject.GetComponentInChildren<PanelScript>();
            actionMenu.unit = this;
            actionMenu.grid = Tile.gameBoard;
            if (Team == 0) {
                Image health = gameObject.transform.Find("HealthBarCanvas/healthBar/healthFill").gameObject.GetComponent<Image>();
                health.color = Color.blue;
            }
            actionMenu.HidePanel();
        }

        public void MoveTo(Tile targetLocation) {
            if (!HasMoved && targetLocation.BoardPiece == null) {
                destination = targetLocation.transform.position;
                moving = true;
                Tile.BoardPiece = null;
                targetLocation.BoardPiece = this.gameObject;
                destinationTile = targetLocation;
                HasMoved = true;
                //Tile = targetLocation;
            }
        }

        public void AttackUnit(Unit otherUnit) {
            if (otherUnit.Team != Team) {
                otherUnit.TakeDamage(DamageDealtModifier * (Character.Attack - otherUnit.Character.Defense));
                HasActed = true;
            }
        }

        public void TakeDamage(int damageAmount) {
            Character.Health -= DamageTakenModifier * damageAmount;
            Debug.Log(Character.Name + " took " + damageAmount + " dmg");
            if (Character.Health <= 0) {
                Debug.Log(Character.Name + " has Died");
                Die();
            }
            healthBar.SetValue(Character.Health);

        }

        public void Heal(int healAmount) {
            Character.Health += healAmount;
            if (Character.Health > Character.MaxHealth) {
                Character.Health = Character.MaxHealth;
            }
            healthBar.SetValue(Character.Health);
        }

        public void StartOfTurnEffects(object sender, Grid.TurnEventArgs turnEvent) {
            if (turnEvent.Team == Team) {
                HasActed = false;
                foreach (Effect effect in TurnStartEffects) {
                    if (effect.Duration > 0) {
                        effect.Execute(this);
                    }
                }
            }
           
        }

        public void EndOfTurnEffects(object sender, Grid.TurnEventArgs turnEvent) {
            if (turnEvent.Team == Team) {
                HasActed = true;
                foreach (Effect effect in TurnEndEffects) {
                    effect.Execute(this);
                }
            }
            
        }

        public void CastAbility(Effect effect, Unit unit) {
            unit.AddEffect(effect);
        }

        public void AddEffect(Effect effect) {
            if (effect.PointOfAction == Effect.Frequency.Immediate) {
                Debug.Log("Added Immediate");
                effect.Execute(this);
                TurnStartEffects.Add(effect);
            } else if (effect.PointOfAction == Effect.Frequency.StartOfTurn) {
                TurnStartEffects.Add(effect);
            } else {
                TurnEndEffects.Add(effect);
            }
        }

        // Clears tile and unit references and hides unit below the board (z -axis)
        public void Die() {
            Tile.BoardPiece = null;
            this.Tile.gameBoard.Teams[Team].Remove(this);
            this.Tile = null;
            HasDied = true;
            this.gameObject.transform.position -= new Vector3(0, 0, 10);
            this.gameObject.transform.GetChild(0).gameObject.transform.position -= new Vector3(0, 0, 10);
            
        }

        public override string ToString() {
            return Character.Name.ToString() + " on Team" + Team;
        }
    }
}