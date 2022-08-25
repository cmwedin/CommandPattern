using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class HealthCommandStream : MonoBehaviour
    {
        private CommandStream internalStream = new CommandStream();
        private Stack<IUndoable> undoStack = new Stack<IUndoable>();
        private IUndoable lastUndo;
        private HealthBar healthBar;
        [SerializeField] private Button IncreaseButton;
        [SerializeField] private Button DecreaseButton;
        [SerializeField] private Button UndoButton;
        private void Start() {
            healthBar = GetComponentInChildren<HealthBar>();
            IncreaseButton?.onClick.AddListener(delegate { internalStream.QueueCommand(HealthCommandFactory.Instance.CreateHealthCommand(healthBar, false)); });
            DecreaseButton?.onClick.AddListener(delegate { internalStream.QueueCommand(HealthCommandFactory.Instance.CreateHealthCommand(healthBar, true)); });
            UndoButton?.onClick.AddListener(delegate {
                if(undoStack.TryPop(out var nextUndo)) {
                    lastUndo = nextUndo;
                    internalStream.TryQueueUndoCommand(nextUndo);
                }
            });

        }
        private void Update() {
            if(internalStream.TryExecuteNext(out var executedCommand)) {
                if(executedCommand is IUndoable && executedCommand != lastUndo?.GetUndoCommand()) {
                    undoStack.Push((IUndoable)executedCommand);
                }
            } else if(executedCommand != null) {
                if (executedCommand is ModifyHealthCommand) {
                if(((ModifyHealthCommand)executedCommand).Magnitude > 0 && healthBar.Health != healthBar.MaxHealth) {
                    internalStream.QueueCommand(new ModifyHealthCommand(healthBar.MaxHealth - healthBar.Health, healthBar));
                } else if (((ModifyHealthCommand)executedCommand).Magnitude < 0 && healthBar.Health != 0) {
                    internalStream.QueueCommand(new ModifyHealthCommand(-healthBar.Health, healthBar));
                }
                }
            }
        }
    }
}
