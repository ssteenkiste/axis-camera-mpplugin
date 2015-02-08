@echo off

rem This bat file will build Axis Cameras DependencyChecker

"%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" /nologo /v:n /m /p:Configuration=Release AxisCamerasDependencyChecker.sln

copy /Y AxisCamerasDependencyChecker\bin\Release\AxisCamerasDependencyChecker.exe Deliverables\

pause