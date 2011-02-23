@echo off

rem This bat file will build a setup

"%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" /nologo /v:n /m /t:BuildSetup Build.proj

pause