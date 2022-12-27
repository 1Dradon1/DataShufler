namespace Core
{
    public class DataShuffler
    {
        internal static string[] ShuffleData(string[] data, int[] shufleMap, int chunkSize)
        {
            var resultData = new string[data.Length];
            var counter = 0;
            var chunks = data.Chunk(chunkSize);

            foreach (var chunk in chunks)
            {
                for (var i = 0; i < chunkSize; i++)
                    resultData[shufleMap[i] + counter * chunkSize] = data[counter * chunkSize + i];

                counter++;
            }

            return resultData;
        }

        internal static string[] UnshuffleData(string[] data, int[] shufleMap, int chunkSize)
        {
            var resultData = new string[data.Length];
            var counter = 0;
            var chunks = data.Chunk(chunkSize);

            foreach (var chunk in chunks)
            {

                for (var i = 0; i < chunkSize; i++)
                    resultData[i + counter * chunkSize] = data[shufleMap[i] + counter * chunkSize];

                counter++;
            }

            return resultData;
        }

        public static string[] ShuffleColumn(string[] data, int seed1, int seed2, int chunkSize)
        {
            var shuffleMap1 = ShuffleMapGenerator.GetMixedArrayFromSeed(seed1, chunkSize);
            var shuffleMap2 = ShuffleMapGenerator.GetMixedArrayFromSeed(seed2, chunkSize);

            return ShuffleData(ShuffleData(data, shuffleMap1, chunkSize), shuffleMap2, chunkSize);
        }

        public static string[] UnshuffleColumn(string[] data, int seed1, int seed2, int chunkSize)
        {
            var shuffleMap1 = ShuffleMapGenerator.GetMixedArrayFromSeed(seed1, chunkSize);
            var shuffleMap2 = ShuffleMapGenerator.GetMixedArrayFromSeed(seed2, chunkSize);

            return UnshuffleData(UnshuffleData(data, shuffleMap2, chunkSize), shuffleMap1, chunkSize);
        }
    }
}
