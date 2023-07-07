Console.Clear();
ConsoleWorker.MainWorker();
try
{
    var result = Discrimenamt();
    for(int i = 0; i < result.Count; i++)
        Console.WriteLine(result[i]);
}
catch (NonDecision ex)
{
    ProgramHelper.FormData(ex.Message, ProgramHelper.Severity.Warning, ProgramHelper.ErrorDict);
}
Console.ReadKey();

static List<double> Discrimenamt()
{
    var values = ProgramHelper.values;
    List<double> result = new List<double>();
    double x1 = 0, x2 = 0;
    var discriminant = Math.Pow(values["b"], 2) - 4 * values["a"] * values["c"];
    if (discriminant < 0)
    {
        ProgramHelper.ErrorDict.Add("х1", "решений нет");
        ProgramHelper.ErrorDict.Add("х2", "решений нет");
        throw new NonDecision("Вещественных значений не найдено");
    }
    else
    {
        if (discriminant == 0) //квадратное уравнение имеет два одинаковых корня
        {                    
            result.Add(x1 = -values["b"] / (2 * values["a"]));
        }
        else //уравнение имеет два разных корня
        {
            result.Add(x1 = (-values["b"] + Math.Sqrt(discriminant)) / (2 * values["a"]));
            result.Add(x2 = (-values["b"] - Math.Sqrt(discriminant)) / (2 * values["a"]));      
        }                
    }    
    return result;
}

public static class ProgramHelper
{
    static public int origRow = 0;    
    static public IDictionary<string, string> ErrorDict = new Dictionary<string, string>();
    static public Dictionary<string, int> values = new Dictionary<string, int>()
    {
        {"a",0},
        {"b",0},
        {"c",0}
    };
    public enum Severity
    {
        Error,
        Warning
    };

    public static void FormData(string message, ProgramHelper.Severity severity, IDictionary<string, string> data)
    {
        if (severity == ProgramHelper.Severity.Warning)
            Console.BackgroundColor = ConsoleColor.Yellow;
        else
            Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine($"{message}");
        var keys = data.Keys.ToList();
        for(int i = 0; i < keys.Count; i++)
        {
            Console.WriteLine($"{keys[i]} = {data[keys[i]]}");
        }
    }
}

public class NonIntValue : Exception
{
    public NonIntValue(string message, string dictionariKey, string inputString) : base(message) 
    { 
        if(ProgramHelper.ErrorDict.ContainsKey(dictionariKey)) 
            ProgramHelper.ErrorDict[dictionariKey] = $"{ProgramHelper.ErrorDict[dictionariKey]}, {inputString}";
        else ProgramHelper.ErrorDict.Add(dictionariKey, inputString);
    }
}

public class NonDecision : Exception
{
    public NonDecision(string msg) : base(msg) 
    {
        
    }
}
