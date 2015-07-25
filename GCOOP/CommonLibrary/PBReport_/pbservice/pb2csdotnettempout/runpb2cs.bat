if "%NET_FRAMEWORK120%" == "" set NET_FRAMEWORK120=%windir%\Microsoft.NET\Framework\v3.5
%NET_FRAMEWORK120%\csc
 if errorlevel 9009 exit 3
setlocal
if exist "C:\PBService\pbservice\pb2csdotnettempout\*.cs" del "C:\PBService\pbservice\pb2csdotnettempout\*.cs"
rename "C:\PBService\pbservice\pb2csdotnettempout\AssemblyInfo.bak" "AssemblyInfo.cs"
"%PBNET_HOME%\bin\pb2cs" @"C:\PBService\pbservice\pb2csdotnettempout\projectinfo.txt"
if ERRORLEVEL 1 exit 1
call build.bat c- "C:\PBService\pbservice\pb2csdotnettempout\csc_output_file_name.txt"
if ERRORLEVEL 1 exit 2

@if errorlevel 1 (goto errorend)
if exist "C:\PBService\pbservice\pb2csdotnettempout\*.cs" del "C:\PBService\pbservice\pb2csdotnettempout\*.cs"

goto end

:errorend
exit /b 2

:end

endlocal
