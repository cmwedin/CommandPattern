using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SadSapphicGames.CommandPattern;

public class Ticker {
    public int count = 0;

    public Ticker() {
    }
}

public class TickerCommand : Command {
    private Ticker ticker;

    public TickerCommand(Ticker ticker) {
        this.ticker = ticker;
    }

    public override void Execute() {
        ticker.count++;
    }
}
