using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace ViolaJonesCS.Utilities
{
    public class Matrix
    {
        #region GLOBAL

        public int nr_rows { get; set; }
        public int nr_cols { get; set; }
        public int nr_vals { get; set; }
        public double[] data { get; set; }
        public bool isRow { get; set; }

        #endregion

        #region SERIALIZATION

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <param name="file"></param>
        static public void SerializeToXML(Matrix m, string file)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Matrix));
            TextWriter textWriter = new StreamWriter(@file);
            serializer.Serialize(textWriter, m);
            textWriter.Close();
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mList"></param>
        /// <param name="file"></param>
        static public void SerializeListToXML(Matrix[] mList, string file)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Matrix[]));
            TextWriter textWriter = new StreamWriter(@file);
            serializer.Serialize(textWriter, mList);
            textWriter.Close();
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        static public Matrix DeserializeFromXML(string file)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(Matrix));
            TextReader textReader = new StreamReader(@file);
            Matrix matrix = (Matrix)deserializer.Deserialize(textReader);
            textReader.Close();
            return matrix;
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        static public Matrix[] DeserializeListFromXML(string file)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(Matrix[]));
            TextReader textReader = new StreamReader(@file);
            Matrix[] matrix = (Matrix[])deserializer.Deserialize(textReader);
            textReader.Close();
            return matrix;
        }

        #endregion

        #region CONSTRUCTORS

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        public Matrix() { }

        /// CHECKED
        /// <summary>Makes a new Matrix</summary>
        /// <param name="nr_cols">The number of columns of the matrix</param>
        /// <param name="nr_rows">The number of rows of the matrix</param>
        public Matrix(int nr_cols, int nr_rows)
        {
            if (nr_cols < 1 ||
                nr_rows < 1)
            {
                throw new Exception();
            }

            this.nr_cols = nr_cols;
            this.nr_rows = nr_rows;
            this.nr_vals = nr_cols * nr_rows;
            this.data = new double[nr_vals];

            this.isRow = nr_rows == 1;
        }

        /// CHECKED
        ///<summary>Makes a new Matrix</summary>
        ///<param name="nr_cols">The number of columns of the matrix</param>
        ///<param name="nr_rows">The number of rows of the matrix</param>
        ///<param name="data">The data to be inserted in the matrix, inserted in row-first order</param>
        public Matrix(int nr_cols, int nr_rows, double[] data)
        {
            if (nr_cols < 1 ||
                nr_rows < 1 ||
                data.Length != nr_rows * nr_cols)
            {
                throw new Exception();
            }

            this.nr_cols = nr_cols;
            this.nr_rows = nr_rows;
            this.nr_vals = nr_cols * nr_rows;
            this.data = new double[this.nr_vals];
            Array.Copy(data, this.data, this.nr_vals);

            this.isRow = nr_rows == 1;
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        public Matrix(Matrix m)
        {
            this.nr_cols = m.nr_cols;
            this.nr_rows = m.nr_rows;
            this.nr_vals = m.nr_vals;
            this.isRow = m.isRow;
            this.data = new double[this.nr_vals];
            Array.Copy(m.data, this.data, this.nr_vals);
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        public Matrix(Matrix m1, Matrix m2)
        {
            if (m1.nr_cols != m2.nr_cols)
                throw new Exception();

            this.nr_cols = m1.nr_cols;
            this.nr_rows = m1.nr_rows + m2.nr_rows;
            this.nr_vals = m1.nr_vals + m2.nr_vals;
            this.isRow = false;
            this.data = new double[this.nr_vals];
            Array.Copy(m1.data, this.data, m1.nr_vals);
            Array.Copy(m2.data, 0, this.data, m1.nr_vals, m2.nr_vals);
        }

        #endregion

        #region GET/SET

        public Matrix getAbsMatrix()
        {
            Matrix result = new Matrix(this.nr_cols, this.nr_rows);
            for (int i = 1; i <= this.nr_vals; i++)
            {
                result[i] = Math.Abs(this[i]);
            }

            return result;
        }

        public Matrix getRoundedMatrix(int numberOfDecimals)
        {
            Matrix result = new Matrix(this.nr_cols, this.nr_rows);
            for (int i = 1; i <= this.nr_vals; i++)
            {
                result[i] = Math.Round(this[i], numberOfDecimals);
                //result[i] = Math.Truncate(this[i] * Math.Pow(10, numberOfDecimals)) / Math.Pow(10, numberOfDecimals);
            }

            return result;
        }

        /// CHECKED
        /// <summary></summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public Matrix getNormal()
        {
            double std = this.getStandardDeviation();
            double mean = this.getMean();

            if (std == 0)
                throw new Exception("Standard deviation is 0");

            return (this - mean) / std;
        }

        /// CHECKED
        /// <summary></summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public double getStandardDeviation()
        {
            if (this.nr_vals - 1 == 0)
                throw new Exception("Division by zero");

            double result = (this - this.getMean()).powerMatrix(2).getMatrixSum() / (double)(this.nr_vals - 1);
            return Math.Sqrt(result);
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public Matrix getSubMatrix(int x, int y, int w, int h)
        {
            if (h < 1 || w < 1 || this.nr_cols < x + w - 1 || this.nr_rows < y + h - 1)
            {
                throw new Exception("Invalid input - width, height, x, y, w, h " +
                    this.nr_cols + ", " + this.nr_rows + ", " +
                    x + ", " + y + ", " + w + ", " + h);
            }

            Matrix sub_matrix = new Matrix(w, h);

            int xx, yy;
            for (xx = x; xx < x + w; xx++)
            {
                for (yy = y; yy < y + h; yy++)
                {
                    sub_matrix[xx - x + 1, yy - y + 1] = this[xx, yy];
                }
            }

            return sub_matrix;
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double getSum()
        {
            return data.Sum();
        }

        /// CHECKED
        /// <summary></summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public double getMean()
        {
            if (this.nr_vals == 0)
                throw new Exception("Division by zero");

            return this.getMatrixSum() / (double)this.nr_vals;
        }

        /// CHECKED
        /// <summary></summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public double getMaxValue()
        {
            return this.data.Max();
        }

        /// CHECKED
        /// <summary></summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public double getMinValue()
        {
            return this.data.Min();
        }

        /// CHECKED
        /// <summary></summary>
        /// <returns></returns>
        public double getMatrixSum()
        {
            return this.data.Sum();
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public Matrix getRow(int row)
        {
            row -= 1;

            if (row < 0 || row >= this.nr_rows)
                throw new Exception();

            int startIndex = row * this.nr_cols;
            int copyLenght = this.nr_cols;

            double[] result = new double[copyLenght];
            Array.Copy(this.data, startIndex, result, 0, copyLenght);

            return new Matrix(this.nr_cols, 1, result);
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="value"></param>
        public void setRow(int row, Matrix value)
        {
            row -= 1;

            if (row < 0 || row >= this.nr_rows)
                throw new Exception();

            int startIndex = row * this.nr_cols;
            int copyLenght = this.nr_cols;

            Array.Copy(value.data, 0, this.data, startIndex, copyLenght);
        }

        /// CHECKED
        /// <summary></summary>
        /// <param name="row_start"></param>
        /// <param name="row_end"></param>
        /// <returns></returns>
        public Matrix getRows(int row_start, int row_end)
        {
            row_start -= 1;
            row_end -= 1;

            if (row_start >= row_end || row_start < 0 || row_end >= this.nr_rows)
                throw new Exception("Illegal arguments");

            int startIndex = row_start * this.nr_cols;
            int copyLenght = (row_end - row_start + 1) * this.nr_cols;

            double[] result = new double[copyLenght];
            Array.Copy(this.data, startIndex, result, 0, copyLenght);

            return new Matrix(this.nr_cols, row_end - row_start + 1, result);
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row_start"></param>
        /// <param name="row_end"></param>
        /// <param name="value"></param>
        public void setRows(int row_start, int row_end, Matrix value)
        {
            row_start -= 1;
            row_end -= 1;

            if (row_start >= row_end || row_start < 0 || row_end >= this.nr_rows)
                throw new Exception("Illegal arguments");

            int startIndex = row_start * this.nr_cols;
            int copyLenght = (row_end - row_start + 1) * this.nr_cols;

            Array.Copy(value.data, 0, this.data, startIndex, copyLenght);
        }

        #endregion

        #region MANIPULATORS

        /// CHECKED
        /// <summary></summary>
        /// <param name="m"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public Matrix powerMatrix(double val)
        {
            double[] result = new double[this.nr_vals];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Pow(this.data[i], val);
            }

            return new Matrix(this.nr_cols, this.nr_rows, result);
        }

        /// CHECKED
        /// <summary></summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public Matrix dotMultiplication(Matrix m)
        {
            if (this.nr_cols != m.nr_cols || this.nr_rows != m.nr_rows)
                throw new Exception("Unmatching dimensions");

            double[] data = new double[this.nr_vals];
            for (int i = 0; i < this.nr_vals; i++)
            {
                data[i] = this.data[i] * m.data[i];
            }

            return new Matrix(this.nr_cols, m.nr_rows, data);
        }

        /// CHECKED
        /// <summary></summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public Matrix dotDivision(Matrix m)
        {
            if (this.nr_cols != m.nr_cols || this.nr_rows != m.nr_rows)
                throw new Exception("Unmatching dimensions");

            double[] data = new double[this.nr_vals];
            for (int i = 0; i < this.nr_vals; i++)
            {
                data[i] = this.data[i] / m.data[i];
            }

            return new Matrix(this.nr_cols, this.nr_rows, data);
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nr_decimals"></param>
        /// <returns></returns>
        public string ToString(int nr_decimals)
        {
            Matrix result = this.getRoundedMatrix(nr_decimals);
            return result.ToString();
        }

        /// CHECKED
        /// <summary></summary>
        /// <returns></returns>
        public override string ToString()
        {
            int row, col;
            string temp;

            StringBuilder result = new StringBuilder();

            result.Append("nr_rows = " + nr_rows + " , nr_cols = " + nr_cols + "\r\n");

            for (row = 1; row <= this.nr_rows; row++)
            {
                for (col = 1; col <= this.nr_cols; col++)
                {
                    temp = this[col, row].ToString().Replace(',', '.') + "\t";
                    result.Append(temp);
                }
                result.Append("\r\n");
            }

            return result.ToString();
        }

        /// CHECKED
        /// <summary></summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double this[int index]
        {
            get
            {
                index -= 1;

                if (index >= nr_vals || index < 0)
                    throw new Exception();

                return data[index];
            }

            set
            {
                index -= 1;

                if (index >= nr_vals || index < 0)
                    throw new Exception();

                data[index] = value;
            }
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public double this[int col, int row]
        {
            get
            {
                col -= 1;
                row -= 1;

                if (col < 0 || col >= this.nr_cols ||
                    row < 0 || row >= this.nr_rows)
                    throw new Exception();

                return data[row * this.nr_cols + col];
            }
            set
            {
                col -= 1;
                row -= 1;

                if (col < 0 || col >= this.nr_cols ||
                    row < 0 || row >= this.nr_rows)
                    throw new Exception();

                data[row * this.nr_cols + col] = value;
            }
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startCol"></param>
        /// <param name="startRow"></param>
        /// <param name="endCol"></param>
        /// <param name="endRow"></param>
        /// <returns></returns>
        public Matrix this[int startCol, int startRow, int endCol, int endRow]
        {
            get
            {
                startCol -= 1;
                startRow -= 1;
                endCol -= 1;
                endRow -= 1;

                if (startCol < 0 || startCol >= nr_cols ||
                    startRow < 0 || startRow >= nr_rows ||
                    endCol < 0 || endCol >= nr_cols ||
                    endRow < 0 || endRow >= nr_rows)
                    throw new Exception();

                Matrix result = new Matrix(endCol - startCol + 1, endRow - startRow + 1);
                for (int col = startCol + 1; col <= endCol + 1; col++)
                {
                    for (int row = startRow + 1; row <= endRow + 1; row++)
                    {
                        result[col - startCol, row - startRow] = this[col, row];
                    }
                }

                return result;
            }

            set
            {
                startCol -= 1;
                startRow -= 1;
                endCol -= 1;
                endRow -= 1;

                if (startCol < 0 || startCol >= nr_cols ||
                    startRow < 0 || startRow >= nr_rows ||
                    endCol < 0 || endCol >= nr_cols ||
                    endRow < 0 || endRow >= nr_rows)
                    throw new Exception();

                for (int col = startCol + 1; col <= endCol + 1; col++)
                {
                    for (int row = startRow + 1; row <= endRow + 1; row++)
                    {
                        this[col, row] = value[col - startCol, row - startRow];
                    }
                }
            }
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public Matrix removeRow(int row)
        {
            if (row > nr_rows)
                throw new Exception();

            Matrix result = new Matrix(nr_cols, nr_rows - 1);

            if (row > 1)
                result[1, 1, nr_cols, row - 1] = this[1, 1, nr_cols, row - 1];
            if (row < nr_rows)
                result[1, row, nr_cols, nr_rows - 1] = this[1, row + 1, nr_cols, nr_rows];

            return result;
        }

        #endregion

        #region OPERATORS

        /// CHECKED
        /// <summary></summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (m1.nr_cols != m2.nr_cols ||
                m1.nr_rows != m2.nr_rows)
            {
                throw new Exception("Unmatching dimensions");
            }

            double[] data = new double[m1.nr_vals];
            for (int i = 0; i < m1.nr_vals; i++)
            {
                data[i] = m1.data[i] + m2.data[i];
            }

            return new Matrix(m1.nr_cols, m1.nr_rows, data);
        }

        /// CHECKED
        /// <summary>Sum the entire Matrix with a value</summary>
        /// <param name="m"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Matrix operator +(Matrix m, double val)
        {
            Matrix result = new Matrix(m.nr_cols, m.nr_rows, m.data);

            if (val == 0)
                return result;

            for (int i = 1; i <= m.nr_vals; i++)
            {
                result[i] += val;
            }

            return result;
        }

        /// CHECKED
        /// <summary></summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.nr_cols != m2.nr_cols || m1.nr_rows != m2.nr_rows)
                throw new Exception("Unmatching dimensions");

            double[] data = new double[m1.nr_vals];
            for (int i = 0; i < m1.nr_vals; i++)
            {
                data[i] = m1.data[i] - m2.data[i];
            }

            return new Matrix(m1.nr_cols, m1.nr_rows, data);
        }

        /// CHECKED
        /// <summary></summary>
        /// <param name="m"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Matrix operator -(Matrix m, double val)
        {
            Matrix result = new Matrix(m.nr_cols, m.nr_rows, m.data);

            if (val == 0)
                return result;

            for (int i = 1; i <= m.nr_vals; i++)
            {
                result[i] -= val;
            }

            return result;
        }

        /// CHECKED
        /// <summary></summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            int i, j, k;

            if (m1.nr_cols != m2.nr_rows)
                throw new Exception("Unmatching dimensions");

            Matrix resultMatrix = new Matrix(m2.nr_cols, m1.nr_rows);

            for (i = 1; i <= m1.nr_rows; i++)
            {
                for (j = 1; j <= m2.nr_cols; j++)
                {
                    double sum = 0;
                    for (k = 1; k <= m1.nr_cols; k++)
                    {
                        sum += m1[k, i] * m2[j, k];
                    }
                    resultMatrix[j, i] = sum;
                }
            }

            return resultMatrix;
        }

        /// CHECKED
        /// <summary></summary>
        /// <param name="m"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix m, double val)
        {

            if (val == 0)
                return new Matrix(m.nr_cols, m.nr_rows, new double[m.nr_vals]);

            Matrix result = new Matrix(m.nr_cols, m.nr_rows, m.data);

            if (val == 1)
                return result;

            for (int i = 1; i <= m.nr_vals; i++)
            {
                result[i] *= val;
            }

            return result;
        }

        /// CHECKED
        /// <summary></summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Matrix operator /(Matrix m1, Matrix m2)
        {
            int i, j, k;

            if (m1.nr_cols != m2.nr_rows)
                throw new Exception("Unmatching dimensions");

            Matrix resultMatrix = new Matrix(m2.nr_cols, m1.nr_rows);

            for (i = 1; i <= m1.nr_rows; i++)
            {
                for (j = 1; j <= m2.nr_cols; j++)
                {
                    double sum = 0;
                    for (k = 1; k <= m1.nr_cols; k++)
                    {
                        sum += m1[k, i] / m2[j, k];
                    }
                    resultMatrix[j, i] = sum;
                }
            }

            return resultMatrix;
        }

        /// CHECKED
        /// <summary></summary>
        /// <param name="m"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Matrix operator /(Matrix m, double val)
        {
            if (val == 0)
                throw new Exception("Division by zero");

            Matrix result = new Matrix(m.nr_cols, m.nr_rows, m.data);

            if (val == 1)
                return result;

            for (int i = 1; i <= m.nr_vals; i++)
            {
                result[i] /= val;
            }

            return result;
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static bool operator ==(Matrix m1, Matrix m2)
        {
            return m1.Equals(m2);
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static bool operator !=(Matrix m1, Matrix m2)
        {
            return !(m1 == m2);
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Matrix m;
            if (obj is Matrix) { m = obj as Matrix; }
            else { return false; }

            bool cols = this.nr_cols == m.nr_cols;
            bool rows = this.nr_rows == m.nr_rows;
            bool vals = this.nr_vals == m.nr_vals;

            if (!(cols && rows && vals))
                return false;

            bool data = true;
            for (int i = 1; i <= this.nr_vals; i++)
            {
                if (this[i] != m[i])
                {
                    data = false;
                    break;
                }
            }

            return data;
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrixList1"></param>
        /// <param name="matrixList2"></param>
        /// <returns></returns>
        public static bool EqualsList(Matrix[] matrixList1, Matrix[] matrixList2)
        {
            if (matrixList1.Length != matrixList2.Length) { return false; }

            for (int i = 0; i < matrixList1.Length; i++)
            {
                if (!matrixList1[i].Equals(matrixList2[i])) { return false; }
            }

            return true;
        }

        #endregion

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        public void print()
        {
            for (int x = 1; x <= this.nr_cols; x++)
            {
                for (int y = 1; y <= this.nr_rows; y++)
                {
                    Console.Write(this[x, y] + " ");
                }
                Console.WriteLine();
            }
        }

        /// CHECKED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public void saveToXML(string fileName)
        {
            Matrix.SerializeToXML(this, fileName + ".xml");
        }

    }
}

