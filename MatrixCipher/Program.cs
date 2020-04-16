using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixCipher {
    class Program {
        static void Main(string[] args) {

            Console.WriteLine("3x3 Matrix: [int array]: ");
            var matrix = ReadInputArray();

            var inputString = "";
            string input;

            Console.WriteLine("Input string [empty = next step]: ");
            do {
                input = Console.ReadLine().Trim().ToLower();
                inputString += input + '\n';
            } while (input != "");

            var result = Matrixate(matrix, inputString);

            Console.WriteLine("Result [int]:");
            for (int i = 0; i < result.Length; i++) {
                Console.Write((int)result[i] + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Result:");
            Console.WriteLine(result);
            Console.ReadKey();

        }

        static int[,] ReadInputArray() {

            int[,] result = new int[3, 3];

            string input;
            int inputInt;

            int i = 0;

            do {
                input = Console.ReadLine().Trim().ToLower();

                if (input.Contains(" ")) {
                    foreach (var item in input.Split(' ')) {
                        if (i >= 3 * 3) {
                            return result;
                        }

                        if (int.TryParse(item, out inputInt)) {
                            result[i / 3, i % 3] = inputInt;
                            i++;
                        }

                        if (i >= 3 * 3) {
                            return result;
                        }
                    }

                    continue;
                }

                if (i >= 3 * 3 || input == "" || !int.TryParse(input, out inputInt)) {
                    break;
                }

                result[i / 3, i % 3] = inputInt;
                i++;

                if (i >= 3 * 3) {
                    break;
                }
            } while (true);

            return result;
        }

        public static int[] MatrixMultiplyVector(int[,] matrix, int[] vector, int size = 3) {

            if (matrix.GetLength(0) != size || matrix.GetLength(1) != size || vector.Length != size) {
                return null;
            }

            int[] result = new int[size];

            int c;

            for (int y = 0; y < size; y++) {
                c = 0;
                for (int x = 0; x < size; x++) {
                    c += matrix[y, x] * vector[x];
                }
                result[y] = c;
            }

            return result;

        }

        public static string Matrixate(int[,] code, string inputString) {

            string result = "";

            int[] vector = new int[3];

            int[] resultVector = new int[3];

            for (int i = 0; i < inputString.Length; i++) {
                vector[i % 3] = inputString[i];

                if (i % 3 == 2) {
                    resultVector = MatrixMultiplyVector(code, vector);
                    for (int j = 0; j < resultVector.Length; j++) {
                        result += (char) resultVector[j];
                    }
                }
            }

            if (result.Length != inputString.Length) {
                var size = inputString.Length - result.Length;
                if (size == 2) {
                    result += code[0, 0] * inputString[result.Length] + code[0, 1] * inputString[result.Length + 1]
                                + code[1, 0] * inputString[result.Length] + code[1, 1] * inputString[result.Length + 1];
                } else if (size == 1) {
                    result += code[0, 0] * inputString[result.Length];
                }
            }

            return result;

        }

    }
}
