using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GR.Utilities;

namespace NUnit.CoreUnitTests
{
    [TestFixture]
    public class DateTimeUtiltiyTests
    {
        [Test]
        public void DateTimeUtility_Min()
        {
            DateTime dt1, dt2;

            //dt2 one day later
            dt1 = new DateTime(2000, 1, 1);
            dt2 = new DateTime(2000, 1, 2);
            Assert.AreEqual(dt1, DateTimeUtility.Min(dt1, dt2));

            //dt2 one millisecond later
            dt2 = new DateTime(2000, 1, 1, 0, 0, 0, 1);
            Assert.AreEqual(dt1, DateTimeUtility.Min(dt1, dt2));

            //dt1 one day later
            dt1 = new DateTime(2000, 1, 2);
            dt2 = new DateTime(2000, 1, 1);
            Assert.AreEqual(dt2, DateTimeUtility.Min(dt1, dt2));

            //dt1 one millisecond later
            dt1 = new DateTime(2000, 1, 1, 0, 0, 0, 1);
            Assert.AreEqual(dt2, DateTimeUtility.Min(dt1, dt2));
        }

        [Test]
        public void DateTimeUtility_MinNull()
        {
            DateTime? dt1, dt2;

            //dt2 one day later
            dt1 = new DateTime(2000, 1, 1);
            dt2 = new DateTime(2000, 1, 2);
            Assert.AreEqual(dt1, DateTimeUtility.Min(dt1, dt2));

            //dt2 one millisecond later
            dt2 = new DateTime(2000, 1, 1, 0, 0, 0, 1);
            Assert.AreEqual(dt1, DateTimeUtility.Min(dt1, dt2));

            //dt1 one day later
            dt1 = new DateTime(2000, 1, 2);
            dt2 = new DateTime(2000, 1, 1);
            Assert.AreEqual(dt2, DateTimeUtility.Min(dt1, dt2));

            //dt1 one millisecond later
            dt1 = new DateTime(2000, 1, 1, 0, 0, 0, 1);
            Assert.AreEqual(dt2, DateTimeUtility.Min(dt1, dt2));

            //dt2 null
            dt1 = new DateTime(2000, 1, 1);
            dt2 = null;
            Assert.AreEqual(dt2, DateTimeUtility.Min(dt1, dt2));

            //dt1 null
            dt1 = null;
            dt2 = new DateTime(2000, 1, 1);
            Assert.AreEqual(dt1, DateTimeUtility.Min(dt1, dt2));

            //dt1 and dt2 null
            dt1 = null;
            dt2 = null;
            Assert.AreEqual(dt1, DateTimeUtility.Min(dt1, dt2));
        }

        [Test]
        public void DateTimeUtility_Max()
        {
            DateTime dt1, dt2;

            //dt2 one day later
            dt1 = new DateTime(2000, 1, 1);
            dt2 = new DateTime(2000, 1, 2);
            Assert.AreEqual(dt2, DateTimeUtility.Max(dt1, dt2));

            //dt2 one millisecond later
            dt2 = new DateTime(2000, 1, 1, 0, 0, 0, 1);
            Assert.AreEqual(dt2, DateTimeUtility.Max(dt1, dt2));

            //dt1 one day later
            dt1 = new DateTime(2000, 1, 2);
            dt2 = new DateTime(2000, 1, 1);
            Assert.AreEqual(dt1, DateTimeUtility.Max(dt1, dt2));

            //dt1 one millisecond later
            dt1 = new DateTime(2000, 1, 1, 0, 0, 0, 1);
            Assert.AreEqual(dt1, DateTimeUtility.Max(dt1, dt2));
        }

        [Test]
        public void DateTimeUtility_MaxNull()
        {
            DateTime? dt1, dt2;

            //dt2 one day later
            dt1 = new DateTime(2000, 1, 1);
            dt2 = new DateTime(2000, 1, 2);
            Assert.AreEqual(dt2, DateTimeUtility.Max(dt1, dt2));

            //dt2 one millisecond later
            dt2 = new DateTime(2000, 1, 1, 0, 0, 0, 1);
            Assert.AreEqual(dt2, DateTimeUtility.Max(dt1, dt2));

            //dt1 one day later
            dt1 = new DateTime(2000, 1, 2);
            dt2 = new DateTime(2000, 1, 1);
            Assert.AreEqual(dt1, DateTimeUtility.Max(dt1, dt2));

            //dt1 one millisecond later
            dt1 = new DateTime(2000, 1, 1, 0, 0, 0, 1);
            Assert.AreEqual(dt1, DateTimeUtility.Max(dt1, dt2));

            //dt2 null
            dt1 = new DateTime(2000, 1, 1);
            dt2 = null;
            Assert.AreEqual(dt2, DateTimeUtility.Max(dt1, dt2));

            //dt1 null
            dt1 = null;
            dt2 = new DateTime(2000, 1, 1);
            Assert.AreEqual(dt1, DateTimeUtility.Max(dt1, dt2));

            //dt1 and dt2 null
            dt1 = null;
            dt2 = null;
            Assert.AreEqual(dt1, DateTimeUtility.Max(dt1, dt2));
        }
    }
}
