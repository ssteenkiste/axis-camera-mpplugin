@echo off

rem This bat file will build the externals

"%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" /nologo /v:n /m /t:BuildExternals Setup\Build.proj

pause