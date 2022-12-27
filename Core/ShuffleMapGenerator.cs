namespace Core
{
    internal static class ShuffleMapGenerator
    {
        public static int[] GetMixedArrayFromSeed(int seed, int size)
        {

            // Initialize the array with the numbers in the sequence from 1 to 256
            var mixedArray = new int[size];
            for (var i = 0; i < size; i++)
            {
                mixedArray[i] = i;
            }

            // Mix the elements of the array using the Fisher-Yates shuffle

            Random rng = new Random(seed);
            for (var i = mixedArray.Length - 1; i > 0; i--)
            {
                var j = rng.Next(i + 1);
                var temp = mixedArray[i];
                mixedArray[i] = mixedArray[j];
                mixedArray[j] = temp;
            }

            // Print the mixed array to the console
            /*Console.WriteLine("Mixed array:");
            foreach (int n in mixedArray)
            {
                Console.Write(n + " ");
            }
            Console.WriteLine();*/
            return mixedArray;

        }
    }
}