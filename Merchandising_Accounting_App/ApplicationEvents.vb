Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
    Partial Friend Class MyApplication
        Private Sub MyApplication_ApplyApplicationDefaults(sender As Object, e As ApplyApplicationDefaultsEventArgs) Handles Me.ApplyApplicationDefaults
            e.Font = New Font("Segoe UI Variable Text", 9.5F, FontStyle.Regular, GraphicsUnit.Point)
            e.HighDpiMode = HighDpiMode.PerMonitorV2
            e.ColorMode = SystemColorMode.System
        End Sub
    End Class
End Namespace
