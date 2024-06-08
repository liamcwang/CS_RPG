using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using static CollectionsUtil.CollectionHelper;
using static EventUtil.DataRequests;

namespace DataEditing {
    public struct EffectData {
        public string[] types {get; set;}
        public float[] values {get; set;}
    
        public EffectData(int size) {
            types = new string[size];
            values = new float[size];
        }


    }

    public struct CombatSkillData {
        public string[] names {get; set;}
        public int[] priorities {get; set;}
        public string[] targetTypes {get; set;}
        public string[] rangeTypes {get; set;}
        public EffectData[] effects {get; set;}
        public int size;

        public CombatSkillData(int size) {
            names = new string[size];
            priorities = new int[size];
            targetTypes = new string[size];
            rangeTypes = new string[size];
            effects = new EffectData[size];
            this.size = size;
        }

        
    }

    public struct CombatSkillRef {
        public string[] skillNames {get; set;}

        public CombatSkillRef(int size) {
            skillNames = new string[size];
        }
    }

    public struct CombatantData {
        public string[] names {get; set;}
        public int[] teams {get; set;}
        public CombatSkillRef[] skills {get; set;}

        public CombatantData(int size) {
            teams = new int[size];
            names = new string[size];
            skills = new CombatSkillRef[size];
        }
    }


    public class DataEditor {
        // TODO: Thinking about creating a temp file that stores all the active program information, then when we need to edit a batch of data we open the data for that part of the program
        // TODO: after working on all this stuff, it seems that my method is really stupid somehow. Maybe just iterate through the references in gamemanager instead? Or be stubborn and iterate the structures to their parts because they represent the modifieable state better
        


        public static CombatSkillData AssembleCombatSkillData() {
            CombatSkill[] combatSkillRef = new CombatSkill[0]; 
            RequestCombatSkillsRef.Invoke(ref combatSkillRef);
            CombatSkillData combatSkillData = new CombatSkillData(combatSkillRef.Count());
            string[] targetTypeNames = Enum.GetNames(typeof(TargetType));
            string[] rangeTypeNames = Enum.GetNames(typeof(RangeType));
            string[] effectTypeNames = Enum.GetNames(typeof(EffectType));
            for (int i = 0; i < combatSkillRef.Count(); i++) {
                // Break the CombatSkill object into its values and enter them into the arrays
                CombatSkill cs = combatSkillRef[i];
                combatSkillData.names[i] = cs.name;
                combatSkillData.priorities[i] = cs.priority;
                combatSkillData.targetTypes[i] = targetTypeNames[(int) cs.targetType];
                combatSkillData.rangeTypes[i] = rangeTypeNames[(int) cs.rangeType];


                EffectData ed = new EffectData(cs.effects.Count());
                for (int j = 0; j < cs.effects.Count(); j++) {
                    Effect e = cs.effects[j];
                    ed.types[j] = effectTypeNames[(int) e.effectType];
                    ed.values[j] = e.value;
                }
                combatSkillData.effects[i] = ed;
            }
            return combatSkillData;
        }   

        public static CombatantData AssembleCombatantData() {
            Combatant[] combatantRef = new Combatant[0];
            RequestCombatantsRef.Invoke(ref combatantRef);
            CombatantData combatantData = new CombatantData(combatantRef.Count());
            for (int i = 0; i < combatantRef.Count(); i++) {
                Combatant c = combatantRef[i];
                combatantData.names[i] =  c.name;
                combatantData.teams[i] = c.team;
                CombatSkillRef skillRef = new CombatSkillRef(c.skills.Count());
                for (int j = 0; j < c.skills.Count(); j++) {
                    CombatSkill cs = c.skills[j];
                    skillRef.skillNames[j] = cs.name;
                }
                combatantData.skills[i] = skillRef;
            }   
            return combatantData;
        } 
    }
}