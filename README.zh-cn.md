[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nanoframework_nanoFramework.Device.Bluetooth&metric=alert_status)](https://sonarcloud.io/dashboard?id=nanoframework_nanoFramework.Device.Bluetooth) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=nanoframework_nanoFramework.Device.Bluetooth&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=nanoframework_nanoFramework.Device.Bluetooth) [![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE) [![NuGet](https://img.shields.io/nuget/dt/nanoFramework.Device.Bluetooth.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.Device.Bluetooth/) [![#yourfirstpr](https://img.shields.io/badge/first--timers--only-friendly-blue.svg)](https://github.com/nanoframework/Home/blob/main/CONTRIBUTING.md) [![Discord](https://img.shields.io/discord/478725473862549535.svg?logo=discord&logoColor=white&label=Discord&color=7289DA)](https://discord.gg/gCyBu8T)

![nanoFramework logo](https://raw.githubusercontent.com/nanoframework/Home/main/resources/logo/nanoFramework-repo-logo.png)

-----
文档语言: [English](README.md) | [简体中文](README.zh-cn.md)


# 欢迎来到 .NET **nanoFramework** nanoFramework.Device.Bluetooth 类库

## 构建状态

| Component | Build Status | NuGet Package |
|:-|---|---|
| nanoFramework.Device.Bluetooth | [![构建状态](https://dev.azure.com/nanoframework/nanoFramework.Device.Bluetooth/_apis/build/status/nanoFramework.Device.Bluetooth?repoName=nanoframework%2FnanoFramework.Device.Bluetooth&branchName=main)](https://dev.azure.com/nanoframework/nanoFramework.Device.Bluetooth/_build/latest?definitionId=85&repoName=nanoframework%2FnanoFramework.Device.Bluetooth&branchName=main) | [![NuGet](https://img.shields.io/nuget/v/nanoFramework.Device.Bluetooth.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.Device.Bluetooth/) |

## nanoFramework.Device.Bluetooth 类库

蓝牙低能耗库

该库基于Windows.Devices.Bluetooth UWP类库，但经过简化，并与异步相关的调用进行同步。
最初的.net程序集依赖于Windows.Storage.Streams for DataReader和DataWriter;这个库简化了内建版本。
所以.net UWP示例中的IBuffer现在应该使用Buffer代替。

我们还为该程序集添加了一个扩展，允许向ServiceProvider添加额外的服务，而不限制类型。

## 固件版本

蓝牙目前仅支持ESP32设备与以下固件。

- ESP32_BLE_REV0
- ESP32_BLE_REV3
- M5Core2

此限制是由于固件映像中的IRAM内存空间。
对于ESP32设备的第1版，PSRAM实现需要大量的PSRAM补丁，这大大减少了
IRAM区域有可用空间，所以目前ESP32_BLE_REV0禁用了PSRAM。与3版设备的蓝牙和
PSRAM都是可用的。

## 示例

许多蓝牙示例可在 [nanoFramework示例在这里](https://github.com/nanoframework/Samples/)

- [蓝牙低能耗示例 1(Basic Read/Write/Notify)](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/BluetoothLESample1)
- [蓝牙低能耗示例 2 (Add Security)](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/BluetoothLESample2)
- [蓝牙低能耗示例 3 (Show cases adding or replacing some standard services)](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/BluetoothLESample3)
- [蓝牙低能耗串行 (SPP)](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/BluetoothLESerial)
- [蓝牙低能耗中心1(简单蓝牙扫描仪)](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/Central1)
- [蓝牙低能耗中心2(数据采集器)](https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/Central2)

## 使用

### 概述

此实现支持Gatt服务器和Gatt客户端实现的简化版本。
设备既可以作为服务器运行，也可以作为客户端运行，但不能同时运行。

例如，如果你启动一个Watcher来寻找来自服务器设备的广告，你就不会这样做
能够连接到这些设备，直到观察者停止。但你可以从connected接收数据
设备，而监视程序正在扫描。

更多信息请参见相关章节:-

- Gatt Server
- Gatt Client / Central

也是该程序集的一部分的是NordicSPP类，它实现了基于的串行协议配置文件
北欧的规范。这使客户端可以轻松地通过蓝牙连接LE发送和接收消息通过一个
蓝牙串行终端应用。一个常见的用例是提供设备。参见后面的SPP部分了解用法。

### 属性和uuid

每个服务、特征和描述符都由它自己唯一的128位UUID定义。这些都是
叫做GUID。这些在蓝牙规范中称为UUID。

如果该属性是蓝牙SIG定义的标准UUID，则它还将具有相应的16位短ID(例如，
特征**Battery Level**的UUID为00002A19-0000-1000-8000-00805F9B34FB，短ID为0x2A19)。
通用的标准uuid可以在gatserviceuuid和gatcharacteruuids中看到。

如果短ID没有出现在GattServiceUuids或gattcharacteristicuids中，那么通过以下方法创建自己的短GUID
调用实用函数CreateUuidFromShortCode。

```csharp
Guid uuid1 = Utility.CreateUuidFromShortCode(0x2A19);
```
## Gatt Server

### 定义服务和关联的特征

GattServiceProvider 用于创建和发布主服务定义。将自动创建一个额外的设备信息服务。

```csharp
GattServiceProviderResult result = GattServiceProvider.Create(uuid);
if (result.Error != BluetoothError.Success)
{
    return result.Error;
}

serviceProvider = result.ServiceProvider;
```

现在向服务添加所有必需的特征和描述符。
目前只支持读、写、无响应写、通知和指示特征。

### 添加读特性

如果用户描述被添加到gatlocalcharacteristicparameters中，那么用户描述描述符将自动添加到特征中。
对于读取特征，您将需要一个关联的事件处理程序来为读取提供数据。

```csharp
GattLocalCharacteristicParameters ReadParameters = new GattLocalCharacteristicParameters
{
    CharacteristicProperties = (GattCharacteristicProperties.Read),
    UserDescription = "我的阅读特点"
};

GattLocalCharacteristicResult characteristicResult = serviceProvider.Service.CreateCharacteristic(uuid1, ReadParameters);
if (characteristicResult.Error != BluetoothError.Success)
{
    // 一个错误发生。
    return characteristicResult.Error;
}

_readCharacteristic = characteristicResult.Characteristic;
_readCharacteristic.ReadRequested += _readCharacteristic_ReadRequested;
```

您可以通过设置**StaticValue**属性来获得具有常量值的读特性。

```csharp
// 设置特征的Int 16常量值。
DataWriter dr = new DataWriter();
dr.WriteInt16(123);

GattLocalCharacteristicParameters ReadParameters = new GattLocalCharacteristicParameters
{
    CharacteristicProperties = (GattCharacteristicProperties.Read),
    UserDescription = "我的阅读特点",
    StaticValue = dr.DetachBuffer()
};

```
如果设置了**StaticValue**，则不会调用读事件，也不需要定义该事件。

### 添加写或无响应写特性

写特性用于从客户端接收数据。

```csharp
GattLocalCharacteristicParameters WriteParameters = new GattLocalCharacteristicParameters
{
    CharacteristicProperties = GattCharacteristicProperties.Write,
    UserDescription = "我的阅读特点",
};


characteristicResult = serviceProvider.Service.CreateCharacteristic(uuid2, WriteParameters);
if (characteristicResult.Error != BluetoothError.Success)
{
    // 一个错误发生。
    return characteristicResult.Error;
}
_writeCharacteristic = characteristicResult.Characteristic;
_writeCharacteristic.WriteRequested += _writeCharacteristic_WriteRequested;
```

### 添加通知特性

notify特征用于在值发生更改时自动通知订阅的客户端。

```csharp
GattLocalCharacteristicParameters NotifyParameters = new GattLocalCharacteristicParameters
{
    CharacteristicProperties = GattCharacteristicProperties.Notify,
    UserDescription = "我的阅读特点",
};

characteristicResult = serviceProvider.Service.CreateCharacteristic(uuid3, NotifyParameters);
if (characteristicResult.Error != BluetoothError.Success)
{
    // 一个错误发生。
    return characteristicResult.Error;
}

_notifyCharacteristic = characteristicResult.Characteristic;
_notifyCharacteristic.SubscribedClientsChanged += _notifyCharacteristic_SubscribedClientsChanged;
```

### 向Notify特征发送数据

通过调用notify特性上的NotifyValue方法，可以将数据发送到订阅的客户机。
可以添加额外的检查，以便仅在有订阅客户端或值发生更改时才发送值
自从去年收到通知。

```csharp
private static void UpdateNotifyValue(double newValue)
{
    DataWriter dw = new DataWriter();
    dw.WriteDouble(newValue);

    _notifyCharacteristic.NotifyValue(dw.DetachBuffer());
}
```

## 事件

### 读请求事件

当客户端请求读取特征时，假定未设置静态值，托管事件将被调用。

如果没有设置事件处理程序或您没有及时响应，将向客户端返回一个不太可能的蓝牙错误。

如果从外围设备读取值需要时间，那么最好将其放在事件处理程序之外。

这显示了向客户端请求返回2个值。

```csharp
private static void _readCharacteristic_ReadRequested(GattLocalCharacteristic sender, GattReadRequestedEventArgs ReadRequestEventArgs)
{
    GattReadRequest request = ReadRequestEventArgs.GetRequest();

    // 创建DataWriter并将数据写入缓冲区
    DataWriter dw = new DataWriter();
    dw.WriteInt16(1);
    dw.WriteInt32(2);

    request.RespondWithValue(dw.DetachBuffer());

    // 如果存在某种类型的错误，则用错误响应
    //request.RespondWithProtocolError((byte)BluetoothError.DeviceNotConnected);
}
```

## 写请求事件

当数据被发送到写特性时，托管事件被调用。如果没有事件处理程序
设置或您没有及时响应，将返回一个不可能的蓝牙错误给客户端。

接收到的数据是一个字节数组，并按特性的要求格式化。这可能是一个单打Int16, Int32, string等，也可以是多个不同的值。

这将显示从缓冲区读取单个Int32值，如果数值错误则返回错误
已提供的字节数。

```csharp
private static void _writeCharacteristic_WriteRequested(GattLocalCharacteristic sender, GattWriteRequestedEventArgs WriteRequestEventArgs)
{
    GattWriteRequest request = WriteRequestEventArgs.GetRequest();

    // 检查预期数据长度
    if (request.Value.Length != 4)
    {
        request.RespondWithProtocolError((byte)BluetoothError.NotSupported);
        return;
    }

    // 从所需格式的缓冲区读取数据
    DataReader rdr = DataReader.FromBuffer(request.Value);
    Int32 data = rdr.ReadInt32();

    // 对接收到的数据做些什么
    Debug.WriteLine($"Rx data::{data}");

    // 如果Write需要响应，则响应
    if (request.Option == GattWriteOption.WriteWithResponse)
    {
        request.Respond();
    }
}
```

## 订阅的客户端更改事件

对于可通知的特征，客户端可以订阅以接收通知值。当客户端
订阅将被调用的托管事件。
特征的SubscribedClients数组包含连接的客户机。

```csharp
private static void _notifyCharacteristic_SubscribedClientsChanged(GattLocalCharacteristic sender, object args)
{
    if ( sender.SubscribedClients.Length > 0)
    {
         Debug.WriteLine($"客户端连接");
    }
}
```

### 添加额外的服务

您可以添加或替换现有的服务，对添加的服务没有任何限制。
添加蓝牙标准的示例请参见蓝牙示例3
服务、设备信息、当前时间、电池电量和环境传感器。

```csharp
// 电池服务显示当前电池电量百分比
BatteryService BatService = new BatteryService(serviceProvider);

// 当电池服务被读取时，用当前电池电量更新电池服务。
BatService.BatteryLevel = 94;
```

## 广告服务

一旦创建了所有的特征，你需要发布服务，以便其他设备可以看到它
和/或连接到它。我们还提供在发现中看到的设备名称。

```csharp
serviceProvider.StartAdvertising(new GattServiceProviderAdvertisingParameters()
{
    DeviceName = "我的例子设备",
    IsConnectable = true,
    IsDiscoverable = true
});
```

# Gatt Client / Central

蓝牙LE客户端用于从设备(服务器)中查找广告并连接到这些广告
设备和读取和写入值的特性。您可以设置通知事件，以便自动
当值发生变化时获得通知。

我们有2个样品可供选择:
- Central1 -这个直接的样本，只看广告和打印结果。
- Central2 -这是一个完整的示例，是一个关于如何在这种情况下收集温度值的示例
来自一堆带有环境传感器服务的设备。这些设备是Sample设备示例的更新版本。

## 监听广告

要监听广告，可以使用BluetoothLEAdvertisementWatcher类。

```csharp
    BluetoothLEAdvertisementWatcher watcher = new();
    watcher.Received += Watcher_Received;
    watcher.Start();
```
当接收到广告时，将引发一个事件，调用Watcher_Received事件处理程序。

在事件处理程序中，您将能够使用事件上提供的信息选择设备。

这可以是设备LocalName或广告数据中提供的其他数据。

有关更多信息，请参见示例。

你也可以添加过滤器到BluetoothLEAdvertisementWatcher。

目前这只是一个RSSI滤波器，所以你只从一定信号强度内的设备接收。

RSSI filter
```csharp
    watcher.SignalStrengthFilter.InRangeThresholdInDBm = -70;
    watcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -77;
    watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(10000);
```

## 创建设备并连接到设备

要与设备通信，需要使用设备的蓝牙地址和类型创建bluetthledevice类。

这可以是BluetoothLEAdvertisementWatcher事件中的蓝牙地址，也可以是使用硬编码的地址。

在本例中，来自观察者的广告接收事件。
```csharp
BluetoothLEDevice device = BluetoothLEDevice.FromBluetoothAddress(args.BluetoothAddress)
```没有具体的连接方式，查询设备时会自动建立连接

为其服务。ConnectionStatusChanged事件可用于检测连接状态的变化和尝试

重新连接可以通过再次查询设备服务来完成。避免在事件中这样做，因为它会阻碍其他的

在连接期间触发的事件。



您可以返回到“观看”查看广告，但有一个限制:在“观看”停止之前，您不能连接到新发现的设备。

在监视程序运行时，您仍然可以与连接的设备通信。最好的方法是收集表中所有找到的设备，直到Watcher被

停止，然后连接到所有找到的设备。见【示例2】(https://github.com/nanoframework/Samples/tree/main/samples/Bluetooth/Central2)


在桌面版本中没有公开Close()方法，但是在这个版本中已经实现了它以便更好地控制连接。

## 查询设备服务

查询设备提供的所有服务。如果“GattDeviceServicesResult”状态为“GattCommunicationStatus”。成功后

数组的GattDeviceService对象可以在GattDeviceServicesResult Services属性中获得。
```csharp
    GattDeviceServicesResult sr = device.GetGattServices();
    if (sr.Status == GattCommunicationStatus.Success)
    {
        GattDeviceService[] services = sr.Services;

    }
```
查询设备提供的指定服务。
```csharp
    GattDeviceServicesResult sr = device.GetGattServicesForUuid(GattServiceUuids.EnvironmentalSensing);
    if (sr.Status == GattCommunicationStatus.Success)
    {
        
    }
```
以上两种获取服务的方法都将尝试连接到设备。

注意:
服务会被缓存，所以当它们第一次被查询时，它会从设备中检索它们。
对服务的进一步调用将返回缓存的结果。要清除缓存，必须对设备进行处理。

## 查询服务特点

当您有正确的GattDeviceService对象时，您可以查询它以获得所需的features。

与服务一样，有一个方法可以查询所有的特征或仅查询特定的特征。

查询所有特征
```csharp
    GattCharacteristicsResult cr = service.GetCharacteristics();
    if (cr.Status == GattCommunicationStatus.Success)
    {
        GattCharacteristic[] chars = cr.Characteristics;
    }    
```

查询具有标准温度UUID的所有特征的服务。
```csharp
    GattCharacteristicsResult cr = service.GetCharacteristicsForUuid(GattCharacteristicUuids.Temperature);
    if (cr.Status == GattCommunicationStatus.Success)
    {
        GattCharacteristic[] gcs = cr.Characteristics;
    }    
```
注意:

特征被缓存，所以第一次请求时它将从设备中检索它们。

对同一服务的进一步调用将返回缓存的结果。

## 查询特征描述符

使用这些方法，描述符可以以与服务和特征相同的方式进行检索

GetDescriptors GetDescriptorsForUuid。

在这些例子中，gc是gatcharacteristic对象。

得到所有描述符。
```csharp
    GattDescriptorsResult dr = gc.GetDescriptors();
    if (dr.Status == GattCommunicationStatus.Success)
    {

    }   
```
获取具有特定UUID的所有描述符。
```csharp
    GattDescriptorsResult dr = gc.GetDescriptorsForUuid(uuid);
    if (dr.Status == GattCommunicationStatus.Success)
    {

    }   
```
属性**UserDescription**或**PresentationFormats**将自动检索描述符

从设备。任何进一步获取描述符的调用都将来自本地缓存。

## 读取和写入属性值

要从trait或Descriptor中读取值，请使用ReadValue()方法。如果成功，则返回Buffer对象

将可用，在那里可以使用DataReader读取数据。

缓冲区中数据的格式取决于被读取的特征/描述符。

这个例子从Characteristic/Descriptor中读取值，并将这3个字节加载到Byte和ushort中。
```csharp
    GattReadResult rr = gc.ReadValue();
    if (rr.Status == GattCommunicationStatus.Success)
    {
        DataReader rdr = DataReader.FromBuffer(rr.Value);
        Byte data1 = rdr.ReadByte();
        ushort data2 = rdr.ReadInt16();

    }
```

要写入一个特征或描述符，创建一个缓冲区所需的数据，并调用WriteValueWithResult()方法。
```csharp
    DataWriter dw = new();
    dw.WriteBytes(new byte[] { 1, 2, 3, 4 });
    dw.WriteUInt32(23);

    GattWriteResult wr = gc.WriteValueWithResult(dw.DetachBuffer(), , GattWriteOption.WriteWithResponse);
    if (wr.Status == GattCommunicationStatus.Success)
    {

    }
```
                       
## 启用值更改通知

这允许在服务器上的Characteristic值发生变化时接收事件。通知是启用的通过设置一个事件，然后设置CCCD描述符的值，如下所示。

gc是GattCharacteristic要启用的示例。
```csharp
    // 设置一个通知值更改事件
    gc.ValueChanged += ValueChanged;

    // 并为Notify配置CCCD
    gc.WriteClientCharacteristicConfigurationDescriptorWithResult(GattClientCharacteristicConfigurationDescriptorValue.Notify);
```
要关闭通知，将none值写入CCCD描述符。

通知事件的事件句柄。

发送者是更改来自的gatcharacteristic和valueChangedEventArgs。CharacteristicValue是Buffer值加上新值。
```csharp
    private static void ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs valueChangedEventArgs)
    {
        DataReader rdr = DataReader.FromBuffer(valueChangedEventArgs.CharacteristicValue);

        // 从DataReader读取值
    }
```

## 处理设备错误

对象上的ConnectionStatusChanged事件以处理连接错误BluetoothLEDevice对象。如果连接丢失，您可以尝试通过再次请求服务。看到Central2样本。

确保检查所有请求的返回状态，以确保它们是成功的。

# 蓝牙串口配置文件(SPP)

该程序集具有北欧SPP的实现，可以很容易地用于在蓝牙客户机和设备之间发送消息

这是一种简单的方式，为设备提供任何额外的信息，如WiFi详细信息。

有许多Android和IOS应用程序支持北欧SPP，可以用来发送/接收消息。

## 创建SPP实例

创建SPP实例，并为读取消息和客户机连接活动提供事件处理程序。

以设备名称开始发布广告。

使用命名空间 **nanoFramework.Device.Bluetooth.Spp**

```csharp
NordicSpp spp = new NordicSpp();
spp.ReceivedData += Spp_ReceivedData;
spp.ConnectedEvent += Spp_ConnectedEvent;

spp.Start("MySpp");

```

完成后，调用Stop方法停止SPP。

处理读数据事件

数据既可以作为字节数组读取，也可以作为字符串读取。

```csharp
private void Spp_ReceivedData(IBluetoothSpp sender, SppReceivedDataEventArgs ReadDataEventArgs)
{
    string message = ReadDataEventArgs.DataString;

    // 对传入的信息做些什么
    Debug.WriteLine($"Message:{message}");

    // 在这个例子中，我们用OK来回应
    NordicSpp spp = sender as NordicSpp;
    spp.SendString("OK");
}
```

## 处理连接事件

当客户端连接或断开与SPP服务器的连接时，将引发连接事件。

在这里，当客户机连接时，我们发送一条消息。

```csharp
private void Spp_ConnectedEvent(IBluetoothSpp sender, EventArgs e)
{
    NordicSpp spp = sender as NordicSpp;

    if (spp.IsConnected)
    {
        spp.SendString("欢迎来到nanoFramework");
    }
}
```

## 反馈和文档

关于文档，提供反馈，问题和找出如何贡献，请参考 [首页这里](https://github.com/nanoframework/Home).

加入我们的Discord社区[在这里](https://discord.gg/gCyBu8T).

## Credits

这个项目的贡献者名单可以在[贡献者](https://github.com/nanoframework/Home/blob/main/CONTRIBUTORS.md).

## 许可证

**nanoFramework** 类库是根据 [MIT license](LICENSE.md).

## 行为准则

本项目采用了《贡献者盟约》所规定的行为准则，以澄清我们社区的预期行为。
有关更多信息，请参阅 [.NET基金会行为准则](https://dotnetfoundation.org/code-of-conduct).

## .NET基金会

这个项目是由 [.NET基金会](https://dotnetfoundation.org).
