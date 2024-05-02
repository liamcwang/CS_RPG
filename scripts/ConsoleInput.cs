using System;

namespace UserInput {

    public static class ConsoleInput {

        public static Combatant ObtainTarget(Combatant source, Combat currCombat, TargetType targetType) {
            List<Combatant> validCombatants = new List<Combatant>();
            foreach (int n in currCombat.combatTeams.Keys) {
                switch (targetType) {
                    case TargetType.ENEMY: 
                        if (source.team != n) {
                            validCombatants.AddRange(currCombat.combatTeams[n]);
                        }
                        break;
                    case TargetType.ALLY:
                        if (source.team == n) {
                            validCombatants.AddRange(currCombat.combatTeams[n]);
                        }
                        break;
                    default:
                        validCombatants.AddRange(currCombat.combatTeams[n]);
                        break;

                }
            }
            Console.WriteLine("Please enter a valid target from the following: ");
            for (int i = 0; i < validCombatants.Count(); i++) {
                Console.WriteLine($"{i}: {validCombatants[i].name}");
            }
            int input = Convert.ToInt32(Console.ReadLine());
            return validCombatants[input];
        }

    }

}