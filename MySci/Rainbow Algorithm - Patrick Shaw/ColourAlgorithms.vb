Imports System.Windows.Media

Public Class ColourAlgorithms

    Public Shared Function ColourValue(input As Double, snakeNo As Long) As Color
        Dim R As Double = 255
        Dim G As Double = 255
        Dim B As Double = 255


        'RAIIIIIIIIIIIIIIINBBBBBBBBBBBBBBOOOOOOOOOOOOOWWWWWWWWWWSSSSSSS!

        Dim rainbowOption As Long = snakeNo Mod 8
        Select Case rainbowOption
            Case 0
                CalcRainbowColour(input + snakeNo, G, B, R)
            Case 1
                CalcRainbowColour(input + snakeNo, R, G, B)
            Case 2
                CalcRainbowColour(input + snakeNo, B, G, R)
            Case 3
                CalcRainbowColour(input + snakeNo, G, B, R)
            Case 4
                CalcRainbowColour(input + snakeNo, R, B, G)
            Case 5
                CalcRainbowColour(input + snakeNo, G, R, B)
            Case 6
                CalcRainbowColour(input + snakeNo, B, R, G)
            Case 7
                CalcRainbowColour(input + snakeNo, R, G, B)
            Case 8
                CalcRainbowColour(input + snakeNo, B, R, G)
        End Select

        Return Color.FromRgb(R, G, B)
    End Function
    Private Shared Sub CalcStripeColour(ByVal input As Single, ByRef R As Double, ByRef G As Double, ByRef B As Double)
        Dim sineTemp As Single = Math.Sin(input / 0.8) * 100.0

        R += sineTemp
        If R > 255 Then R = 255
        If R < 0 Then R = 0
        G += sineTemp
        If G > 255 Then G = 255
        If G < 0 Then G = 0
        B += sineTemp
        If B > 255 Then B = 255
        If B < 0 Then B = 0
    End Sub
    Private Shared Sub CalcRainbowColour(ByVal input As Double, ByRef R As Double, ByRef G As Double, ByRef B As Double)
        Const Rate = 20.0
        input *= Rate

Begin:

        If input >= 1785 Then
            input -= 1530
            GoTo Begin
        End If
        Select Case input
            Case 0 To 255
                'White to Yellow'
                CCDown(B, input)
            Case 256 To 510
                'Yellow to Red'
                input -= 255
                B = 0
                CCDown(G, input)
            Case 511 To 765
                'Red to Purple'
                G = 0
                B = 0
                input -= 510
                CCUp(B, input)
            Case 766 To 1020
                'Purple to Blue'
                input -= 765
                G = 0
                CCDown(R, input)
            Case 1021 To 1275
                'Blue to Cyan'
                input -= 1020
                R = 0
                G = 0
                CCUp(G, input)
            Case 1276 To 1530
                'Cyan to Green'
                input -= 1275
                R = 0
                CCDown(B, input)
            Case 1531 To 1785
                'Green to Yellow'
                B = 0
                R = 0
                input -= 1530
                CCUp(R, input)
        End Select
    End Sub

    Private Shared Sub CCDown(ByRef colour As Double, ByRef input As Double)
        If input >= 255 Then
            colour = 0
            input -= 255
        Else
            colour -= input
            input = 0
        End If
    End Sub
    Private Shared Sub CCUp(ByRef colour As Double, ByRef input As Double)
        If input >= 255 Then
            colour = 255
            input -= 255
        Else
            colour += input
            input = 0
        End If
    End Sub
End Class