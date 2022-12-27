using Core;
using System.Data;

namespace ConsoleApplication
{
    class Program
    {
        private static string _inputFileName;
        private const string OutputFileName = "tableRow.csv";
        private const int ChunkSize = 256;
        private static int _seed1;
        private static int _seed2;
        private static int[] _columnsToShuffleIndicies = null!;
        private static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Введите название зашированного файла");
            _inputFileName = Console.ReadLine();
            Console.WriteLine("Введите seed1");
            _seed1 = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите seed2");
            _seed2 = int.Parse(Console.ReadLine());
            CheckFiles();

            



            var extendedQueue = new ExtendedQueue(ChunkSize);
            extendedQueue.GroupCollected += HandleGroup;

            var lines = File.ReadLines(_inputFileName);
            var firstLine = lines.First();
            var columns = firstLine.Split(',').ToArray();

            using var file = new StreamWriter(OutputFileName, append: true);
            file.WriteLine(firstLine);
            file.Close();


            _columnsToShuffleIndicies = AskUserAndGetColumnsToShuffleIndicies(columns);

            foreach (var line in lines.Skip(1).Where(x => x != string.Empty))
                extendedQueue.Enqueue(line.Split(',').ToArray());

        }

        private static void CheckFiles()
        {
            if (!File.Exists(_inputFileName))
                throw new Exception("Отсутствует файл данных");

            if (File.Exists(OutputFileName))
            {
                File.WriteAllText(OutputFileName, "");
            }
            else
            {
                using var f = File.Create(OutputFileName);
            }
        }

        private static void HandleGroup(string[][] rows)
        {
            var columns = ParseRowsIntoColumns(rows);
            var tempColumns = new string[columns.Length][];

            for (var i = 0; i < columns.Length; i++)
            {
                if (_columnsToShuffleIndicies.Contains(i))
                    tempColumns[i] = DataShuffler.UnshuffleColumn(columns[i], _seed1, _seed2, ChunkSize);
                else
                    tempColumns[i] = columns[i];
            }

            var resultRows = ParseColumnsIntoRows(tempColumns);

            using var file = new StreamWriter(OutputFileName, append: true);
            foreach (var row in resultRows)
                file.WriteLine(string.Join(',', row));
        }

        private static string[][] ParseRowsIntoColumns(string[][] rows)
        {
            var columns = new string[rows[0].Length][];

            for (var i = 0; i < columns.Length; i++)
                columns[i] = new string[rows.Length];

            for (var i = 0; i < rows.Length; i++)
                for (var j = 0; j < rows[0].Length; j++)
                    columns[j][i] = rows[i][j];


            return columns;
        }

        private static string[][] ParseColumnsIntoRows(string[][] columns)
        {
            var rows = new string[columns[0].Length][];

            for (var i = 0; i < rows.Length; i++)
                rows[i] = new string[columns.Length];

            for (var i = 0; i < columns.Length; i++)
                for (var j = 0; j < columns[0].Length; j++)
                    rows[j][i] = columns[i][j];


            return rows;
        }

        private static int[] AskUserAndGetColumnsToShuffleIndicies(string[] columns)
        {
            var columnsTitle = string.Join(", ", columns);
            Console.WriteLine($"Столбцы: {columnsTitle}");
            Console.WriteLine($"Введите через пробел названия столбцов, которые нужно восстановить");

            var result = new List<int>();
            var columnsInput = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var count = 0;

            foreach (var columnInput in columnsInput)
            {
                for (var i = 0; i < columns.Length; i++)
                {
                    if (columnInput == columns[i])
                    {
                        result.Add(i);
                        count++;
                    }
                }
            }

            if (columnsInput.Length != count)
                throw new Exception("Неверное имя столбца!");

            return result.ToArray();
        }
    }
}