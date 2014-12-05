@echo off
set nosign=false
set version=%1
if "%1" == "" echo ��������汾�ţ�
if "%1" == "" goto :eof
if "%2" == "" set nosign=true
cd /d %~dp0
@call "%VS110COMNTOOLS%\..\..\VC\vcvarsall.bat" x86
@set "PATH=%WindowsSdkDir%bin;%PATH%"
set projectdir=..\DoubanFM\bin\Release
set imagedir=Images
set tempdir=Temp
set outputdir=Output
set compile=C:\Program Files (x86)\NSIS\Unicode\makensis.exe
set setup=DoubanFMSetup_%version%.exe

:copy
xcopy "%imagedir%" "%tempdir%\" /Q/E/Y
if %errorlevel% NEQ 0 goto :clear
xcopy "%projectdir%" "%tempdir%\" /Q/E/Y
if %errorlevel% NEQ 0 goto :clear
xcopy "..\LICENSE.txt" "%tempdir%\" /Q/Y
if %errorlevel% NEQ 0 goto :clear
xcopy "..\README.txt" "%tempdir%\" /Q/Y
if %errorlevel% NEQ 0 goto :clear
xcopy "DoubanFM.nsi" "%tempdir%\" /Q/Y
if %errorlevel% NEQ 0 goto :clear
if "%nosign%" == "true" goto :compile
:sign
call :SignFile "%2" "%tempdir%\DoubanFM.exe"
if %errorlevel% NEQ 0 goto :clear
call :SignFile "%2" "%tempdir%\DoubanFM.Core.dll"
if %errorlevel% NEQ 0 goto :clear
call :SignFile "%2" "%tempdir%\DoubanFM.Bass.dll"
if %errorlevel% NEQ 0 goto :clear
call :SignFile "%2" "%tempdir%\Hardcodet.Wpf.TaskbarNotification.dll"
if %errorlevel% NEQ 0 goto :clear
:compile
"%compile%" "/XOutFile \"%setup%\"" "/X!define PRODUCT_VERSION \"%version%\"" "%tempdir%\DoubanFM.nsi"
if %errorlevel% NEQ 0 goto :clear
xcopy "%tempdir%\%setup%" "%outputdir%\" /Q/Y
if %errorlevel% NEQ 0 goto :clear
if "%nosign%" == "true" goto :end
:signsetup
call :SignFile "%2" "%outputdir%\%setup%"
if %errorlevel% NEQ 0 goto :clear

:end
if "%nosign%" == "true" echo ���棺û������ǩ��
goto :clear

:clear
set error=%errorlevel%
if exist "%tempdir%" rmdir "%tempdir%" /s /q
goto :eof

:SignFile
signtool sign /f ..\key.pfx /p "%1" /t "http://timestamp.globalsign.com/scripts/timstamp.dll" "%2"
@exit /B %errorlevel%