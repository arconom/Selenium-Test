call:clearDir "C:\DevEnv\jboss-eap-6.4hibernate\standalone\deployments"
call:clearDir "C:\DevEnv\jboss-eap-6.4hibernate\standalone\log"
call:clearDir "C:\DevEnv\jboss-eap-6.4hibernate\standalone\tmp"

:clearDir
cd /d %~1
for /F "delims=" %%i in ('dir /b') do (rmdir "%%i" /s/q || del "%%i" /s/q)
EXIT /B 0
