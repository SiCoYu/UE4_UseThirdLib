setlocal
set CurPath=%~dp0
set CurBinaries=%CurPath%Binaries
set PluginsPath=%CurPath%Plugins
echo CurBinaries
echo PluginsPath

echo off & color 0A

set "regexp_dll=-[0-9][0-9][0-9][0-9].dll"
set "regexp_pdb=-[0-9][0-9][0-9][0-9].pdb"

for /R %PluginsPath% %%f in (*.dll) do ( 
	set "line=%%f"
	setlocal enabledelayedexpansion	
	echo "%%f"|findstr /r /C:"%regexp_dll%" >nul 2>&1
	if ERRORLEVEL 1 (
		rem echo "%%f"
	) else (
		del /q "%%f"
		echo %%f
	)
	endlocal
)

for /R %CurBinaries% %%f in (*.pdb) do ( 
	set "line=%%f"
	setlocal enabledelayedexpansion	
	echo "%%f"|findstr /r /C:"%regexp_pdb%" >nul 2>&1
	if ERRORLEVEL 1 (
		rem echo "%%f"
	) else (
		del /q "%%f"
		echo %%f
	)
	endlocal
)

for /R %CurBinaries% %%f in (*.dll) do ( 
	set "line=%%f"
	setlocal enabledelayedexpansion	
	echo "%%f"|findstr /r /C:"%regexp_dll%" >nul 2>&1
	if ERRORLEVEL 1 (
		rem echo "%%f"
	) else (
		del /q "%%f"
		echo %%f
	)
	endlocal
)

for /R %PluginsPath% %%f in (*.pdb) do ( 
	set "line=%%f"
	setlocal enabledelayedexpansion	
	echo "%%f"|findstr /r /C:"%regexp_pdb%" >nul 2>&1
	if ERRORLEVEL 1 (
		rem echo "%%f"
	) else (
		del /q "%%f"
		echo %%f
	)
	endlocal
)