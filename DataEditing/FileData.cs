using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using static CollectionsUtil.CollectionHelper;

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

        public CombatSkillData(int size) {
            names = new string[size];
            priorities = new int[size];
            targetTypes = new string[size];
            rangeTypes = new string[size];
            effects = new EffectData[size];
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
            CombatSkill[] combatSkillRef = GameData.combatSkillRef;
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
            Combatant[] combatantRef = GameData.combatantRef;
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

        /// <summary>
        /// Unpacks a CombatSkillData bundle for use in the program
        /// </summary>
        /// <param name="combatSkillData"></param>
        public static void UnpackCombatSkillDataBundle(CombatSkillData combatSkillData) {
             
            int dataLength = combatSkillData.names.Length;
            GameData.combatSkillRef = new CombatSkill[dataLength];
            for (int i = 0; i < dataLength; i++) {
                GameData.combatSkillRef[i] = new CombatSkill();
                GameData.combatSkillRef[i].name = combatSkillData.names[i];
                GameData.combatSkillRef[i].priority = combatSkillData.priorities[i];
                GameData.combatSkillRef[i].targetType = (TargetType) Enum.Parse(typeof(TargetType), combatSkillData.targetTypes[i]); 
                GameData.combatSkillRef[i].rangeType = (RangeType) Enum.Parse(typeof(RangeType), combatSkillData.targetTypes[i]);

                Effect[] effectData = new Effect[combatSkillData.effects.Length];
                for (int j = 0; j < combatSkillData.effects.Length; j++) {
                    EffectType effType = (EffectType) Enum.Parse(typeof(EffectType),combatSkillData.effects[i].types[j]);
                    effectData[i] = new Effect(effType);
                    effectData[i].value = combatSkillData.effects[i].values[j];
                }
                GameData.combatSkillRef[i].effects = effectData;
            }
        }

        /// <summary>
        /// Unpacks a CombatantData bundle for use in the program
        /// </summary>
        /// <param name="combatantData"></param>
        public static void UnpackCombatantDataBundle(CombatantData combatantData) {

        }
    }
}