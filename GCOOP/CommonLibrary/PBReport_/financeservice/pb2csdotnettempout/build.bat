setlocal

if "%1" == "" goto usage 
if "%PBNET_HOME%" == "" goto sethome

md bin
md install

if "%1" == "c-" goto notcopy
if "%1" == "C-" goto notcopy
goto copyfile

rem ************************************************************************
rem		For release build, not copy assemblies to the folder bin 
rem ************************************************************************
:notcopy

reg import "%PBNET_HOME%\bin\PBExceptionTrace.reg"


xcopy /D /Y "c:\gcoop\pbservice\commonservice\commonservice.pbd" bin\

xcopy /D /Y "c:\gcoop\pbservice\commonservice\commonservice.pbd" install\

xcopy /D /Y "c:\gcoop\pbservice\financeservice\financeservice.pbd" bin\

xcopy /D /Y "c:\gcoop\pbservice\financeservice\financeservice.pbd" install\


if "%~2" == "" (
csc /t:library /debug /nowarn:1591 /doc:"bin\pbservice.finance.xml"   /platform:x86  /linkres:bin\commonservice.pbd /linkres:bin\financeservice.pbd "/r:%PBNET_HOME%\bin\Sybase.PowerBuilder.Core.dll;%PBNET_HOME%\bin\Sybase.PowerBuilder.Interop.dll;%PBNET_HOME%\bin\Sybase.PowerBuilder.Common.dll;%PBNET_HOME%\bin\Sybase.PowerBuilder.Win.dll;%PBNET_HOME%\bin\Sybase.PowerBuilder.WinWebDataWindowCommon.dll;%PBNET_HOME%\bin\com.sybase.ejb.net.dll;%PBNET_HOME%\bin\com.sybase.iiop.net.dll" /out:"bin\pbservice.finance.dll" *.cs
) else (
csc /t:library /debug /nowarn:1591 /doc:"bin\pbservice.finance.xml"   /platform:x86  /linkres:bin\commonservice.pbd /linkres:bin\financeservice.pbd "/r:%PBNET_HOME%\bin\Sybase.PowerBuilder.Core.dll;%PBNET_HOME%\bin\Sybase.PowerBuilder.Interop.dll;%PBNET_HOME%\bin\Sybase.PowerBuilder.Common.dll;%PBNET_HOME%\bin\Sybase.PowerBuilder.Win.dll;%PBNET_HOME%\bin\Sybase.PowerBuilder.WinWebDataWindowCommon.dll;%PBNET_HOME%\bin\com.sybase.ejb.net.dll;%PBNET_HOME%\bin\com.sybase.iiop.net.dll" /out:"bin\pbservice.finance.dll" *.cs > "%~2"
)

@if errorlevel 1 (goto builderror) else echo Build Succeeded.
goto end

rem ************************************************************************
rem		For debug build, copy assemblies
rem ************************************************************************
:copyfile
xcopy /D /Y "%PBNET_HOME%\bin\Sybase.Data.AseClient.dll" bin\
xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.Ado.dll" bin\
xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.Db*" bin\

xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.Core.dll" bin\
if exist "%PBNET_HOME%\bin\Sybase.PowerBuilder.Core.pdb" xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.Core.pdb" bin\

xcopy /D /Y /E "%PBNET_HOME%\bin\de\*.*" bin\de\
xcopy /D /Y /E "%PBNET_HOME%\bin\fr\*.*" bin\fr\

xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.Interop.dll" bin\
if exist "%PBNET_HOME%\bin\Sybase.PowerBuilder.Interop.pdb" xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.Interop.pdb" bin\

xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.Common.dll" bin\
if exist "%PBNET_HOME%\bin\Sybase.PowerBuilder.Common.pdb" xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.Common.pdb" bin\

xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.Win.dll" bin\
if exist "%PBNET_HOME%\bin\Sybase.PowerBuilder.Win.pdb" xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.Win.pdb" bin\

xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.DataWindow.Win.dll" bin\
if exist "%PBNET_HOME%\bin\Sybase.PowerBuilder.DataWindow.Win.pdb" xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.DataWindow.Win.pdb" bin\
xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.DataWindow.Win.tlb" bin\

xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.WinWebDataWindowCommon.dll" bin\
if exist "%PBNET_HOME%\bin\Sybase.PowerBuilder.WinWebDataWindowCommon.pdb" xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.WinWebDataWindowCommon.pdb" bin\

xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.DataWindow.Interop.*" bin\

xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.EditMask.Win.*" bin\
xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.EditMask.Interop.*" bin\

xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.Graph*" bin\

xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.WebService.Runtime*.dll" bin\
if exist "%PBNET_HOME%\bin\Sybase.PowerBuilder.WebService.Runtime*.pdb" xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.WebService.Runtime*.pdb" bin\

xcopy /D /Y "%PBNET_HOME%\bin\Microsoft.Ink*.dll" bin\

xcopy /D /Y "%PBNET_HOME%\bin\com.sybase.ejb.net.dll" bin\
xcopy /D /Y "%PBNET_HOME%\bin\com.sybase.iiop.net.dll" bin\

xcopy /D /Y "%PBNET_HOME%\bin\Sybase.PowerBuilder.DataWindow.Excel12.dll" bin\

reg import "%PBNET_HOME%\bin\PBExceptionTrace.reg"


xcopy /D /Y "c:\gcoop\pbservice\commonservice\commonservice.pbd" bin\

xcopy /D /Y "c:\gcoop\pbservice\commonservice\commonservice.pbd" install\

xcopy /D /Y "c:\gcoop\pbservice\financeservice\financeservice.pbd" bin\

xcopy /D /Y "c:\gcoop\pbservice\financeservice\financeservice.pbd" install\


if "%~2" == "" (
csc /t:library /debug /nowarn:1591 /doc:"bin\pbservice.finance.xml"   /platform:x86  /linkres:bin\commonservice.pbd /linkres:bin\financeservice.pbd "/r:bin\Sybase.PowerBuilder.Core.dll;bin\Sybase.PowerBuilder.Interop.dll;bin\Sybase.PowerBuilder.Common.dll;bin\Sybase.PowerBuilder.Win.dll;bin\Sybase.PowerBuilder.DataWindow.Win.dll;bin\Sybase.PowerBuilder.WinWebDataWindowCommon.dll;bin\com.sybase.ejb.net.dll;bin\com.sybase.iiop.net.dll" /out:"bin\pbservice.finance.dll" *.cs
) else (
csc /t:library /debug /nowarn:1591 /doc:"bin\pbservice.finance.xml"   /platform:x86  /linkres:bin\commonservice.pbd /linkres:bin\financeservice.pbd "/r:bin\Sybase.PowerBuilder.Core.dll;bin\Sybase.PowerBuilder.Interop.dll;bin\Sybase.PowerBuilder.Common.dll;bin\Sybase.PowerBuilder.Win.dll;bin\Sybase.PowerBuilder.DataWindow.Win.dll;bin\Sybase.PowerBuilder.WinWebDataWindowCommon.dll;bin\com.sybase.ejb.net.dll;bin\com.sybase.iiop.net.dll" /out:"bin\pbservice.finance.dll" *.cs > "%~2"
)

@if errorlevel 1 (goto builderror) else echo Build Succeeded.
goto end

:builderror
@echo.
@echo Build Failed.
goto errorend

:usage
@echo Usage: build c+/c- [tempfile]
goto errorend

:sethome
@echo The PBNET_HOME system environment variable is not set. Please set it to the location of your PowerBuilder 11.0\DotNET directory.

:errorend
exit /b 1

:end
xcopy /D /Y "bin\pbservice.finance.dll" install\

endlocal