@echo off
set consolepath=C:\NUnit.Console-3.7.0
set projectpath=D:\Automation\QC HN\Standard Frameworks\CSharp\NT_CoreFramework\WebAutomationTest
set bindir=bin\Debug

"%consolepath%\nunit3-console" "%projectpath%\%bindir%\WebAutomationTest.dll" --test="WebAutomationTest.RegressionTests.TestCases.LoginSuccessTest.LoginSuccess1"