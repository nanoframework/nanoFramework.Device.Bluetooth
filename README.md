[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nanoframework_nanoFramework.Device.Bluetooth&metric=alert_status)](https://sonarcloud.io/dashboard?id=nanoframework_nanoFramework.Device.Bluetooth) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=nanoframework_nanoFramework.Device.Bluetooth&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=nanoframework_nanoFramework.Device.Bluetooth) [![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE) [![NuGet](https://img.shields.io/nuget/dt/nanoFramework.Device.Bluetooth.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.Device.Bluetooth/) [![#yourfirstpr](https://img.shields.io/badge/first--timers--only-friendly-blue.svg)](https://github.com/nanoframework/Home/blob/main/CONTRIBUTING.md) [![Discord](https://img.shields.io/discord/478725473862549535.svg?logo=discord&logoColor=white&label=Discord&color=7289DA)](https://discord.gg/gCyBu8T)

![nanoFramework logo](https://raw.githubusercontent.com/nanoframework/Home/main/resources/logo/nanoFramework-repo-logo.png)

-----
document language: [English](README.md) | [简体中文](README.zh-cn.md)

# Welcome to the .NET **nanoFramework** nanoFramework.Device.Bluetooth Library repository

## Build status

| Component | Build Status | NuGet Package |
|:-|---|---|
| nanoFramework.Device.Bluetooth | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.Device.Bluetooth/_apis/build/status/nanoFramework.Device.Bluetooth?repoName=nanoframework%2FnanoFramework.Device.Bluetooth&branchName=main)](https://dev.azure.com/nanoframework/nanoFramework.Device.Bluetooth/_build/latest?definitionId=85&repoName=nanoframework%2FnanoFramework.Device.Bluetooth&branchName=main) | [![NuGet](https://img.shields.io/nuget/v/nanoFramework.Device.Bluetooth.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.Device.Bluetooth/) |

## nanoFramework.Device.Bluetooth class Library

Bluetooth Low Energy library.

This library is based on the Windows.Devices.Bluetooth UWP class library but simplified and with the asynchronous related calls made synchronous.
The original .Net assembly depended on Windows.Storage.Streams for DataReader & DataWriter; this library has simplified inbuilt versions.
So references to IBuffer in .Net UWP examples should now use Buffer instead.

## Firmware versions

Bluetooth is currently only supported on ESP32 devices with following firmware.

- ESP32_BLE_REV0
- ESP32_BLE_REV3
- ESP32_PSRAM_BLE_GenericGraphic_REV3
- ESP32_S3_BLE
- M5Core2
- LilygoTWatch2021
- ESP32_ETHERNET_KIT_1.2

The Bluetooth is not in every firmware due to a restriction in the IRAM memory space in the firmware image. 
For earlier revision 1 ESP32 devices, the PSRAM implementation required a large number of PSRAM library fixes which greatly reduces the 
available space in the IRAM area, so PSRAM is currently disabled for ESP32_BLE_REV0. With the revision 3 devices the Bluetooth and 
PSRAM are both available.

## Samples

A number of Bluetooth LE samples are available in the [nanoFramework samples repo](https://github.com/nanoframework/Samples/)

- [Bluetooth Low energy sample 1 (Basic Read/Write/Notify)](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/BluetoothLESample1)
- [Bluetooth Low energy sample 2 (Add Security)](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/BluetoothLESample2)
- [Bluetooth Low energy sample 3 (Show cases adding or replacing some standard services)](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/BluetoothLESample3)
- [Bluetooth Low energy serial (SPP)](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/BluetoothLESerial) 
- [Bluetooth Low energy central 1 (Simple Bluetooth scanner) ](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/Central1) 
- [Bluetooth Low energy central 2 (Data collector) ](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/Central2) 
- [Bluetooth Low energy central 3 (Authenticated Pairing sample) ](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/Central3) 

## Usage

### Overview

This implementation supports a cut down version of the Gatt Server and Gatt Client implementations.

The device can either run as a Server or Client, but not at the same time.  

For more information see relevant sections: -

- [Gatt Server](#gatt-server)
- [Gatt Client / Central](#gatt-client-central)
- [Advertisement Publishing](#advertisement-publishing)

Also as part of this assembly is the NordicSPP class which implements a Serial Protocol Profile based on 
the Nordic specification. This allows clients to easily connect via Bluetooth LE to send and receive messages via a 
Bluetooth Serial Terminal application. A common use case is for provisioning devices.  See SPP section later for usage. 

### Attributes and UUIDs

Each service, characteristic and descriptor is defined by it's own unique 128-bit UUID. These are 
called GUID in .Net and UUID in the Bluetooth specifications. 

If the attribute is standard UUID defined by the Bluetooth SIG, it will also have a corresponding 16-bit short ID (for example, 
the characteristic **Battery Level** has a UUID of 00002A19-0000-1000-8000-00805F9B34FB and the short ID is 0x2A19). 
The common standard UUIDs can be seen in the classes GattServiceUuids and GattCharacteristicUuids.

If the short ID is not present in GattServiceUuids or GattCharacteristicUuids then create your own short GUID by 
calling the utility function CreateUuidFromShortCode.

```csharp
Guid uuid1 = Utility.CreateUuidFromShortCode(0x2A19);
```

### Security and Pairing

The assembly supports pairing with encryption and authentication.

If you don't do anything in your code it will use the **Just works** method of pairing which will enable encryption on the connection.

To enable **Authentication** you will need to handle the pairing events in your code. 

For more information see [**Pairing**](#pairing) section.

## Gatt Server

The main object for the Gatt Server is the **BluetoothLEServer** class. This is a singleton class so there can only be one occurance of the BluetoothLEServer object.
The BluetoothLEServer object can be accessed by using the static **BluetoothLEServer.Instance** property. The first call to this will create the object.
Calling dispose will remove the object from memory.

This object is new and doesn't exist in the normal Windows implementation and was added to allow better handling of pairing and connections from the managed code.

### Setting the Device Name and it Appearance

The device name and appearance is part of the Generic Access service which is automatically
included in each Gatt Server definition. The name should be the name of current device and the appearance which
is optional is a 16 bit code that represents the use of the device. Defaults to 0 which is "Unknown Device".
For details on codes see the Bluetooth Sig Assigned numbers document; section **2.6.3 Appearance Sub­category values**. Use the values column for the code.
For example code 0x0481 is for a cycling computer.

```csharp
    BluetoothLEServer server = BluetoothLEServer.Instance;
    server.DeviceName = "Esp32_01";
    server.Appearance = 0x0481;
```

### Defining the service and associated Characteristics

The GattServiceProvider is used to create and advertise the primary service definitions. An extra device information service will be automatically created when first service is created.

```csharp
GattServiceProviderResult result = GattServiceProvider.Create(uuid);
if (result.Error != BluetoothError.Success)
{
    return result.Error;
}

serviceProvider = result.ServiceProvider;
```

To create further services for the Gatt server call **GattServiceProvider.Create(UUID)** for each new service. 

Access all created services via the BluetoothLEServer instance.

```csharp
BluetoothLEServer server = BluetoothLEServer.instance;
GattServiceProvider[] services = server.Services()
```

Or find a specific service by using its UUID.

```csharp
BluetoothLEServer server = BluetoothLEServer.instance;
GattServiceProvider service = server.GetServiceByUUID(uuid)
```

Using the *Service* property of the GattServiceProvider all the required characteristics can be added. 
Currently only Read, Write, WriteWithoutResponse, Notify and Indicate characteristics are supported.

### Adding a Read Characteristic

If a userDescription is added to the GattLocalCharacteristicParameters then a user description descriptor will be automatically added to the Characteristic. 
For a read Characteristic you will need an associated event handler to provide the data for the read.

```csharp
GattLocalCharacteristicParameters ReadParameters = new GattLocalCharacteristicParameters
{
    CharacteristicProperties = (GattCharacteristicProperties.Read),
    UserDescription = "My Read Characteristic"
};

GattLocalCharacteristicResult characteristicResult = serviceProvider.Service.CreateCharacteristic(uuid1, ReadParameters);
if (characteristicResult.Error != BluetoothError.Success)
{
    // An error occurred.
    return characteristicResult.Error;
}

_readCharacteristic = characteristicResult.Characteristic;
_readCharacteristic.ReadRequested += _readCharacteristic_ReadRequested;
```

You can have a read Characteristics with a constant value by setting the **StaticValue** property.

```csharp
// Setting a Int 16 constant value to the characteristic. 
DataWriter dr = new DataWriter();
dr.WriteInt16(123);

GattLocalCharacteristicParameters ReadParameters = new GattLocalCharacteristicParameters
{
    CharacteristicProperties = (GattCharacteristicProperties.Read),
    UserDescription = "My Read Characteristic",
    StaticValue = dr.DetachBuffer()
};

```
If the **StaticValue** is set the read event will not be called and doesn't need to be defined.

### Adding a Write or WriteWithoutResponse Characteristic

The Write Characteristic is used for receiving data from the client.  

```csharp
GattLocalCharacteristicParameters WriteParameters = new GattLocalCharacteristicParameters
{
    CharacteristicProperties = GattCharacteristicProperties.Write,
    UserDescription = "My Write Characteristic"
};


characteristicResult = serviceProvider.Service.CreateCharacteristic(uuid2, WriteParameters);
if (characteristicResult.Error != BluetoothError.Success)
{
    // An error occurred.
    return characteristicResult.Error;
}
_writeCharacteristic = characteristicResult.Characteristic;
_writeCharacteristic.WriteRequested += _writeCharacteristic_WriteRequested;
```

### Adding a Notify Characteristic

A notify Characteristic is used to automatically notify subscribed clients when a value has changed.

```csharp
GattLocalCharacteristicParameters NotifyParameters = new GattLocalCharacteristicParameters
{
    CharacteristicProperties = GattCharacteristicProperties.Notify,
    UserDescription = "My Notify Characteristic"
};

characteristicResult = serviceProvider.Service.CreateCharacteristic(uuid3, NotifyParameters);
if (characteristicResult.Error != BluetoothError.Success)
{
    // An error occurred.
    return characteristicResult.Error;
}

_notifyCharacteristic = characteristicResult.Characteristic;
_notifyCharacteristic.SubscribedClientsChanged += _notifyCharacteristic_SubscribedClientsChanged;
```

### Sending data to a Notify Characteristic

Data can be sent to subscribed clients by calling the NotifyValue method on the notify characteristic.
Extra checks can be added to only send values if there are subscribed clients or if the values has changed
since last notified.

```csharp
private static void UpdateNotifyValue(double newValue)
{
    DataWriter dw = new DataWriter();
    dw.WriteDouble(newValue);

    _notifyCharacteristic.NotifyValue(dw.DetachBuffer());
}
```

## Events

### Read requested event

When a client requests to read a characteristic, the managed event will be called assuming a static value hasn't been set.
If no event handler is set or you don't respond in a timely manner an Unlikely Bluetooth error will be returned to client.  
If reading the value from a peripheral device takes time then best to put this outside the event handler.

This show the returning of 2 values to client request. 

```csharp
private static void _readCharacteristic_ReadRequested(GattLocalCharacteristic sender, GattReadRequestedEventArgs ReadRequestEventArgs)
{
    GattReadRequest request = ReadRequestEventArgs.GetRequest();

    // Create DataWriter and write the data into buffer
    DataWriter dw = new DataWriter();
    dw.WriteInt16(1);
    dw.WriteInt32(2);

    request.RespondWithValue(dw.DetachBuffer());

    // If there is some sort of error then response with an error 
    //request.RespondWithProtocolError((byte)BluetoothError.DeviceNotConnected);
}
```

## Write requested event

When data is sent to a write characteristic the managed event is called. If no event handler is 
set or you don't respond in a timely manner an Unlikely Bluetooth error will be returned to client.

The data received is a array of bytes and this is formatted as required by characteristic. This could be a single
value of Int16, Int32, string etc. or it could be a number of different values.

This shows the reading of a single Int32 value from buffer and returns an error if the wrong number 
of bytes has been supplied.

```csharp
private static void _writeCharacteristic_WriteRequested(GattLocalCharacteristic sender, GattWriteRequestedEventArgs WriteRequestEventArgs)
{
    GattWriteRequest request = WriteRequestEventArgs.GetRequest();

    // Check expected data length
    if (request.Value.Length != 4)
    {
        request.RespondWithProtocolError((byte)BluetoothError.NotSupported);
        return;
    }

    // Read data from buffer of required format
    DataReader rdr = DataReader.FromBuffer(request.Value);
    Int32 data = rdr.ReadInt32();

    // Do something with received data
    Debug.WriteLine($"Rx data::{data}");

    // Respond if Write requires response
    if (request.Option == GattWriteOption.WriteWithResponse)
    {
        request.Respond();
    }
}
```
## Subscribed Clients changed event

For notifiable characteristics a client can subscribe to receive the notification values. When a client
subscribes the managed event will be called.
The SubscribedClients array of the characteristics contains the connected clients.

```csharp
private static void _notifyCharacteristic_SubscribedClientsChanged(GattLocalCharacteristic sender, object args)
{
    if ( sender.SubscribedClients.Length > 0)
    {
         Debug.WriteLine($"Client connected ");
    }
}
```

## Advertising your service

Once all the Characteristics have been created you need to advertise the Service so other devices can see it 
and/or connect to it. 

As part of the ServiceProvider we add the following data sections to the advertisement 
payload automatically.

* Flags 
* Complete Local Name 
* UUID of service defined on provider.
* ServiceData ( Optional )

An extension for nanoFramework allows the advertisement to be added to by adding data sections to
the Advertisement property of the GattServiceProviderAdvertisingParameters object.
Also by setting the GattServiceProviderAdvertisingParameters.CustomAdvertisement flag all data sections can
be set up by the user.

### Starting the advertisement.
```csharp
serviceProvider.StartAdvertising(new GattServiceProviderAdvertisingParameters()
{
    IsConnectable = true,
    IsDiscoverable = true
});
```

# Gatt Client / Central

The Bluetooth LE client is used to look for advertisements from devices(Servers) and to connect to those 
devices and read and/or write values to Characteristics. Notification can be set up so events will automatically
fire when a value changes.

We have 2 samples available:
- Central1  - This straight forward sample to just watch for advertisements and print out results.
- Central2  - This more of a full on sample and is an example on how to collect values in this case temperatures 
from a bunch of devices with the Environmental Sensor service. The devices are an updated version of the Sample device example.

## Watching for Advertisements

To watch for advertisements you use the BluetoothLEAdvertisementWatcher class.

```csharp
    BluetoothLEAdvertisementWatcher watcher = new();
    watcher.Received += Watcher_Received;
    watcher.Start();
```
When a advertisement is received an event will be raised calling the Watcher_Received event handler.
In the event handler you will be able to select a device using the information supplied on the event.
This could be device LocalName or other data supplied in the advertisement data.

If you start a Watcher to look for advertisements from Server devices you will not 
be able to connect to those devices until the Watcher has been stopped, but you can receive data from connected 
devices while Watcher is scanning. So the Watcher needs to be stopped while connections are being made to servers.

See samples for more information.

Filters can be added to the BluetoothLEAdvertisementWatcher.
THere are 2 filters  available:-
* RSSI filter - Advertisements are only received from devices within a certain signal strength. 
* Advertisement Filter - Advertisements are filtered by the contents of the advertisement.

### RSSI filter
```csharp
    watcher.SignalStrengthFilter.InRangeThresholdInDBm = -70;
    watcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -77;
    watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(10000);
```

### Advertisements Filter

Advertisements are only received from devices which match the data sections or part of a data section contained in the filter.

Update the advertisement object with data sections for the filter to match against.
There are some properties on object for common data sections. For other data sections load the "dataSections" arrayList property with the data sections required to match.

Using a property to match a local name of device.
Only adverts from devices with a local name of "Sample" will be received.
```csharp
    watcher.AdvertisementFilter.Advertisement.LocalName = "Sample";
```

This filter uses a partial match of part of data section.
Select all Advertisements with an "a" in 2nd position of Local name

If you want to filter on part of a data section then use the BytePatterns arrayList.
```csharp
   BluetoothLEAdvertisementBytePattern pattern = new BluetoothLEAdvertisementBytePattern()
   {
      DataType = (byte)BluetoothLEAdvertisementDataSectionType.CompleteLocalName,
      Data = new Buffer(new Byte[] { (Byte)'a' }),
      Offset = 1
   };

   watcher.AdvertisementFilter.BytePatterns.Add(pattern2;
```

Any data thats contained in an advertisement can be filtered on.
Advertisement and byte patterns can be used together.

For more examples of advertisement filters see the Watcher filter sample.

## Creating a device and connecting to device.

To communicate with a device a BluetoothLEDevice class needs to be created using the devices Bluetooth address and type.
This can be the Bluetooth address from the BluetoothLEAdvertisementWatcher event or using a hard coded address.

In this case from the Watcher advertisement received event BluetoothAddress argument.
```csharp
BluetoothLEDevice device = BluetoothLEDevice.FromBluetoothAddress(args.BluetoothAddress)
```
There are no specific connection methods, a connection will be made automatically when the device is queried for services or a Pairing operation is started. 
The ConnectionStatusChanged event can be used to detect a change in connection status and an attempt to 
reconnect can be done by a query to the devices services again. Avoid doing this in the event, as it can block other 
events being fired during the connection.

After connecting to the device go back to Watching for advertisements with the restriction that you can't connect to newly found devices until the Watching is stopped.
You can still communication with connected devices while the Watcher is running. Best way is to collect all found devices in a table until Watcher is
stopped then connect to all found devices. See [Central 2 sample](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/Central2)

The Close() method is not exposed in the desktop version but it has been implemented in this version 
to give better control over the connection.

## Getting information about the connected device

The Generic Access device name and appearance code are available as properties of the BluetoothLEDevice object after connecting to the device.

## Querying device services

Querying for all services provided by device. If the GattDeviceServicesResult status is a GattCommunicationStatus.Success then an
array of GattDeviceService objects are available in the GattDeviceServicesResult Services property.
```csharp
    GattDeviceServicesResult sr = device.GetGattServices();
    if (sr.Status == GattCommunicationStatus.Success)
    {
        GattDeviceService[] services = sr.Services;

    }
```
Querying for a specific service provided by device.
```csharp
    GattDeviceServicesResult sr = device.GetGattServicesForUuid(GattServiceUuids.EnvironmentalSensing);
    if (sr.Status == GattCommunicationStatus.Success)
    {
        
    }
```
Both above get service methods will try to connect to device.

Note: 
The services are cached so the first time they are queried it will retrieve them from the device.
Further calls for services will return the cached results. To clear the cache the device must be disposed.


## Querying service characteristics

With the GattDeviceService object a query can be made for the required Characteristics.
In the same way as the service there is a method to query for a all Characteristics or just specific Characteristics.

Query for all Characteristics
```csharp
    GattCharacteristicsResult cr = service.GetCharacteristics();
    if (cr.Status == GattCommunicationStatus.Success)
    {
        GattCharacteristic[] chars = cr.Characteristics;
    }    
```

Query for service for all Characteristics with the standard Temperature UUID.
```csharp
    GattCharacteristicsResult cr = service.GetCharacteristicsForUuid(GattCharacteristicUuids.Temperature);
    if (cr.Status == GattCommunicationStatus.Success)
    {
        GattCharacteristic[] gcs = cr.Characteristics;
    }    
```
Note: 
The Characteristics are cached so the first time requested it will retrieve them from the device.
Further calls to same service will return the cached results.

## Querying characteristics descriptors

Descriptors can be retrieved in the same way as Services and Characteristics using the methods 
GetDescriptors and GetDescriptorsForUuid.

In these examples gc is the GattCharacteristic object.

Get all descriptors.
```csharp
    GattDescriptorsResult dr = gc.GetDescriptors();
    if (dr.Status == GattCommunicationStatus.Success)
    {

    }   
```
Get all descriptors with particular UUID.
```csharp
    GattDescriptorsResult dr = gc.GetDescriptorsForUuid(uuid);
    if (dr.Status == GattCommunicationStatus.Success)
    {

    }   
```
The properties **UserDescription** or **PresentationFormats** will automatically retrieve the descriptors
from the device. Any further calls to get descriptors will come from a local cache.

## Reading and Writing attribute values

To read a value from a Characteristic or Descriptor use the ReadValue() method. If successful a Buffer object
will be available where the data can be read from using the DataReader.

The format of the data in Buffer will depend on Characteristic/Descriptors being read.

This example reads the value from Characteristic/Descriptor and loads the 3 bytes into a Byte and a ushort.
```csharp
    GattReadResult rr = gc.ReadValue();
    if (rr.Status == GattCommunicationStatus.Success)
    {
        DataReader rdr = DataReader.FromBuffer(rr.Value);
        Byte data1 = rdr.ReadByte();
        ushort data2 = rdr.ReadInt16();

    }
```

To write to a Characteristic or Descriptor, create a Buffer with desired data and call the WriteValueWithResult() method.
This can be used to Write with or without a response.
```csharp
    DataWriter dw = new();
    dw.WriteBytes(new byte[] { 1, 2, 3, 4 });
    dw.WriteUInt32(23);

    GattWriteResult wr = gc.WriteValueWithResult(dw.DetachBuffer(), , GattWriteOption.WriteWithResponse);
    if (wr.Status == GattCommunicationStatus.Success)
    {

    }
```
                       
## Enabling the value changed notifications

This enables the receiving of events when a Characteristic value changes on the server. The notifications are enabled
by setting up an event then setting the value of the CCCD descriptor as below.

Example with gc being the GattCharacteristic to enable.
```csharp
    // Set up a notify value changed event
    gc.ValueChanged += ValueChanged;

    // and configure CCCD for Notify
    gc.WriteClientCharacteristicConfigurationDescriptorWithResult(GattClientCharacteristicConfigurationDescriptorValue.Notify);
```
To switch off the notifications write the none value to the CCCD descriptor.

## Event handler for notification events. 

The sender is the GattCharacteristic the change is coming from and the valueChangedEventArgs.CharacteristicValue is
the Buffer value with the new value. 
```csharp
    private static void ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs valueChangedEventArgs)
    {
        DataReader rdr = DataReader.FromBuffer(valueChangedEventArgs.CharacteristicValue);

        // Read value from DataReader
    }
```

## Handling device errors

Handle connection errors by monitoring the ConnectionStatusChanged event on the BluetoothLEDevice object.  
If the connection is lost, reconnect by requesting the services again. 

See Central2 sample.

Make sure you check the return status for all requests to make sure they are successful.


# Pairing

Pairing is handled by the **DevicePairing** class which is a property on the *BluetoothLEServer* and *BluetoothLEDevice* classes.
The properties *ProtectionLevel* and *IOCapabilities* control how the pairing will be done.

For good information on Bluetooth Pairing and how the IO Capabilities effect the type of pairing see this very useful [Blog](https://www.bluetooth.com/blog/bluetooth-pairing-part-2-key-generation-methods/) on www.bluetooth.com

## ProtectionLevel

By default the *ProtectionLevel* is **None** and the *IOCapabilities* is **NoInputNoOutput** so the pairing will use **Just Works** method if both ends are nanoFramework.

The ProtectionLevel will be automatically updated depending on the requirements of any added Characteristics.
This can be manually set to force a different level of protection.

| *ProtectionLevel* | |
| ------- | ----- |
| None | No security is used for connection, legacy pairing |
| Encryption | A Secure connection is used. Keys are automatically generated for connection. | 
| EncryptionAndAuthentication | As secure connection is used and authentication is required. IoCapabilities must be set correctly |

## IOCapabilities

The IO Capabilities are input and output peripherals available on the device for pairing.
Depending on what IOCapabilities each device has will govern how the pairing will be done.

| *IOCapabilities* | |
| ------- | ----- |
| DisplayOnly | Only a Display is available. This can be used to display passKey for other device to input. |
| DisplayYesNo | A Display and a means of inputing Yes or No. i.e 2 buttons. | 
| NoInputNoOutput | No input or output is available. |
| KeyboardDisplay | A display and Keyboard is available |

## Pairing Matrix

The Initiator is the device thats starts the pairing operation. This would normally be the Client.
The Responder is the device responding to the pairing request.

*Just Works*  : Means there is no input required and connection is just set up. If any Characteristics 
have a protection level of Authentication then a access error will be given.

| **Responder IOCapabilities** || <- | <- | *Initiator* *IOCapabilities* | ->  | -> |
| --------- | ----------- | ------------ | ------------ | --------------- | --------------- |
|  || *DisplayOnly* | *DisplayYesNo* | *KeyboardOnly* | *NoInputNoOutput* | *KeyboardDisplay* | 
| **DisplayOnly** || Just Works | Just Works | PassKey R->I | Just Works | PassKey R->I |
| **DisplayYesNo** || Just Works | Just Works or Compare(sec) | PassKey R->I | Just Works | PassKey R->I or Compare(sec) |
| **KeyboardOnly** || PassKey I->R | PassKey I->R | Passkey R&I | Just Works | PassKey I->R |
| **NoInputNoOutput** || Just Works | Just Works | Just Works | Just Works | Just Works |
| **KeyboardDisplay** || PassKey I->R | PassKey I->R or Compare(sec) | PassKey R->I | Just Works | PassKey I->R or Compare(sec) |

Where:-
* *Passkey R->I* : The passkey is displayed on responder and input done on initiator.
* *Passkey I->R* : The passkey is displayed on initiator and input done on responder.
* *Passkey R&I*  : PassKey is inputted on initiator and responder.
* *Compare(sec)* : Numeric comparison when using secure connection. Both ends input and display passkey.

### Common use cases for IOCapabilities.

| Responder(Client) | Pairing | Initiator(Server) |
| --------- | ----------- | ------------ |
| *NoInputNoOutput* | Just Works | *NoInputNoOutput* |
| *KeyboardOnly* | PassKey input/sent from Client, Server checks | *DisplayOnly* |

### Code examples 

For a full example see the samples *BluetoothLESample2* and *Central3*. 

#### Server

##### Setup for Server pairing. 

The client has to provide the correct passKey.

```csharp
    BluetoothLEServer server = BluetoothLEServer.Instance;
    server.DeviceName = "MyTestEsp32";

    // Set up an event handler for handling pairing requests
    server.Pairing.PairingRequested += Pairing_PairingRequested;
    server.Pairing.PairingComplete += Pairing_PairingComplete;

    // Say we have a display
    server.Pairing.IOCapabilities = DevicePairingIOCapabilities.DisplayOnly;

    // Set ProtectionLevel
    server.Pairing.ProtectionLevel = DevicePairingProtectionLevel.EncryptionAndAuthentication;

    // Start the Bluetooth server. 
    server.Start();

    // Add services and advertise

```
##### Event Handler for PairingRequested event. 

The PairingKind indicates what the application needs to do.
In this case its DevicePairingKinds.DisplayPin which just needs the passKey to compare with client.

For a general use device with a display the passkey should be displayed so client knows what to input.

```csharp
private static void Pairing_PairingRequested(object sender, DevicePairingRequestedEventArgs args)
{
    switch (args.PairingKind)
    {
        // Passkey displayed on current device or just a know secret passkey
        // Tell BLE what passkey is, so it can be checked against what has been entered on other device.
        case DevicePairingKinds.DisplayPin:
            args.Accept(654321);
            break;
    }
}
```

##### Event handle for PairingComplete event. 

This will inform program if pairing was successful or not.
Check the args.Status is a success and the IsPaired property is true.

```csharp
private static void Pairing_PairingComplete(object sender, DevicePairingEventArgs args)
{
    DevicePairing dp = sender as DevicePairing;
         
    Console.WriteLine($"PairingComplete:{args.Status} IOCaps:{dp.IOCapabilities} IsPaired:{dp.IsPaired} IsAuthenticated:{dp.IsAuthenticated}");
}
```


#### Client

##### Setup of BluetoothLEDevice for pairing with Authentication.

```csharp
static void SetupDevice(BluetoothLEDevice device)
{
    // Event handlers pairing
    device.Pairing.PairingRequested += Pairing_PairingRequested;
    device.Pairing.PairingComplete += Pairing_PairingComplete;

    // Set up IOCapabilities & ProtectionLevel
    device.Pairing.IOCapabilities = DevicePairingIOCapabilities.KeyboardOnly;
    device.Pairing.ProtectionLevel = DevicePairingProtectionLevel.EncryptionAndAuthentication;

    // Pair with device
    DevicePairingResult pairResult = device.Pairing.Pair();

```

##### Event handler for inputting passkey. 

```csharp
private static void Pairing_PairingRequested(object sender, DevicePairingRequestedEventArgs args)
{
    Console.WriteLine($"Pairing_PairingRequested:{args.PairingKind}");

    switch (args.PairingKind)
    {
        case DevicePairingKinds.ProvidePin:
            // Provide valid passkey
            args.Accept(654321);
            break;
    }
}

```

##### Event Handler for pairing complete event.

```csharp
private static void Pairing_PairingComplete(object sender, DevicePairingEventArgs args)
{
    // Pick up DevicePairing from sender or just use it directly
    DevicePairing pairing = (DevicePairing)sender;

    if (args.Status == DevicePairingResultStatus.Paired)
    {
        Console.WriteLine($"PairingComplete:{args.Status} IOCaps:{pairing.IOCapabilities} IsPaired:{pairing.IsPaired} IsAuthenticated:{pairing.IsAuthenticated}");
    }
    else
    {
        Console.WriteLine($"PairingComplete failed - status = {args.Status}");
    }
}
```

# Advertisement Publishing

The BluetoothLEAdvertisementPublisher class allows the configuration and advertising of a Bluetooth LE advertisement packet.
The payload of the advertisement is configured when the class is constructed via the BluetoothLEAdvertisement class.

The BluetoothLEAdvertisement class is used to control exactly what the advertisement packet contains and is mainly
used to create beacons.

The advertisement is constructed by adding BluetoothLEAdvertisementDataSection to the BluetoothLEAdvertisement class.
For some common Data Sections there are properties on the BluetoothLEAdvertisement class which automatically add the correct 
data section to advertisement. i.e. LocalName, Flags

Once the BluetoothLEAdvertisementPublisher class has been constructed the advertising can be started or stopped with 
the Start() and Stop() methods.

## Creating an advertisement packet
For legacy advertisements the packet length is 31 bytes which includes all data sections.
Each data section is 1 byte for length, 1 byte for section type and the data bytes. Any data section that
doesn't fit in advertisement will be moved to the scan response or left off it no room.

Currently we don't support extended advertisement. This will be available in future release.


```csharp
BluetoothLEAdvertisementPublisher publisher = new BluetoothLEAdvertisementPublisher();

// Add Flags using property
publisher.Advertisement.Flags = BluetoothLEAdvertisementFlags.GeneralDiscoverableMode |
                                BluetoothLEAdvertisementFlags.DualModeControllerCapable |
                                BluetoothLEAdvertisementFlags.DualModeHostCapable;

// Adding flags using Data Sections
publisher.Advertisement.

```


## Starting and Stopping Publisher
Advertise for 1 minute.
```csharp
publisher.Start();

Thread.Sleep(60000);

publisher.Stop();
```




---

# Bluetooth Serial Port Profile(SPP)

This assembly has an implementation of the Nordic SPP which can easily be used to send messages between a Bluetooth client and the device 
running the SPP. This is a simple way of provisioning a device with any extra information like WiFi details.

There are a number of Android and IOS app that support Nordic SPP that can be used to send/receive messages.

## Create instance of the SPP

Create an instance of the SPP and provide event handlers for reading messages and client connection activity.
Start advertising with a device name.

Uses namespace **nanoFramework.Device.Bluetooth.Spp**

```csharp
NordicSpp spp = new NordicSpp();
spp.ReceivedData += Spp_ReceivedData;
spp.ConnectedEvent += Spp_ConnectedEvent;

spp.Start("MySpp");

```

When complete, call the Stop method to stop the SPP.

## Handling Read Data events

Data can be read as either a array of bytes or as a string.

```csharp
private void Spp_ReceivedData(IBluetoothSpp sender, SppReceivedDataEventArgs ReadDataEventArgs)
{
    string message = ReadDataEventArgs.DataString;

    // Do something with incoming message
    Debug.WriteLine($"Message:{message}");

    // For this example lets respond with "OK"
    NordicSpp spp = sender as NordicSpp;
    spp.SendString("OK");
}
```

## Handling connection events

A connection event is thrown when a client connects or disconnects from SPP server.
Here we send a message when a client connects. 

```csharp
private void Spp_ConnectedEvent(IBluetoothSpp sender, EventArgs e)
{
    NordicSpp spp = sender as NordicSpp;

    if (spp.IsConnected)
    {
        spp.SendString("Welcome to nanoFramework");
    }
}
```

## Feedback and documentation

For documentation, providing feedback, issues and finding out how to contribute please refer to the [Home repo](https://github.com/nanoframework/Home).

Join our Discord community [here](https://discord.gg/gCyBu8T).

## Credits

The list of contributors to this project can be found at [CONTRIBUTORS](https://github.com/nanoframework/Home/blob/main/CONTRIBUTORS.md).

## License

The **nanoFramework** Class Libraries are licensed under the [MIT license](LICENSE.md).

## Code of Conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

## .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).
