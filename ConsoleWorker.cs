using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

static public class ConsoleWorker
{    
    [DllImport("User32.dll", CharSet = CharSet.Unicode)]
    public static extern int MessageBox(IntPtr h, string message, string title, long type);

    public static void MainWorker()
    {
        Console.CursorVisible = false;
        Console.Write(@"a * x^2 + b * x + c = 0
Введите значение
>a: 
b: 
c: 
Для завершения ввода значения нажмите пробел
Для решения нажмите ENTER");
        Console.SetCursorPosition(4,2);
        CursorPosition();
    }

    private static void CursorPosition()
    {
        #region INIT VARIEBLE        
        int rowSum = 0;
        ConsoleKeyInfo keyInfo = Console.ReadKey();
        #endregion

        while(keyInfo.Key != ConsoleKey.Enter)
        {
            if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                rowSum += 1;
                if (rowSum + ProgramHelper.origRow > 4) rowSum = 2;
                if (rowSum + ProgramHelper.origRow < 2) rowSum = 2;                   
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                rowSum -= 1;
                if (rowSum + ProgramHelper.origRow < 2) rowSum = 4;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                if (ProgramHelper.values.Count < 3)
                {
                    ProgramHelper.ErrorDict.Add("Не все переменные заполнены", "Решение не возможно");                    
                }
                break;
            }
            else
            {
                string input = keyInfo.KeyChar.ToString();
                ConsoleKeyInfo stoKey = Console.ReadKey();
                while (stoKey.Key != ConsoleKey.Spacebar)
                {
                    input += stoKey.KeyChar;
                    stoKey = Console.ReadKey();
                }
                if(rowSum == 0)
                    rowSum = 2;
                ReadValue(input, rowSum + ProgramHelper.origRow);                    
            }                    
            WriteAt($"{GetString(rowSum + ProgramHelper.origRow)}", rowSum, 0);
            keyInfo = Console.ReadKey();
        }
        Console.SetCursorPosition(0, 7);
        Console.WriteLine("===================================");
        
    }

    private static void ReadValue(string inputString, int row)
    {        
        switch (row)
        {
            case 2:
                try
                {
                    SetValue(inputString, "a");
                }catch(NonIntValue nEx)
                {
                    MessageBox((IntPtr)0, nEx.Message, "Ошибка", 0x00000020L);
                }
                break;
            case 3:
                try
                {
                    SetValue(inputString, "b");
                }catch(NonIntValue nEx)
                {
                    MessageBox((IntPtr)0, nEx.Message, "Ошибка", 0x00000020L);
                }
                break;
            case 4:
                try
                {
                    SetValue(inputString, "c");
                }catch(NonIntValue nEx)
                {
                    MessageBox((IntPtr)0, nEx.Message, "Ошибка", 0x00000020L);
                }
                break;
        }
    }

    private static void SetValue(string inputString, string dictionariKey)
    {
        var argValue = 0;
        if (int.TryParse(inputString, out argValue))
        {
            if (ProgramHelper.values.ContainsKey(dictionariKey)) ProgramHelper.values[dictionariKey] = argValue;
            else ProgramHelper.values.Add(dictionariKey, argValue);
        }
        else
        {
            throw new NonIntValue("Введеное значение не является числом!\n Введите корректное значение", dictionariKey, inputString);
        }
    }
    static void WriteAt(string s, int x, int y)
    {
        Console.Clear();
        int position = ProgramHelper.origRow + x;
        if(position == 0)
        {
            position = 2;
        }
        Console.Write(s);
        if (position >= 2 && position < 5)
        {
            Console.SetCursorPosition(4, position);
        }
    }

    static StringBuilder GetString(int row)
    {
        StringBuilder sb = new StringBuilder();
        switch (row)
        {
            case 2:
                sb.Append($@"a * x^2 + b * x + c = 0
Введите значение
>a: {ProgramHelper.values["a"]}
b: {ProgramHelper.values["b"]}
c: {ProgramHelper.values["c"]}
Для завершения ввода значения нажмите пробел
Для решения нажмите ENTER");
                break;
            case 3:
                sb.Append($@"a * x^2 + b * x + c = 0
Введите значение
a: {ProgramHelper.values["a"]}
>b: {ProgramHelper.values["b"]}
c: {ProgramHelper.values["c"]}
Для завершения ввода значения нажмите пробел
Для решения нажмите ENTER");
                break;
            case 4:
                sb.Append($@"a * x^2 + b * x + c = 0
Введите значение
a: {ProgramHelper.values["a"]}
b: {ProgramHelper.values["b"]}
>c: {ProgramHelper.values["c"]}
Для завершения ввода значения нажмите пробел
Для решения нажмите ENTER");
                break;
        }            
        return sb;
    } 
}