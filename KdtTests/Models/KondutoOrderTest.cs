﻿using KdtSdk.Exceptions;
using KdtSdk.Models;
using KdtSdk.Utils;
using KdtTests.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace KdtTests.Models
{
    [TestClass]
    public class KondutoOrderTest
    {
        [TestMethod]
        public void IsValidTest()
        {
            KondutoOrder order = new KondutoOrder();
            Assert.IsFalse(order.IsValid(), "order should be invalid without id");

            order.Id = "order1";
            Assert.IsFalse(order.IsValid(), "order should be invalid without total amount");

            order.TotalAmount = 120.1;
            Assert.IsFalse(order.IsValid(), "order should be invalid without customer");

            order.Customer = KondutoCustomerFactory.BasicCustomer();
            Assert.IsTrue(order.IsValid(), "order should be valid");
            Assert.IsTrue(order.GetError() == null, "order errors should be empty");
        }

        [TestMethod]
        public void SerializationTest()
        {
            KondutoOrder order = KondutoOrderFactory.completeOrder();
            String orderJSON = KondutoUtils.LoadJson<KondutoOrder>(Properties.Resources.order).ToJson();

            try
            {
                Assert.AreEqual(orderJSON, order.ToJson(), "serialization failed");
            }
            catch (KondutoInvalidEntityException e)
            {
                Assert.Fail("order should be valid");
            }

            KondutoOrder deserializedOrder = KondutoModel.FromJson<KondutoOrder>(orderJSON);
            Assert.IsTrue(order.Equals(deserializedOrder), "deserialization failed");

        }

        [TestMethod]
        public void SerializationTestWithShoppingAndFlight()
        {
            KondutoOrder order = KondutoOrderFactory.completeOrder();
            order.Travel = KondutoFlightFactory.CreateFlight();

            try
            {
                order.ToJson();
                Assert.Fail("order should be invalid");
            }
            catch (KondutoInvalidEntityException e)
            {
                //ok
            }

            order = KondutoOrderFactory.completeOrder();
            order.Travel = KondutoFlightFactory.CreateFlight();
            order.ShoppingCart = null;
            //ok
        }

        [TestMethod]
        public void SerializationTestWithSeller()
        {
            KondutoOrder order = KondutoOrderFactory.completeOrder();
            order.Seller = KondutoSellerFactory.Create();

            try
            {
                order.ToJson();
                //ok
            }
            catch (KondutoInvalidEntityException e)
            {

                Assert.Fail("order should be invalid");
            }
        }

        [TestMethod]
        public void SerializationTestWithBureauxQueries()
        {
            KondutoOrder order = KondutoOrderFactory.completeOrder();
            order.BureauxQueries = KondutoBureauxQueriesFactory.Create();

            try
            {
                order.ToJson();
                //ok
            }
            catch (KondutoInvalidEntityException e)
            {

                Assert.Fail("order should be invalid");
            }
        }

        [TestMethod]
        public void SerializationTestWithTriggeredRules()
        {
            KondutoOrder order = KondutoOrderFactory.completeOrder();
            order.TriggeredRules = KondutoTriggeredRulesFactory.Create();

            try
            {
                order.ToJson();
                //ok
            }
            catch (KondutoInvalidEntityException e)
            {

                Assert.Fail("order should be invalid");
            }
        }

        [TestMethod]
        public void SerializationTestWithTriggeredDecisionList()
        {
            KondutoOrder order = KondutoOrderFactory.completeOrder();
            order.TriggeredDecisionList = KondutoTriggeredDecisionListFactory.Create();

            try
            {
                order.ToJson();
                //ok
            }
            catch (KondutoInvalidEntityException e)
            {

                Assert.Fail("order should be invalid");
            }
        }

        [TestMethod, ExpectedException(typeof(KondutoInvalidEntityException))]
        public void invalidOrderSerializationThrowsExceptionTest()
        {
            KondutoOrder order = new KondutoOrder();
            order.ToJson(); // triggers invalid customer exception
        }
    }
}
