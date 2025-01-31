using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain;

public record Error
{
    private Error(string code, string message)
    {
        Code = code;
        Message = message;
    }
    public string Code { get; private set; }
    public string Message { get; private set; }
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "The result value is null!");
    public static Error Create(string code, string message) => new(code, message);
}