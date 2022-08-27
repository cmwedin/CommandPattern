using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.EditorTesting
{
    public class Ticker
    {
        public int count = 0;

        public Ticker()
        {
        }
    }

    public class TickerCommand : Command, IUndoable
    {
        public Ticker ticker;

        public TickerCommand(Ticker ticker)
        {
            this.ticker = ticker;
        }

        public override void Execute()
        {
            ticker.count++;
        }

        public ICommand GetUndoCommand()
        {
            return new UndoTickerCommand(this);
        }
    }
    public class UndoTickerCommand : Command, IUndoable
    {
        public Ticker ticker;

        public UndoTickerCommand(TickerCommand command)
        {
            ticker = command.ticker;
        }

        public override void Execute()
        {
            ticker.count--;
        }

        public ICommand GetUndoCommand()
        {
            return new TickerCommand(ticker);
        }
    }
}