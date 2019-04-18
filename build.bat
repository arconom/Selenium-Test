
dir /s /B src\*.java > sources.txt
dir /s /B *.jar, *.class > tmpjars.txt
@echo Class-Path: > jars.txt
@echo off
setLocal EnableDelayedExpansion
for /f "tokens=* delims= " %%a in (tmpjars.txt) do (
set /a N+=1
echo ^ %%a^ >>jars.txt
)
jar cfm manifest.jar jars.txt
if exist tmpclasses (
	rd /s /q tmpclasses 
	md tmpclasses
) else (
	md tmpclasses
)
javac -target 1.8 -source 1.8 -classpath manifest.jar -d ./tmpclasses @sources.txt 2> errors.txt 1>output.txt
del sources.txt
del tmpjars.txt
del jars.txt
xcopy src tmpclasses /y /e /i /exclude:exclude.txt >nul
xcopy webapp staged /y /e /i >nul
xcopy tmpclasses staged\WEB-INF\classes /y /i /e >nul
xcopy staged C:\DevEnv\jboss-eap-6.4hibernate\standalone\deployments\asm.war\ /y /e /i >nul
type nul >C:\DevEnv\jboss-eap-6.4hibernate\standalone\deployments\asm.war.dodeploy

REM call "C:\DevEnv\jboss-eap-6.4hibernate\bin\jboss-cli.bat" --connect --controller=localhost:9999 command=:shutdown
REM call "C:\DevEnv\jboss-eap-6.4hibernate\bin\standalone.bat"

REM this is for making an actual jar
REM jar cf asm.war .\staged\*
REM copy asm.war C:\DevEnv\jboss-eap-6.4hibernate\standalone\deployments\asm.war
