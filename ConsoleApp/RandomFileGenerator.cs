using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class RandomFileGenerator
    {

        public static void Generate(string fileName, int columns, int rows)
            => File.WriteAllLines(fileName, GetFileContent(columns, rows));


        private static IEnumerable<string> GetFileContent(int columns, int rows)
            => Enumerable.Range(0, rows)
                         .Select(x => GetDataRow(columns));

        private static string GetDataRow(int columns)
             => Enumerable.Range(0, columns)
                          .Select(ColumnData)
                          .Aggregate(string.Empty, (acc, col) => acc + col);



        private static string ColumnData(int i)
            => Guid.NewGuid().ToString()+',';
    }
}
