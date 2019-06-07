.\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -register:user "-filter:+[Sea Battle]* -[*Test]*" "-target:.\packages\NUnit.ConsoleRunner.3.8.0\tools\nunit-agent-x86.exe" "-targetargs:.\bin\Debug\Курсовая.exe"

.\packages\ReportGenerator.3.1.2\tools\ReportGenerator.exe "-reports:results.xml" "-targetdir:.\coverage"

pause