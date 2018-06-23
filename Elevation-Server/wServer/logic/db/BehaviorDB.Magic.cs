using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Magic = () => Behav()

        .Init("Magic Nymph",
            new State(
                new State("attack",
                        new Follow(0.5, range: 1),
                        new Shoot(10, 6, 10, projectileIndex: 0, coolDown: 1500),
                        new TimedTransition(6000, "b4bombs")
                    ),
                new State("b4bombs",
                     new Flash(0xf389E13, 0.3, 60),
                     new TimedTransition(3000, "bombs")
                    ),
                new State("bombs",
                    new Grenade(4, 80, 10, coolDown: 500),
                    new TimedTransition(3000, "attack")
                    )
                    )
            )
         .Init("Magic Dragon",
            new State(
                new State("attack",
                        new Follow(1, range: 1),
                        new Shoot(25, projectileIndex: 0, count: 4, shootAngle: 10, coolDown: 1000, coolDownOffset: 1000),                    
                        new TimedTransition(6000, "glow")
                    ),
                new State("glow",
                    new Flash(0x7FFFD4, 0.3, 60),
                    new TimedTransition(3000, "spray")
                    ),
                new State("spray",
                        new Shoot(25, count: 1, coolDown: 400, coolDownOffset: 0, shootAngle: 90, projectileIndex: 1),
                        new Shoot(25, count: 1, coolDown: 200, coolDownOffset: 200, shootAngle: 90, projectileIndex: 2),
                        new TimedTransition(4000, "attack")
                    )
                )
                )
          .Init("Stone Dragon",
            new State(
                new State("attack",
                        new Follow(1, range: 1),
                        new Shoot(10, count: 4, fixedAngle: 0, shootAngle: 6, projectileIndex: 0, coolDown: 500),
                        new Shoot(10, count: 4, fixedAngle: 180, shootAngle: 6, projectileIndex: 0, coolDown: 500)

                    )
                )
        )

        .Init("Ruler of Eternity",
            new State(
                new State("wait",
                    new PlayerWithinTransition(7, "activate")
                    ),
                new State("activate",
                    new Taunt("Ah, I see you are the hero that intruded my place"),
                    new TimedTransition(3000, "taunt1")
                    ),
                new State("taunt1",
                    new Flash(0x7FFFD4, 0.3, 60),
                    new Taunt("Come closer and I shall provide you a gift."),
                    new TimedTransition(4000, "taunt2")
                    ),
                new State("taunt2",
                    new Taunt("AN ETERNITY OF DESPAIR!!!"),
                    new TimedTransition(5000, "attack")
                    ),
                new State("attack",
                    new Shoot(1, 4, projectileIndex: 0, coolDown: 4575, fixedAngle: 90, coolDownOffset: 0, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 0, coolDown: 3575, fixedAngle: 100, coolDownOffset: 200, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 0, coolDown: 3575, fixedAngle: 110, coolDownOffset: 400, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 0, coolDown: 3575, fixedAngle: 120, coolDownOffset: 600, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 0, coolDown: 3575, fixedAngle: 130, coolDownOffset: 800, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 0, coolDown: 3575, fixedAngle: 140, coolDownOffset: 1000, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 0, coolDown: 3575, fixedAngle: 150, coolDownOffset: 1200, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 0, coolDown: 3575, fixedAngle: 160, coolDownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 0, coolDown: 3575, fixedAngle: 170, coolDownOffset: 1600, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 0, coolDown: 3575, fixedAngle: 180, coolDownOffset: 1800, shootAngle: 90),
                            new Shoot(1, 8, projectileIndex: 0, coolDown: 3575, fixedAngle: 180, coolDownOffset: 2000, shootAngle: 45),
                            new Shoot(1, 4, projectileIndex: 0, coolDown: 3575, fixedAngle: 180, coolDownOffset: 0, shootAngle: 90)



                    )
                )

            )
                ;
           }
    }
                 