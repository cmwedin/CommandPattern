using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo {
    public class Player : MonoBehaviour
    {
        public bool DemoActive { get; set; }
        public RectTransform boundingBox;
        [SerializeField] private int speed;
        InputCommandStream inputCommandStream;
        // Start is called before the first frame update
        void Start()
        {
            inputCommandStream = InputCommandStream.Instance;
        }

        // Update is called once per frame
        void Update() {
            if(DemoActive) {
                Vector2 movementVector = Vector2.zero;
                if(Input.GetKey(inputCommandStream.upKeyCode)) {
                    movementVector += speed * Time.deltaTime * Vector2.up;
                } if (Input.GetKey(inputCommandStream.downKeyCode)) {
                    movementVector += speed * Time.deltaTime * Vector2.down;
                } if (Input.GetKey(inputCommandStream.leftKeyCode)) {
                    movementVector += speed * Time.deltaTime * Vector2.left;
                } if (Input.GetKey(inputCommandStream.rightKeyCode)) {
                    movementVector += speed * Time.deltaTime * Vector2.right;
                } if (Input.GetKeyDown(inputCommandStream.fireKeyCode)) {
                    Debug.Log("Fire key pressed");
                }
                if(movementVector != Vector2.zero) {
                    inputCommandStream.QueueCommand(new MovePlayerCommand(this, movementVector));
                }
            }
        }
    }
}