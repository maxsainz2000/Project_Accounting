Option Strict On
Option Explicit On
Option Compare Binary

Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<Assembly: DoNotParallelize>

Namespace Merchandising_Accounting_App.Tests

    <TestClass>
    Public Class AssemblySetup

        <AssemblyInitialize>
        Public Shared Sub InitializeApplicationDefaults(context As TestContext)
            Try
                Application.SetHighDpiMode(HighDpiMode.PerMonitorV2)
                Application.EnableVisualStyles()
                Application.SetCompatibleTextRenderingDefault(False)
                Application.SetDefaultFont(New Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point))
            Catch
                ' Ignore if already initialized in runner
            End Try
        End Sub

    End Class

End Namespace
