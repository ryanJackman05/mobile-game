# 1 - The game’s current economy loop plan

Coins are **earned** by the player from hitting enemies, collecting items, and breaking distance records.
The player can **spend** coins to buy power ups and upgrades before a run, or to restart a failed run from game over.
Upgrades may come in the form of temporary invincibility, or increased damage resistance

# 2 - Map economy to model

Consumables that the player holds aren't part of the game spec. All upgrades or powerups will apply upon purchase or "collect"

As I'm not keen on in-app purchases, I would like to include an ad watch -> reward option, if the player wished for a free restart or powerup. 

I think banner ads only have suitability in the main menu or settings screens, and even then, I can see it being hard to integrate without feeling invasive.

Lootboxes & season passes are a hard No.

Providing players with a remove ads option is a nice idea, but I would hope to have ad integration that is minimally invasive, and wouldn't be aggressive enough to incentivise players into spending their money.

A hybrid model suits my intent best, but I would have to be cautious of the risk of complexity.

# 3 - Potential Dark Patterns

- This spec has no notable temporal exploitations, other than possibly a timed "Watch Ad?" button beforee the player can continue. (Nagging)
- I don't want scarcity to become an incentive for buying upgrades, so game balancing will affect this quite a bit.
- Depending on the damage scaling of enemy zombies, the reduced damage powerups may become a big incentivisor for wanting a powerup. Powerups can not be mandatory for progress.
- Given that a leaderboard is a very suitable feature for this app, I have to be careful that earning points and breaking records is easy without purchases or ads.
- Interstitial Ad traps hopefully won't apply here, but they have to be considered if watching ads manually is going to be an option. The current Google Play add integration is horrifically hard to dismiss.

### Guardrails
| Risk | Guardrail |
|---|---|
| Purchase prompts around play | A Buy button on the Menu or Result screen only; never during play, and never a pop-up or offer pushed at the player after a fail |
| Premium currency | None |
| Random rewards | No rewards |
| Daily rewards | none |
| Timers | No restricting timers |
| Ads | Rewarded ads only, opt-in, with a clear close button, usually triggered from Result screen |
| Limited-time offers | None |
| Children in the audience | Not aimed at children, but still no purchase prompts aimed at children; |
| Subscriptions | None |