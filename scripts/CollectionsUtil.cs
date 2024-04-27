
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
    }
    }

    
    