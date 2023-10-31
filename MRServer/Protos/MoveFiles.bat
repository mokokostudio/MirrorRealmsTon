@echo off

set "folderA=Out\Battle"
set "folderB=..\MirrorRealmsBattleServer\Proto"
set "folderC=..\..\MirrorRealms\Assets\Scripts\Proto\Battle"

REM 清空文件夹B
RD /S /Q "%folderB%"
MD "%folderB%"
REM 清空文件夹C
RD /S /Q "%folderC%"
MD "%folderC%"

REM 拷贝文件夹A内的所有文件到文件夹B
XCOPY "%folderA%\*" "%folderB%\" /E /C /H /Y
REM 拷贝文件夹A内的所有文件到文件夹C
XCOPY "%folderA%\*" "%folderC%\" /E /C /H /Y

pause