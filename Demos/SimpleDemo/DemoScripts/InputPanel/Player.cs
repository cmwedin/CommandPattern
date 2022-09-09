using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo {
    /// <summary>
    /// The component for the player GameObject
    /// </summary>
    public class Player : MonoBehaviour
    {
        /// <summary>
        /// Wether the code for detecting inputs should be run (i.e. if the demo is active or not)
        /// </summary>
        public bool ReadInputs { get; set; }
        /// <summary>
        /// The RectTransform within which the player can move
        /// </summary>
        public RectTransform boundingBox;
        /// <summary>
        /// The base speed the player moves at
        /// </summary>
        [SerializeField] private int baseSpeed = 100;
        /// <summary>
        /// The factor by which sprinting modifies the base speed
        /// </summary>
        [SerializeField] private float sprintFactor = 1.5f;
        /// <summary>
        /// A QoL list to easily determine if a KeyCode corresponds to a mouse button
        /// </summary>
        private List<KeyCode> mouseButtons = new List<KeyCode> {
            KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse2
        };
        /// <summary>
        /// The currently selected projectile type for the primary fire
        /// </summary>
        private ProjSO primaryProjType;
        /// <summary>
        /// Sets the primary fire's projectile type
        /// </summary>
        /// <param name="projSO">The projectile scriptable object to set</param>
        public void SetPrimaryProj(ProjSO projSO) {
            primaryProjType = projSO;
        }

        /// <summary>
        /// The currently selected projectile type for the alternate fire
        /// </summary>
        private ProjSO altProjType;
        /// <summary>
        /// Sets the alternate fire's projectile type
        /// </summary>
        /// <param name="projSO">The projectile scriptable object to set</param>
        public void SetAltProj(ProjSO projSO) {
            altProjType = projSO;
        }
        /// <summary>
        /// The length of time after which a projectile should be destroyed regardless of its position (only relevant for the random ricochet projectile type)
        /// </summary>
        [SerializeField,Tooltip("The maximum length of time a projectile should exist in seconds")] private int projMaxLifespan = 100;

        /// <summary>
        /// a QoL member to shorten lines accessing the InputCommandStream instance
        /// </summary>
        InputCommandStream inputCommandStream;
        /// <summary>
        /// sets the inputCommandStream reference
        /// </summary>
        void Start()
        {
            inputCommandStream = InputCommandStream.Instance;
        }

        /// <summary>
        /// Determine what (excluding movement commands)commands to send to the InputCommandStream instance based on the entered inputs (if ReadInputs is true)
        /// if the fire inputs are set to a mouse button also requires that the mouse be in the bounding box
        /// </summary>
        void Update()
        {
            if (ReadInputs)
            {
                if (
                    Input.GetKeyDown(inputCommandStream.InputKeybinds[InputType.Fire]) 
                    && primaryProjType != null
                    && (boundingBox.Contains(Input.mousePosition) || !mouseButtons.Contains(inputCommandStream.InputKeybinds[InputType.Fire]))
                ) {
                    inputCommandStream.QueueCommand(new SpawnProjectileCommand(this, primaryProjType,projMaxLifespan));
                } else if (
                    Input.GetKeyDown(inputCommandStream.InputKeybinds[InputType.AltFire]) 
                    && altProjType != null
                    && (boundingBox.Contains(Input.mousePosition) || !mouseButtons.Contains(inputCommandStream.InputKeybinds[InputType.Fire]))
                ) {
                    inputCommandStream.QueueCommand(new SpawnProjectileCommand(this, altProjType,projMaxLifespan));
                }

                if (Input.GetKey(inputCommandStream.InputKeybinds[InputType.Undo]))
                {
                    inputCommandStream.Undo();
                }
            }
        }
        /// <summary>
        /// The same as above but for movement inputs
        /// </summary>
        private void FixedUpdate() {
            if(ReadInputs) {
                Vector2 movementVector = ReadMovementInput();
                if (movementVector != Vector2.zero) {
                    if (Input.GetKey(inputCommandStream.InputKeybinds[InputType.Sprint])) {
                        inputCommandStream.QueueCommand(new MovePlayerCommand(this, movementVector, sprintFactor * baseSpeed));
                    } else {
                        inputCommandStream.QueueCommand(new MovePlayerCommand(this, movementVector, baseSpeed));
                    }
                }
            }
            
        }
        /// <summary>
        /// combines the directional inputs into a single vector
        /// </summary>
        /// <returns>a vector represent which directional input where entered</returns>
        private Vector2 ReadMovementInput() {
            Vector2 output = Vector2.zero;
            if (Input.GetKey(inputCommandStream.InputKeybinds[InputType.MoveUp])) {
                output += Vector2.up;
            } if (Input.GetKey(inputCommandStream.InputKeybinds[InputType.MoveDown])) {
                output += Vector2.down;
            } if (Input.GetKey(inputCommandStream.InputKeybinds[InputType.MoveLeft])) {
                output += Vector2.left;
            } if (Input.GetKey(inputCommandStream.InputKeybinds[InputType.MoveRight])) {
                output += Vector2.right;
            }
            return output;
        }
    }
}