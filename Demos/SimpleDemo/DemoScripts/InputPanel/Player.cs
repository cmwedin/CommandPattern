using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo {
    public class Player : MonoBehaviour
    {
        public bool DemoActive { get; set; }
        public RectTransform boundingBox;
        [SerializeField] private int baseSpeed = 100;
        [SerializeField] private float sprintFactor = 1.5f;
        [SerializeField] private GameObject projectileVisuals;
        [SerializeField] private GameObject altProjectileVisuals;


        InputCommandStream inputCommandStream;
        // Start is called before the first frame update
        void Start()
        {
            inputCommandStream = InputCommandStream.Instance;
        }

        // Update is called once per frame
        void Update()
        {
            if (DemoActive)
            {
                Vector2 movementVector = ReadMovementInput();
                if (movementVector != Vector2.zero) {
                    if (Input.GetKey(inputCommandStream.sprintKey)) {
                        inputCommandStream.QueueCommand(new MovePlayerCommand(this, movementVector, sprintFactor * baseSpeed));
                    } else {
                        inputCommandStream.QueueCommand(new MovePlayerCommand(this, movementVector, baseSpeed));
                    }
                }

                if (Input.GetKeyDown(inputCommandStream.fireKeyCode)) {
                    inputCommandStream.QueueCommand(new SpawnProjectileCommand(this, projectileVisuals, 200));
                } else if (Input.GetKeyDown(inputCommandStream.altFireKeyCode)) {
                    inputCommandStream.QueueCommand(new SpawnProjectileCommand(this, altProjectileVisuals, 50));

                }

                if (Input.GetKey(inputCommandStream.undoKeyCode))
                {
                    inputCommandStream.Undo();
                }
            }
        }
        private Vector2 ReadMovementInput() {
            Vector2 output = Vector2.zero;
            if (Input.GetKey(inputCommandStream.upKeyCode)) {
                output += Vector2.up;
            } if (Input.GetKey(inputCommandStream.downKeyCode)) {
                output += Vector2.down;
            } if (Input.GetKey(inputCommandStream.leftKeyCode)) {
                output += Vector2.left;
            } if (Input.GetKey(inputCommandStream.rightKeyCode)) {
                output += Vector2.right;
            }
            return output;
        }
    }
}