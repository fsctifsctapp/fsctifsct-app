csc
 if errorlevel 9009 exit 3
setlocal
if exist "C:\GCOOP\PBService\misservice\pb2csdotnettempout\*.cs" del "C:\GCOOP\PBService\misservice\pb2csdotnettempout\*.cs"
rename "C:\GCOOP\PBService\misservice\pb2csdotnettempout\AssemblyInfo.bak" "AssemblyInfo.cs"
"%PBNET_HOME%\bin\pb2cs" @"C:\GCOOP\PBService\misservice\pb2csdotnettempout\projectinfo.txt"
if ERRORLEVEL 1 exit 1
call build.bat c- "C:\GCOOP\PBService\misservice\pb2csdotnettempout\csc_output_file_name.txt"
if ERRORLEVEL 1 exit 2

@if errorlevel 1 (goto errorend)
if exist "C:\GCOOP\PBService\misservice\pb2csdotnettempout\*.cs" del "C:\GCOOP\PBService\misservice\pb2csdotnettempout\*.cs"

goto end

:errorend
exit /b 2

:end

endlocal
