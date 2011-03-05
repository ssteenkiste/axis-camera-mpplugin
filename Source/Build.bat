@echo off

rem This bat file will:
rem   1. Build the source in debug mode
rem   2. Run FxCop
rem   3. Run the tests

"%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" /nologo /v:n /m /t:CheckinGateKeeper Setup\Build.proj

pause