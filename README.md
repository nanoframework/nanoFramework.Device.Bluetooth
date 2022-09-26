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

We have also added an extension to this assembly allowing extra services to be added to ServiceProvider with no restriction on type.

## Firmware versions

Bluetooth is currently only supported on ESP32 devices with following firmware.

- ESP32_BLE_REV0
- ESP32_BLE_REV3
- M5Core2

This restriction is due to IRAM memory space in the firmware image. 
With revision 1 of ESP32 devices, the PSRAM implementation requires a large number of PSRAM fixes which greatly reduces the 
available space in IRAM area so PSRAM is currently disabled for ESP32_BLE_REV0. With the revision 3 devices the Bluetooth and 
PSRAM are both available.

## Samples

A number of Bluetooth LE samples are available in the [nanoFramework samples repo](https://github.com/nanoframework/Samples/)

- [Bluetooth Low energy sample 1 (Basic Read/Write/Notify)](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/BluetoothLESample1)
- [Bluetooth Low energy sample 2 (Add Security)](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/BluetoothLESample2)
- [Bluetooth Low energy sample 3 (Show cases adding or replacing some standard services)](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/BluetoothLESample3)
- [Bluetooth Low energy serial (SPP)](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/BluetoothLESerial) 
- [Bluetooth Low energy central 1 (Simple Bluetooth scanner) ](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/Central1) 
- [Bluetooth Low energy central 2 (Data collector) ](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/Central2) 

## Usage

### Overview

This implementation supports a cut down version of the Gatt Server and Gatt Client implementation.

The device can either run as a Server or Client, but not at the same time.  
For example if you start a Watcher to look for advertisements from Server devices you will not 
be able to connect to those devices until the Watcher has been stopped. But you can recieve data from connected 
devices while Watcher is scanning.

For more information see relevant sections: -

- Gatt Server
- Gatt Client / Central

Also as part of this assembly is the NordicSPP class which implements a Serial Protocol Profile based on 
the Nordic specification. This allows clients to easily connect via Bluetooth LE to send and receive messages via a 
Bluetooth Serial Terminal application. A common use case is for provisioning devices.  See SPP section later for usage. 

### Attributes and UUIDs

Each service, characteristic and descriptor is defined by it's own unique 128-bit UUID. These are 
called GUID in this assembly. These are called UUID in the Bluetooth specifications. 

If the attribute is standard UUID defined by the Bluetooth SIG, it will also have a corresponding 16-bit short ID (for example, 
the characteristic **Battery Level** has a UUID of 00002A19-0000-1000-8000-00805F9B34FB and the short ID is 0x2A19). 
The common standard UUIDs can be seen in GattServiceUuids and GattCharacteristicUuids.

If the short ID is not present in GattServiceUuids or GattCharacteristicUuids then create your own short GUID by 
calling the utility function CreateUuidFromShortCode.

```csharp
Guid uuid1 = Utility.CreateUuidFromShortCode(0x2A19);
```
## Gatt Server

### Defining the service and associated Characteristics

The GattServiceProvider is used to create and advertise the primary service definition. An extra device information service will be automatically created.

```csharp
GattServiceProviderResult result = GattServiceProvider.Create(uuid);
if (result.Error != BluetoothError.Success)
{
    return result.Error;
}

serviceProvider = result.ServiceProvider;
```

Now add to the service all the required characteristics and descriptors. 
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
If the **StaticValue** is set the the read event will not be called and doesn't need to be defined.

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
If no event handler is set or you don't respond in a timely manner an Unlikely bluetooth error will be returned to client.  
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
set or you don't respond in a timely manner an Unlikely bluetooth error will be returned to client.

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

### Adding extra services
You can add or replace existing services and there are no restrictions on which services you add. 
See the Bluetooth sample 3 for an example of adding the bluetooth standard 
services, Device Information, Current Time, Battery level and Environmental Sensor.

```csharp
// Battery service exposes the current battery level percentage
BatteryService BatService = new BatteryService(serviceProvider);

// Update the Battery service with the current battery level when ever it is read.
BatService.BatteryLevel = 94;
```

## Advertising your service

Once all the Characteristics have been created you need to advertise the Service so other devices can see it 
and/or connect to it. We also provide the device name seen on the discovery.

```csharp
serviceProvider.StartAdvertising(new GattServiceProviderAdvertisingParameters()
{
    DeviceName = "My Example Device",
    IsConnectable = true,
    IsDiscoverable = true
});
```

# Gatt Client / Central

The Bluetooth LE client is used to look for advertisements from devices(Servers) and to connect to those 
devices and read and write values to Characteristics. You can set up notification events so you automatically
get informed when a value changes.

We have 2 samples available:
- Central1  - This straight forward sample to just watch for advertisements and print out results.
- Central2  - This more of a full on sample and is an example on how to collect values in this case temperatures 
from a bunch of devices with the Environmental Sensor service. The devices are an updated version of the Sample device example.

## Watching for Advertisments

To watch for advertisements you use the BluetoothLEAdvertisementWatcher class.

```csharp
    BluetoothLEAdvertisementWatcher watcher = new();
    watcher.Received += Watcher_Received;
    watcher.Start();
```
When a advertisement is received an event will be raised calling the Watcher_Received event handler.
In the event handler you will be able to select a device using the information supplied on the event.
This could be device LocalName or other data supplied in the advertisement data.

See samples for more information.

You can also add filters to the BluetoothLEAdvertisementWatcher.
Currently this is just an RSSI filter so you only receive from devices within a certain signal strength. 

RSSI filter
```csharp
    watcher.SignalStrengthFilter.InRangeThresholdInDBm = -70;
    watcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -77;
    watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(10000);
```

## Creating a device and connecting to device.

To communicate with a device a BluetoothLEDevice class needs to be created using the devices bluetooth address and type.
This can be the bluetooth address from the BluetoothLEAdvertisementWatcher event or using a hard coded address.

In this case from the Watcher advertisement received event.
```csharp
BluetoothLEDevice device = BluetoothLEDevice.FromBluetoothAddress(args.BluetoothAddress)
```
There are no specific connection methods but a connection will be made automatically when you query the device 
for its services. The ConnectionStatusChanged event can used to detect a change in connection status and an attempt to 
reconnect can be done by a query to the devices services again. Avoid doing this in the event, as it can block other 
events being fired during the connection.

You can go back to Watching for advertisements with the restriction that you can't connect to newly found devcies until the Watching is stopped.
You can still communication with connected devices while the Watcher is running. Best way is to collect all found devices in a table until Watcher is
stopped then connect to all found devices. See [Central 2 sample](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/Central2)

The Close() method is not exposed in the desktop version but it has been implemented in this version 
to give better control over the connection.

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
Both above get service method will try to connect to device.

Note: 
The services are cached so the first time they are queried it will retrieve them from the device.
Further calls for services will return the cached results. To clear the cache the device must be disposed.


## Querying service characterisics

When you have the correct GattDeviceService object you can query it for the Characteristics required.
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

## Querying characterisics descriptors

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
from the device. Any further calls to get descriptors will come from a local cached.

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
                       
## Enabling the value changed notifcations

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

Event handle for notification events. 
The sender is the GattCharacteristic the change is coming from and the valueChangedEventArgs.CharacteristicValue is
the Buffer value with new value. 
```csharp
    private static void ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs valueChangedEventArgs)
    {
        DataReader rdr = DataReader.FromBuffer(valueChangedEventArgs.CharacteristicValue);

        // Read value from DataReader
    }
```

## Handling device errors

To handle connection errors you need to monitor the ConnectionStatusChanged event on the 
BluetoothLEDevice object.  If the connection is lost you can try to reconnect by 
requesting the services again. See Central2 sample.

Make sure you check the return status for all requests to make sure they are successful.






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

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behaviour in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

## .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).

