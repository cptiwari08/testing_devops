using System.ComponentModel.DataAnnotations;

namespace EY.CE.Copilot.API.Static
{
    public class  ScriptNotAllowed : RegularExpressionAttribute
    {
        public ScriptNotAllowed() : base("(?i)^((?!<[\\s\\S]*script[\\s\\S]*?>[\\s\\S]*?<[\\s\\S]*/script[\\s\\S]*>).)*$")
        {
            ErrorMessage = "Field value cannot have script tag";
        }
    }

    public class SpecialCharNotAllowed : RegularExpressionAttribute
    {
        public SpecialCharNotAllowed() : base("^[a-zA-Z0-9].*")
        {
            ErrorMessage = "Field value cannot start with special characters";
        }
    }
}
