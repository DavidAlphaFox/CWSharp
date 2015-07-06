@echo Off
set framework=%1
if "%framework%" == "" (
   set framework=v4.5
)
set config=%2
if "%config%" == "" (
   set config=Release
)
msbuild src\Yamool.CWSharp.csproj /p:Configuration="%config%" /p:TargetFrameworkVersion="%framework%" /m /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false
