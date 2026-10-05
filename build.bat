@echo off
setlocal
set "CSC=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if not exist "%CSC%" set "CSC=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\csc.exe"
if not exist "%CSC%" (
    echo O compilador nativo do .NET Framework nao foi encontrado.
    exit /b 1
)
"%CSC%" /nologo /target:winexe /optimize+ /out:Vesper.exe /reference:System.Windows.Forms.dll /reference:System.Drawing.dll /reference:System.Security.dll Vesper.cs
if errorlevel 1 exit /b 1
echo Vesper.exe compilado sem instalar pacotes.
