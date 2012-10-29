using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViolaJonesCS.Utilities;


namespace TestProject3
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class MatrixTest
    {
        public MatrixTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestConstructor1()
        {
            Matrix m001 = new Matrix(2, 3);
            Matrix m002 = new Matrix(2, 3, new double[6] { 0, 0, 0, 0, 0, 0 });
            Matrix m003 = new Matrix(2, 3, new double[6] { 0, 0, 0, 0, 0, 1 });

            

            Assert.AreEqual(m001, m002);
            Assert.AreNotEqual(m001, m003);
        }

        [TestMethod]
        public void TestConstructor2()
        {
            Matrix m001 = new Matrix(2, 3);
            Matrix m002 = new Matrix(m001);
            
            Assert.AreEqual(m001 ,m002);
        }

        [TestMethod]
        public void TestEqualOperator()
        {
            Matrix m001 = new Matrix(2, 3);
            Matrix m002 = new Matrix(m001);
            Matrix m003 = new Matrix(2, 3, new double[6] { 0, 0, 0, 0, 0, 1 });

            Assert.IsTrue(m001 == m002);
            Assert.IsTrue(m001 != m003);
        }

        [TestMethod]
        public void TestPlusMatrix()
        {
            Matrix m001 = new Matrix(2, 3);
            Matrix m002 = new Matrix(2, 3, new double[6] { 1, 2, 3, 4, 5, 6 });
            m001 += m002;
            
            Assert.AreEqual(m001, m002);
        }

        [TestMethod]
        public void TestMinusMatrix()
        {
            Matrix m001 = new Matrix(2, 3, new double[6] { 1, 2, 3, 4, 5, 6 });
            Matrix m002 = new Matrix(2, 3, new double[6] { 1, 2, 3, 4, 5, 6 });
            m001 += m002;
            m001 -= m002;
            
            Assert.AreEqual(m001, m002);
        }

        [TestMethod]
        public void TestPlusNumeric()
        {
            Matrix m001 = new Matrix(2, 3, new double[6] { 1, 2, 3, 4, 5, 6 });
            Matrix m002 = new Matrix(2, 3, new double[6] { 2, 3, 4, 5, 6, 7 });
            m001 += 1;
            
            Assert.AreEqual(m001 ,m002);
        }

        [TestMethod]
        public void TestMinusNumeric()
        {
            Matrix m001 = new Matrix(2, 3, new double[6] { 1, 2, 3, 4, 5, 6 });
            Matrix m002 = new Matrix(2, 3, new double[6] { 2, 3, 4, 5, 6, 7 });
            m002 -= 1;
            
            Assert.AreEqual(m001, m002);
        }

        [TestMethod]
        public void TestPower()
        {
            Matrix m001 = new Matrix(2, 3, new double[6] { 1, 2, 3, 4, 5, 6 });
            Matrix m002 = m001.dotMultiplication(m001);
            Matrix m003 = m001.powerMatrix(2);
            
            Assert.AreEqual(m002, m003);
        }

        [TestMethod]
        public void TestDotDivision()
        {
            Matrix m001 = new Matrix(2, 3, new double[6] { 1, 2, 3, 4, 5, 6 });
            Matrix m002 = m001.dotMultiplication(m001);
            Matrix m003 = m002.dotDivision(m001);
            
            Assert.AreEqual(m001, m003);
        }
        
        [TestMethod]
        public void TestGetIndex()
        {
            Matrix m001 = new Matrix(2, 3, new double[6] { 1, 2, 3, 4, 5, 6 });
            
            Assert.AreEqual(m001[1], m001[1, 1]);
            Assert.AreEqual(m001[4], m001[2, 2]);
            Assert.AreEqual(m001[6], m001[2, 3]);
        }
        
        [TestMethod]
        public void TestGetRows()
        {
            Matrix m001 = new Matrix(2, 3, new double[6] { 1, 2, 3, 4, 5, 6 });
            Matrix m002 = m001.getRows(2, 3);
            Matrix m003 = new Matrix(2, 2, new double[4] { 3, 4, 5, 6 });
            
            Assert.AreEqual(m002, m003);
        }

        [TestMethod]
        public void TestGetRow()
        {
            Matrix m001 = new Matrix(2, 3, new double[6] { 1, 2, 3, 4, 5, 6 });
            Matrix m003 = m001.getRow(2);
            Matrix m004 = m001.getRow(3);
            Matrix m005 = new Matrix(2, 1, new double[2] { 3, 4 });
            Matrix m006 = new Matrix(2, 1, new double[2] { 5, 6 });
            
            Assert.AreEqual(m003, m005);
            Assert.AreEqual(m004, m006);
        }

        [TestMethod]
        public void TestStandarddeviation()
        {
            Matrix image = new Matrix(19, 19, new double[361]
                    {-0.4068,  0.2601,  1.0011,  1.6309,  1.7421,  1.5939,  1.9273,  2.2978,  2.5942,  2.5942,  2.2237,  1.2234,  0.8529,  0.0007, -0.2957,  0.2971, -0.4068, -0.5180, -1.1478,
                     -0.9255, -0.4809,  0.0378,  0.1860, -0.4068, -0.7032,  0.1860,  0.4453,  1.0381,  1.7421,  1.2975, -1.1478, -1.1849, -0.7403, -1.2960, -1.7406, -1.6295, -1.3701, -0.8885,
                     -0.8885, -0.9255, -1.2590, -0.4809, -1.2590, -1.5183, -1.0367,  0.1489, -0.1475,  2.2978,  0.0748, -1.2960, -0.6291, -0.9255, -1.0737, -0.8144, -1.2960, -0.8885, -1.3331,
                     -1.1108, -1.5554, -1.8148,  1.0752, -1.3331, -0.3698, -0.4809, -0.5180, -0.3327,  1.1493, -0.0363, -0.2216,  0.7047,  2.7424, -1.6665, -2.2594, -1.8148, -1.2960, -0.9626,
                     -0.6662, -1.9259, -0.9996, -0.5550, -0.2586, -0.1845, -0.9255, -0.7403, -0.6662,  1.1493,  0.1860, -1.1849, -0.5180,  0.5194, -0.5921, -0.8514, -1.4442, -1.6295, -0.8514,
                     -0.5921, -0.7773, -1.1108, -0.7032, -0.2586, -0.3698, -0.7032, -0.1475, -0.7032,  1.1863,  0.3342, -1.5183, -1.1849, -0.7032, -0.4439, -0.4068, -0.5550, -0.7403, -0.7403,
                      0.5194, -0.3698, -0.0363,  0.3712,  0.2601,  0.4824,  0.1489,  0.5565, -0.6291,  0.8529, -0.1475, -0.7773, -0.4809,  0.2971,  0.7047,  0.4083,  0.5565, -0.4439, -0.4439,
                      1.0752,  0.0378,  0.6676,  1.1493,  1.5568,  1.4827,  0.2601,  0.2971, -0.6662,  0.9270,  0.1489, -0.9996, -0.0734,  0.2971,  1.2234,  1.0381,  0.7047, -0.0734,  0.2230,
                      0.7417,  1.2234,  0.8158,  1.5568,  1.5939,  1.5198,  0.6676, -0.0363, -0.7032,  1.5568,  0.4453, -1.2219,  0.1489,  0.9640,  1.2975,  1.7421,  1.4827,  0.6676,  0.4824,
                      0.5194,  0.7047,  1.1493,  1.6309,  1.8532,  2.3719,  1.3716, -0.4439, -0.0734,  2.2237,  0.5194, -0.9255,  0.4453,  1.8162,  1.7791,  1.5198,  1.3345,  0.7417,  0.4824,
                      0.1860,  0.8158,  0.7417,  1.1122,  1.3716,  2.4831,  1.7050, -0.8144, -0.0363,  2.4831,  1.2604, -0.9255,  0.0378,  2.5201,  1.6680,  1.4457,  1.0381,  0.4824,  0.0748,
                      0.0378,  0.4824,  0.6306,  0.8158,  1.0381,  1.5568,  1.5939, -0.7773, -1.2590,  0.1860, -0.4068, -1.1108, -0.2216,  1.2604,  1.8162,  1.4086,  0.7417,  0.5194,  0.1489,
                      0.2601,  0.5565,  0.5194,  1.0381,  1.5568,  1.2975,  0.8529, -0.3327, -0.8144, -0.3327, -0.2216, -1.1849, -0.6662, -0.0734,  0.7047,  1.5198,  0.8899,  0.6306,  0.1489,
                     -0.1845,  0.3712,  0.4083,  0.8899,  0.7047, -0.1845, -0.8514, -0.7032, -0.0734,  0.1860,  0.1489, -0.2957, -0.8885, -0.8514, -0.7032, -0.2216,  0.5935,  0.4083, -0.2216,
                     -0.6662,  0.1489,  0.5565,  0.4453, -0.5550, -0.8144, -0.2957,  0.3342,  0.9640,  1.0011,  0.7417,  0.3712, -0.4068, -0.5550, -1.1108, -0.7403,  0.0007,  0.5194, -0.7032,
                     -0.9255, -0.1475,  0.4083, -0.1104, -0.6291, -1.1108, -1.5554, -1.3701, -0.8514, -0.2957, -0.8514, -1.4813, -1.8889, -2.0000, -1.5924, -0.9255,  0.3712,  0.2971, -1.1478,
                     -0.7403, -0.8144,  0.2230,  0.2230, -0.8885, -1.0737, -1.5183, -1.7777, -1.1849, -0.9255, -0.9996, -1.0737, -1.0737,  0.0378, -0.1104, -0.3698,  1.0381,  0.0748, -1.4813,
                     -0.9255, -1.1849, -0.1104,  0.4453, -0.3698, -0.2586, -0.1104, -0.2586,  0.2230,  0.2601,  0.0007,  0.0748, -0.5180,  0.3712,  0.0748, -0.2216,  0.6676, -0.4439, -1.5183,
                     -1.4813, -0.9996, -0.9255, -0.7773, -0.2216,  0.6306,  0.2971,  0.0007, -0.0734, -0.2586, -0.1475, -0.4439,  0.2971, -0.1845,  0.1860, -0.4809, -0.2216, -1.0737, -1.6295});

            Assert.AreEqual(1, Math.Round(image.getStandardDeviation()));
        }

        [TestMethod]
        public void TestMedian()
        {
            Matrix image = new Matrix(19, 19, new double[361]
                    {-0.4068,  0.2601,  1.0011,  1.6309,  1.7421,  1.5939,  1.9273,  2.2978,  2.5942,  2.5942,  2.2237,  1.2234,  0.8529,  0.0007, -0.2957,  0.2971, -0.4068, -0.5180, -1.1478,
                     -0.9255, -0.4809,  0.0378,  0.1860, -0.4068, -0.7032,  0.1860,  0.4453,  1.0381,  1.7421,  1.2975, -1.1478, -1.1849, -0.7403, -1.2960, -1.7406, -1.6295, -1.3701, -0.8885,
                     -0.8885, -0.9255, -1.2590, -0.4809, -1.2590, -1.5183, -1.0367,  0.1489, -0.1475,  2.2978,  0.0748, -1.2960, -0.6291, -0.9255, -1.0737, -0.8144, -1.2960, -0.8885, -1.3331,
                     -1.1108, -1.5554, -1.8148,  1.0752, -1.3331, -0.3698, -0.4809, -0.5180, -0.3327,  1.1493, -0.0363, -0.2216,  0.7047,  2.7424, -1.6665, -2.2594, -1.8148, -1.2960, -0.9626,
                     -0.6662, -1.9259, -0.9996, -0.5550, -0.2586, -0.1845, -0.9255, -0.7403, -0.6662,  1.1493,  0.1860, -1.1849, -0.5180,  0.5194, -0.5921, -0.8514, -1.4442, -1.6295, -0.8514,
                     -0.5921, -0.7773, -1.1108, -0.7032, -0.2586, -0.3698, -0.7032, -0.1475, -0.7032,  1.1863,  0.3342, -1.5183, -1.1849, -0.7032, -0.4439, -0.4068, -0.5550, -0.7403, -0.7403,
                      0.5194, -0.3698, -0.0363,  0.3712,  0.2601,  0.4824,  0.1489,  0.5565, -0.6291,  0.8529, -0.1475, -0.7773, -0.4809,  0.2971,  0.7047,  0.4083,  0.5565, -0.4439, -0.4439,
                      1.0752,  0.0378,  0.6676,  1.1493,  1.5568,  1.4827,  0.2601,  0.2971, -0.6662,  0.9270,  0.1489, -0.9996, -0.0734,  0.2971,  1.2234,  1.0381,  0.7047, -0.0734,  0.2230,
                      0.7417,  1.2234,  0.8158,  1.5568,  1.5939,  1.5198,  0.6676, -0.0363, -0.7032,  1.5568,  0.4453, -1.2219,  0.1489,  0.9640,  1.2975,  1.7421,  1.4827,  0.6676,  0.4824,
                      0.5194,  0.7047,  1.1493,  1.6309,  1.8532,  2.3719,  1.3716, -0.4439, -0.0734,  2.2237,  0.5194, -0.9255,  0.4453,  1.8162,  1.7791,  1.5198,  1.3345,  0.7417,  0.4824,
                      0.1860,  0.8158,  0.7417,  1.1122,  1.3716,  2.4831,  1.7050, -0.8144, -0.0363,  2.4831,  1.2604, -0.9255,  0.0378,  2.5201,  1.6680,  1.4457,  1.0381,  0.4824,  0.0748,
                      0.0378,  0.4824,  0.6306,  0.8158,  1.0381,  1.5568,  1.5939, -0.7773, -1.2590,  0.1860, -0.4068, -1.1108, -0.2216,  1.2604,  1.8162,  1.4086,  0.7417,  0.5194,  0.1489,
                      0.2601,  0.5565,  0.5194,  1.0381,  1.5568,  1.2975,  0.8529, -0.3327, -0.8144, -0.3327, -0.2216, -1.1849, -0.6662, -0.0734,  0.7047,  1.5198,  0.8899,  0.6306,  0.1489,
                     -0.1845,  0.3712,  0.4083,  0.8899,  0.7047, -0.1845, -0.8514, -0.7032, -0.0734,  0.1860,  0.1489, -0.2957, -0.8885, -0.8514, -0.7032, -0.2216,  0.5935,  0.4083, -0.2216,
                     -0.6662,  0.1489,  0.5565,  0.4453, -0.5550, -0.8144, -0.2957,  0.3342,  0.9640,  1.0011,  0.7417,  0.3712, -0.4068, -0.5550, -1.1108, -0.7403,  0.0007,  0.5194, -0.7032,
                     -0.9255, -0.1475,  0.4083, -0.1104, -0.6291, -1.1108, -1.5554, -1.3701, -0.8514, -0.2957, -0.8514, -1.4813, -1.8889, -2.0000, -1.5924, -0.9255,  0.3712,  0.2971, -1.1478,
                     -0.7403, -0.8144,  0.2230,  0.2230, -0.8885, -1.0737, -1.5183, -1.7777, -1.1849, -0.9255, -0.9996, -1.0737, -1.0737,  0.0378, -0.1104, -0.3698,  1.0381,  0.0748, -1.4813,
                     -0.9255, -1.1849, -0.1104,  0.4453, -0.3698, -0.2586, -0.1104, -0.2586,  0.2230,  0.2601,  0.0007,  0.0748, -0.5180,  0.3712,  0.0748, -0.2216,  0.6676, -0.4439, -1.5183,
                     -1.4813, -0.9996, -0.9255, -0.7773, -0.2216,  0.6306,  0.2971,  0.0007, -0.0734, -0.2586, -0.1475, -0.4439,  0.2971, -0.1845,  0.1860, -0.4809, -0.2216, -1.0737, -1.6295});

            Assert.AreEqual(0, Math.Round(image.getMean()));
        }

        [TestMethod]
        public void TestMultiplication()
        {
            Matrix m001 = new Matrix(2, 3, new double[6] { 1, 2, 3, 4, 5, 6 });
            Matrix m002 = new Matrix(2, 3, new double[6] { 2, 4, 6, 8, 10, 12 });
            Matrix m003 = m001 * 2;
            
            Assert.AreEqual(m002, m003);
        }

        [TestMethod]
        public void TestDivision()
        {
            Matrix m001 = new Matrix(2, 3, new double[6] { 1, 2, 3, 4, 5, 6 });
            Matrix m002 = new Matrix(2, 3, new double[6] { 2, 4, 6, 8, 10, 12 });
            Matrix m003 = m002 / 2;
            
            Assert.AreEqual(m001, m003);
        }

        [TestMethod]
        public void TestSetRow1()
        {
            Matrix m001 = new Matrix(2, 3, new double[6] { 1, 2, 3, 4, 5, 6 });
            Matrix m002 = new Matrix(2, 3, new double[6] { 5, 5, 3, 4, 5, 6 });
            m001.setRow(1, new Matrix(2, 1, new double[2] { 5, 5 }));
            
            Assert.AreEqual(m002, m001);
        }

        [TestMethod]
        public void TestSetRows2()
        {
            Matrix m001 = new Matrix(2, 3, new double[6] { 5, 5, 5, 5, 5, 5 });
            Matrix m002 = new Matrix(2, 3, new double[6] { 5, 5, 1, 2, 3, 4 });
            m001.setRows(2, 3, new Matrix(2, 2, new double[4] { 1, 2, 3, 4 }));
            
            Assert.AreEqual(m001, m002);
        }

        [TestMethod]
        public void TestMultiplicationMatrix()
        {
            Matrix m001 = new Matrix(3, 4, new double[12] { 14, 9, 3, 2, 11, 15, 0, 12, 17, 5, 2, 3 });
            Matrix m002 = new Matrix(2, 3, new double[6] { 12, 25, 9, 10, 8, 5 });
            Matrix m003 = new Matrix(2, 4, new double[8] { 273, 455, 243, 235, 244, 205, 102, 160 });
            
            Assert.AreEqual(m001 * m002, m003);
        }

        [TestMethod]
        public void TestGetSubMatrix()
        {
            Matrix m001 = new Matrix(2, 2, new double[4] { 9, 3, 11, 15 });
            Matrix m002 = new Matrix(3, 4, new double[12] { 14, 9, 3, 2, 11, 15, 0, 12, 17, 5, 2, 3 });
            
            Assert.AreEqual(m002[2, 1, 3, 2], m001);
        }

        [TestMethod]
        public void TestSetSubMatrix()
        {
            Matrix m001 = new Matrix(2, 2, new double[4] { 1, 2, 3, 4 });
            Matrix m002 = new Matrix(3, 4, new double[12] { 14, 9, 3, 2, 11, 15, 0, 12, 17, 5, 2, 3 });
            m002[2, 1, 3, 2] = m001;
            
            Assert.AreEqual(m002[2, 1, 3, 2], m001);
        }

        [TestMethod]
        public void TestRemoveRow()
        {
            Matrix m001 = new Matrix(3, 3, new double[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            Matrix m002 = new Matrix(3, 2, new double[6] { 4, 5, 6, 7, 8, 9 });
            Matrix m003 = new Matrix(3, 2, new double[6] { 1, 2, 3, 7, 8, 9 });
            Matrix m004 = new Matrix(3, 2, new double[6] { 1, 2, 3, 4, 5, 6 });
            
            Assert.AreEqual(m001.removeRow(1), m002);
            Assert.AreEqual(m001.removeRow(2), m003);
            Assert.AreEqual(m001.removeRow(3), m004);
        }

        [TestMethod]
        public void TestMergeMatrix()
        {
            Matrix m001 = new Matrix(3, 1, new double[3] { 1, 2, 3 });
            Matrix m002 = new Matrix(3, 2, new double[6] { 4, 5, 6, 7, 8, 9 });
            Matrix m003 = new Matrix(3, 3, new double[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            Matrix m004 = new Matrix(m001, m002);
            
            Assert.AreEqual(m003, m004);
        }

        [TestMethod]
        public void TestSerialize()
        {
            Matrix m001 = new Matrix(3, 3, new double[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            Matrix.SerializeToXML(m001, "testMatrix.xml");
            Matrix m002 = Matrix.DeserializeFromXML("testMatrix.xml");
            
            Assert.AreEqual(m001, m002);
        }

        [TestMethod]
        public void TestSerializeList()
        {
            Matrix m001 = new Matrix(3, 3, new double[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            Matrix m002 = new Matrix(1, 3, new double[3] { 4, 1, 3 });
            Matrix[] mList001 = new Matrix[2] { m001, m002 };
            Matrix.SerializeListToXML(mList001, "testMatrix.xml");
            Matrix[] mList002 = Matrix.DeserializeListFromXML("testMatrix.xml");

            Assert.IsTrue(Matrix.EqualsList(mList001, mList002));
        }

        [TestMethod]
        public void TestGetSubmatrix()
        {
            Matrix m001 = new Matrix(3, 3, new double[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            Matrix m002 = new Matrix(2, 3, new double[6] { 2, 3, 5, 6, 8, 9 });
            Matrix m003 = m001.getSubMatrix(2, 1, 2, 3);

            Assert.AreEqual(m002, m003);
        }

        [TestMethod]
        public void TestGetSum()
        {
            Matrix m001 = new Matrix(3, 3, new double[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

            Assert.AreEqual(m001.getSum(), 45);
            Assert.AreNotEqual(m001.getSum(), 0);
        }

        [TestMethod]
        public void TestEqualsList()
        {
            Matrix m001 = new Matrix(3, 3, new double[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            Matrix m002 = new Matrix(1, 2, new double[2] { 1, 2 });
            Matrix m003 = new Matrix(1, 2, new double[2] { 1, 3 });

            Matrix[] am001 = new Matrix[2] { m001, m002 };
            Matrix[] am002 = new Matrix[2] { m001, m002 };
            Matrix[] am003 = new Matrix[2] { m001, m003 };            

            Assert.IsTrue(Matrix.EqualsList(am001, am002));
            Assert.IsFalse(Matrix.EqualsList(am001, am003));
        }
    }
}
