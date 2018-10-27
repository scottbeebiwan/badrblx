@echo off
call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\Tools\VsDevCmd.bat"
prompt $G
@echo on
msbuild /p:Configuration=Release badrblx-launcher.sln
rd /s /q bin 1>nul 2>nul
md bin 1>nul 2>nul
move badrblx-installer\bin\release\badrblx-installer.exe bin\
move badrblx-launcher\bin\release\badrblx-launcher.exe bin\