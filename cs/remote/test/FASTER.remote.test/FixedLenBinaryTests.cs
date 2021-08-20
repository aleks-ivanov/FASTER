﻿using NUnit.Framework;

namespace FASTER.remote.test
{
    [TestFixture]
    public class FixedLenBinaryTests
    {
        FixedLenServer<long, long> server;
        FixedLenClient<long, long> client;

        [SetUp]
        public void Setup()
        {
            server = new FixedLenServer<long, long>(TestContext.CurrentContext.TestDirectory + "/FixedLenBinaryTests", (a, b) => a + b, enablePubSub: false);
            client = new FixedLenClient<long, long>();
        }

        [TearDown]
        public void TearDown()
        {
            client.Dispose();
            server.Dispose();
        }

        [Test]
        public void UpsertReadRMWTest()
        {
            using var session = client.GetSession();
            session.Upsert(10, 23);
            session.CompletePending();
            session.Read(10, userContext: 23);
            session.CompletePending();
            session.RMW(20, 23);
            session.RMW(20, 23);
            session.RMW(20, 23);
            session.CompletePending();
            session.Read(20, userContext: 23 * 3);
            session.CompletePending(true);
        }
    }
}
