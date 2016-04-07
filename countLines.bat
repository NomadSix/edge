@ECHO OFF
cd %cd%
PowerShell.exe "(dir -include *.cs,*.xaml -recurse | select-string .).Count"
Echo Lines of Code
PAUSE