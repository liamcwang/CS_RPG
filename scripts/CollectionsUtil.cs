
namespace CollectionsUtil {
    public static class CollectionHelper {
        public static T[] Populate<T>(this T[] arr, T value ) {
            for ( int i = 0; i < arr.Length;i++ ) {
                arr[i] = value;
            }
            return arr;
        }

       public static int EnumLength(Type someEnum) {
            return Enum.GetNames(someEnum).Length;
       }

       public static T?[] RemoveFirstElement<T> (this T?[] arr) {
            for (int i = 1; i < arr.Length; i++) {
                arr[i-1] = arr[i];
            }

            arr[arr.Length - 1] = default(T);


            return arr;
       }
    }

    public class BitMask {
        public Int32 value = 1;
        private string[] names = new string[32];


        public BitMask() {
            names[0] = "None";
            names[1] = "Default";
        }

        public void SetBit(int bit, string name) {
            if (bit < 1) {
                throw new ArgumentException("Bit given lower than 2, must not ovewrite preset values");
            }
            if (bit > 31) {
                throw new ArgumentException("Bit given greater than 31.");
            }

            Int32 newBit = 1 << bit;
            names[bit] = name;
            value = value | newBit;
        }

        public void ClearBit(int bit) {
            if (bit < 1) {
                throw new ArgumentException("Bit given lower than 2, must not ovewrite preset values");
            }
            if (bit > 31) {
                throw new ArgumentException("Bit given greater than 31.");
            }

            Int32 newBit = ~(1 << bit);
            names[bit] = "";
            value = value | newBit;
        }

        public bool NameInMask(string name) {
            for (int i = 0; i < names.Length; i++) {
                if (names[i] == name) {
                    return true;
                }
            }
            return false;
        }

        public Int32 NameToBit(string name) {
            for (int i = 0; i < names.Length; i++) {
                if (names[i] == name) {
                    return 1 << i;
                }
            }
            throw new ArgumentException($"{name} not found in bitmask");
        }
    }
}

    
    