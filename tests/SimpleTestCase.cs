﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SAP.Middleware.Connector;
using System;

namespace SharpSapRfc.Test
{
    [TestClass]
    public class SimpleTestCase
    {
        [TestMethod]
        public void ExportSingleParameterTest()
        {
            using (SapRfcConnection conn = new SapRfcConnection("TST"))
            {
                var result = conn.ExecuteFunction("Z_SSRT_SUM", 
                    new RfcParameter("i_num1", 2),
                    new RfcParameter("i_num2", 4)
                );

                var total = result.GetOutput<int>("e_result");
                Assert.AreEqual(6, total);
            }
        }

        [TestMethod]
        public void ExportSingleParameterTest_WithAnonymousType()
        {
            using (SapRfcConnection conn = new SapRfcConnection("TST"))
            {
                var result = conn.ExecuteFunction("Z_SSRT_SUM", new
                {
                    i_num1 = 2,
                    i_num2 = 7
                });

                var total = result.GetOutput<int>("e_result");
                Assert.AreEqual(9, total);
            }
        }

        [TestMethod]
        public void ChangingSingleParameterTest()
        {
            using (SapRfcConnection conn = new SapRfcConnection("TST"))
            {
                var result = conn.ExecuteFunction("Z_SSRT_ADD",
                    new RfcParameter("i_add", 4),
                    new RfcParameter("c_num", 4)
                );

                var total = result.GetOutput<int>("c_num");
                Assert.AreEqual(8, total);
            }
        }

        [TestMethod]
        public void ExportMultipleParametersTest()
        {
            using (SapRfcConnection conn = new SapRfcConnection("TST"))
            {
                var result = conn.ExecuteFunction("Z_SSRT_DIVIDE", 
                    new RfcParameter("i_num1", 5),
                    new RfcParameter("i_num2", 2)
                );

                var quotient = result.GetOutput<decimal>("e_quotient");
                var remainder = result.GetOutput<int>("e_remainder");
                Assert.AreEqual(2.5m, quotient);
                Assert.AreEqual(1, remainder);
            }
        }

        [TestMethod]
        public void AllTypesInOutTest()
        {
            using (SapRfcConnection conn = new SapRfcConnection("TST"))
            {
                var result = conn.ExecuteFunction("Z_SSRT_IN_OUT", new
                {
                    I_ID = 2,
                    I_PRICE = 464624.521,
                    I_DATUM = new DateTime(2014, 4, 6),
                    I_UZEIT = new DateTime(1, 1, 1, 12, 10, 53),
                    i_active = true
                });

                Assert.AreEqual(2, result.GetOutput<int>("E_ID"));
                Assert.AreEqual(464624.521m, result.GetOutput<decimal>("E_PRICE"));
                Assert.AreEqual(new DateTime(2014, 4, 6), result.GetOutput<DateTime>("E_DATUM"));
                Assert.AreEqual(new DateTime(1, 1, 1, 12, 10, 53), result.GetOutput<DateTime>("E_UZEIT"));
                Assert.AreEqual(true, result.GetOutput<bool>("e_active"));
            }
        }

        [TestMethod]
        public void ExceptionTest()
        {
            using (SapRfcConnection conn = new SapRfcConnection("TST"))
            {
                try
                {
                    var result = conn.ExecuteFunction("Z_SSRT_DIVIDE", 
                        new RfcParameter("i_num1", 5),
                        new RfcParameter("i_num2", 0)
                    );
                    Assert.Fail();
                }
                catch (RfcAbapException ex)
                {
                    Assert.AreEqual("DIVIDE_BY_ZERO", ex.Key);
                }
            }
        }
    }
}
