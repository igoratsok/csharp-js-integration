# JasperServer Integration
JSIntegration for C# quickly integrates C# to Jasper Server and retrieves the binary data of the exported report.

# Usage
Usage is quite simple:

```csharp
using System;
using System.IO;
using S2Station.JSIntegration;
using System.Collections.Generic;

namespace TesteJSIntegration
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // Optional parameters
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("P_ID_ALUNO", "1");

            // Creating JasperServerIntegration object
            JasperServerIntegration jsi = new JasperServerIntegration(
                "http://localhost:8080/jasperserver", // Jasper Server base URL
                "reports/aluno_escola_filtro",        // Path to the Report Unit
                "pdf",                                // Export type
                "jasperadmin",                        // User
                "jasperadmin",                        // Password
                parameters                            // Optional parameters
            );

            try
            {
                // Byte array with the report
                byte[] report = jsi.Execute();

                // You can do anything: here, I'm going to save the report to the disk
                File.WriteAllBytes("report.pdf", report);

                Console.WriteLine("OK!");
            } catch(JSIntegrationException e)
            {
                Console.WriteLine("Error! " + e.ErrorCode + ": " + e.ErrorMessage);
            }

        }
    }
}

```
