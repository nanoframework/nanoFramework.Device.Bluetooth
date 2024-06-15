//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using nanoFramework.TestFramework;
using nanoFramework.Device.Bluetooth;

namespace NFUnitTest1
{
  /// <summary>
  /// Tests the different conversion and type detection functions for Bluetooth uuids
  /// </summary>
  [TestClass]
  public class TestUuidUtilities
  {
    [TestMethod]
    public void BaseUuidIsBluetoothSigUuid()
    {
      var baseUuid = new Guid("00000000-0000-1000-8000-00805f9b34fb");

      Assert.IsTrue(Utilities.IsBluetoothSigUUID(baseUuid));

      // the base uid marks the start of the pre-allocated range of 16bit or 32bit uuid values
      // the 16bit or 32bit value is 0 so we give it the Uuid16 type
      Assert.IsTrue(Utilities.TypeOfUuid(baseUuid) == Utilities.UuidType.Uuid16, "Expecting a 16bit uuid");
    }

    [TestMethod]
    public void NonBaseUuidIs128Uuid()
    {
      var uuid = new Guid("00000000-0000-1000-8000-00805f9b34fc"); // last digit differs from base uuid

      Assert.IsFalse(Utilities.IsBluetoothSigUUID(uuid));

      // as this uid is not based on the base uid, the type is a random Uuid128
      Assert.IsTrue(Utilities.TypeOfUuid(uuid) == Utilities.UuidType.Uuid128, "Expecting a 128bit uuid");
    }

    [TestMethod]
    public void Test16BitUuids()
    {
      ushort value16 = 0x1234;

      var serviceUid = Utilities.CreateUuidFromShortCode(value16);

      var bytes = serviceUid.ToByteArray();
      Assert.AreEqual((byte)0x34, bytes[0]);
      Assert.AreEqual((byte)0x12, bytes[1]);
      Assert.AreEqual((byte)0x00, bytes[2]);
      Assert.AreEqual((byte)0x00, bytes[3]);

      // the uuid must be recognized as falling in the range of 16 or 32bit uuids
      Assert.IsTrue(Utilities.IsBluetoothSigUUID(serviceUid));

      Assert.IsTrue(Utilities.TypeOfUuid(serviceUid) == Utilities.UuidType.Uuid16, "Expecting a 16bit uuid");

      ushort result = Utilities.ConvertUuidToShortId(serviceUid);

      Assert.AreEqual(value16, result, "After conversion, the end result must be the same value we started with");

      var refGuid = new Guid("00001234-0000-1000-8000-00805F9B34FB");

      Assert.AreEqual(refGuid, serviceUid, "The 16bit value is in the expected place of the 128bit uuid");
    }

    [TestMethod]
    public void Test32BitUuids()
    {
      var uuid32 = new Guid("12345678-0000-1000-8000-00805F9B34FB"); // 32bit value equals 0x12345678

      var bytes = uuid32.ToByteArray();
      Assert.AreEqual((byte)0x78, bytes[0]);
      Assert.AreEqual((byte)0x56, bytes[1]);
      Assert.AreEqual((byte)0x34, bytes[2]);
      Assert.AreEqual((byte)0x12, bytes[3]);

      // the uuid must be recognized as falling in the range of 16 or 32bit uuids
      Assert.IsTrue(Utilities.IsBluetoothSigUUID(uuid32));

      Assert.IsTrue(Utilities.TypeOfUuid(uuid32) == Utilities.UuidType.Uuid32, "Expecting a 32bit uuid");

      var result = Utilities.ConvertUuidToIntId(uuid32);

      Assert.AreEqual(0x12345678, result, "After conversion, the end result must be the same value we started with");
    }
  }
}
