using System;
using System.Text;

namespace MixedColumns_AES
{
    class Program
    {
        static void Main(string[] args)
        {
            MixColumns();
        }
        private static byte GMul(byte a, byte b)
        { // Galois Field (256) Multiplication of two Bytes
            byte p = 0;

            for (int counter = 0; counter < 8; counter++)
            {
                if ((b & 1) != 0)
                {
                    p ^= a;
                }

                bool hi_bit_set = (a & 0x80) != 0;
                a <<= 1;
                if (hi_bit_set)
                {
                    a ^= 0x1B; /* x^8 + x^4 + x^3 + x + 1 */
                }
                b >>= 1;
            }

            return p;
        }

        private static void MixColumns()
        {
            int rows = 4;
            //[nr. rows, nr. columns]
            byte[,] inputArray = new byte[4, 4];
            //00 , 01 , 02 ,03
            //10 , 11 , 12 ,13
            //20 , 21 , 22 ,23
            //30 , 21 , 32 ,33
           //1. ROW
            inputArray[0, 0] = 0x00;
            inputArray[1, 0] = 0x00;
            inputArray[2, 0] = 0x00;
            inputArray[3, 0] = 0x00;
            //2. ROW
            inputArray[0, 1] = 0x00;
            inputArray[1, 1] = 0x00;
            inputArray[2, 1] = 0x00;
            inputArray[3, 1] = 0x00;
            // 3.ROW
            inputArray[0, 2] = 0x00;
            inputArray[1, 2] = 0x00;
            inputArray[2, 2] = 0x00;
            inputArray[3, 2] = 0x00;
            //4. ROW
            inputArray[0, 3] = 0x00;
            inputArray[1, 3] = 0x00;
            inputArray[2, 3] = 0x00;
            inputArray[3, 3] = 0x00;
            byte[,] outputArray = new byte[4, 4];

            Array.Clear(outputArray, 0, outputArray.Length);

            for (int c = 0; c < rows; c++)
            {
                outputArray[0, c] = (byte)(GMul(0x02, inputArray[0, c]) ^ GMul(0x03, inputArray[1, c]) ^ inputArray[2, c] ^ inputArray[3, c]);
                outputArray[1, c] = (byte)(inputArray[0, c] ^ GMul(0x02, inputArray[1, c]) ^ GMul(0x03, inputArray[2, c]) ^ inputArray[3, c]);
                outputArray[2, c] = (byte)(inputArray[0, c] ^ inputArray[1, c] ^ GMul(0x02, inputArray[2, c]) ^ GMul(0x03, inputArray[3, c]));
                outputArray[3, c] = (byte)(GMul(0x03, inputArray[0, c]) ^ inputArray[1, c] ^ inputArray[2, c] ^ GMul(0x02, inputArray[3, c]));
            }
            Print2DArray(outputArray);
        }

        public static void Print2DArray(byte[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j].ToString("X") + "\t");
                }
                Console.WriteLine();
            }
        }
    }
}
