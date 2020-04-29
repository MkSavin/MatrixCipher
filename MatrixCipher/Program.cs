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

            Console.WriteLine("Input string [int]: ");
            for (int i = 0; i < inputString.Length; i++) {
                Console.Write((int)inputString[i] + " ");
            }
            Console.WriteLine();
            Console.WriteLine();

            var result = Matrixate(matrix, inputString);

            Console.WriteLine("Crypted result [int]:");
            for (int i = 0; i < result.Length; i++) {
                Console.Write((int)result[i] + " ");
            }

            Console.WriteLine();
            Console.WriteLine("Crypted result:");
            Console.WriteLine(result);

            matrix = Inverse(matrix);

            result = Matrixate(matrix, result);

            Console.WriteLine("Decrypted crypted result [int]:");
            for (int i = 0; i < result.Length; i++) {
                Console.Write((int)result[i] + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Decrypted crypted result:");
            Console.WriteLine(result);

            Console.ReadKey();

        }

        static float[,] ReadInputArray() {

            float[,] result = new float[3, 3];

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

        public static float[] MatrixMultiplyVector(float[,] matrix, float[] vector, int size = 3) {

            if (matrix.GetLength(0) != size || matrix.GetLength(1) != size || vector.Length != size) {
                return null;
            }

            float[] result = new float[size];

            float c;

            for (int y = 0; y < size; y++) {
                c = 0;
                for (int x = 0; x < size; x++) {
                    c += (float)Math.Round(matrix[y, x] * vector[x]);
                }
                result[y] = c;
            }

            return result;

        }

        public static string Matrixate(float[,] code, string inputString) {

            string result = "";

            float[] vector = new float[3];

            float[] resultVector = new float[3];

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

        public static float[,] Inverse(float[,] matrix) {

            if (matrix.GetLength(0) != 3 || matrix.GetLength(1) != 3) {
                return null;
            }

            var d3 = Det3(matrix);

            float[,] result = new float[,] {
                {
                    Det2(new float[,] { { matrix[1, 1], matrix[1, 2] }, { matrix[2, 1], matrix[2, 2] } }) / d3,
                    Det2(new float[,] { { matrix[0, 2], matrix[0, 1] }, { matrix[2, 2], matrix[2, 1] } }) / d3,
                    Det2(new float[,] { { matrix[0, 1], matrix[0, 2] }, { matrix[1, 1], matrix[1, 2] } }) / d3,
                },
                {
                    Det2(new float[,] { { matrix[1, 2], matrix[1, 0] }, { matrix[2, 2], matrix[2, 0] } }) / d3,
                    Det2(new float[,] { { matrix[0, 0], matrix[0, 2] }, { matrix[2, 0], matrix[2, 2] } }) / d3,
                    Det2(new float[,] { { matrix[0, 2], matrix[0, 0] }, { matrix[1, 2], matrix[1, 0] } }) / d3,
                },
                {
                    Det2(new float[,] { { matrix[1, 0], matrix[1, 1] }, { matrix[2, 0], matrix[2, 1] } }) / d3,
                    Det2(new float[,] { { matrix[0, 1], matrix[0, 0] }, { matrix[2, 1], matrix[2, 0] } }) / d3,
                    Det2(new float[,] { { matrix[0, 0], matrix[0, 1] }, { matrix[1, 0], matrix[1, 1] } }) / d3,
                }
            };

            return result;
        }

        public static float Det2(float[,] matrix) {

            if (matrix.GetLength(0) != 2 || matrix.GetLength(1) != 2) {
                return 0;
            }

            return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

        }

        public static float Det3(float[,] matrix) {

            if (matrix.GetLength(0) != 3 || matrix.GetLength(1) != 3) {
                return 0;
            }

            return
                matrix[0, 0] * matrix[1, 1] * matrix[2, 2] -
                matrix[0, 0] * matrix[2, 1] * matrix[1, 2] -
                matrix[1, 0] * matrix[0, 1] * matrix[2, 2] +
                matrix[1, 0] * matrix[2, 1] * matrix[0, 2] +
                matrix[2, 0] * matrix[0, 1] * matrix[1, 2] -
                matrix[2, 0] * matrix[1, 1] * matrix[0, 2];

        }

    }
}
