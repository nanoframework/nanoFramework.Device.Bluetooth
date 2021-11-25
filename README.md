[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nanoframework_nanoFramework.Device.Bluetooth&metric=alert_status)](https://sonarcloud.io/dashboard?id=nanoframework_nanoFramework.Device.Bluetooth) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=nanoframework_nanoFramework.Device.Bluetooth&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=nanoframework_nanoFramework.Device.Bluetooth) [![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE) [![NuGet](https://img.shields.io/nuget/dt/nanoFramework.Device.Bluetooth.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.Device.Bluetooth/) [![#yourfirstpr](https://img.shields.io/badge/first--timers--only-friendly-blue.svg)](https://github.com/nanoframework/Home/blob/main/CONTRIBUTING.md) [![Discord](https://img.shields.io/discord/478725473862549535.svg?logo=discord&logoColor=white&label=Discord&color=7289DA)](https://discord.gg/gCyBu8T)

![nanoFramework logo](https://raw.githubusercontent.com/nanoframework/Home/main/resources/logo/nanoFramework-repo-logo.png)

-----

# Welcome to the .NET **nanoFramework** nanoFramework.Device.BluetoothLibrary repository

## Build status

| Component | Build Status | NuGet Package |
|:-|---|---|
| nanoFramework.Device.Bluetooth | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.Device.Bluetooth/_apis/build/status/nanoframework.nanoFramework.Device.Bluetooth?repoName=nanoframework%2FnanoFramework.Device.Bluetooth&branchName=main)](https://dev.azure.com/nanoframework/nanoFramework.Device.Bluetooth/_build/latest?definitionId=85&repoName=nanoframework%2FnanoFramework.Device.Bluetooth&branchName=main) | [![NuGet](https://img.shields.io/nuget/v/nanoFramework.Device.Bluetooth.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.Device.Bluetooth/) |
| nanoFramework.Device.Bluetooth (preview) | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.Device.Bluetooth/_apis/build/status/nanoframework.nanoFramework.Device.Bluetooth?repoName=nanoframework%2FnanoFramework.Device.Bluetooth&branchName=develop)](https://dev.azure.com/nanoframework/nanoFramework.Device.Bluetooth/_build/latest?definitionId=85&repoName=nanoframework%2FnanoFramework.Device.Bluetooth&branchName=develop) | [![NuGet](https://img.shields.io/nuget/vpre/nanoFramework.Device.Bluetooth.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.Device.Bluetooth/) |

## nanoFramework.Device.Bluetooth class Library

Bluetooth Low Energy library.

This library is based on the Windows.Devices.Bluetooth UWP class library but simplified and with all the async related calls removed.
The original .Net assembly depended on Windows.Storage.Streams for DataReader & DataWriter; this library has simplified inbuilt versions. References to IBuffer in .Net examples should now use Buffer instead.

Currently only supported on ESP32 devices with following firmware.

- ESP32_BLE_REV0
- ESP32_BLE_REV3

The restriction is due to IRam memory space in the firmware image. 
With revision 1 ESP32 devices the PSRAM implementation requires PSRAM fixes which takes space
in IRam so PSRAM is disabled for ESP32_BLE_REV0. With revision 3 devices the Bluetooth and 
PSRAM fit and available.

## Usage


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

