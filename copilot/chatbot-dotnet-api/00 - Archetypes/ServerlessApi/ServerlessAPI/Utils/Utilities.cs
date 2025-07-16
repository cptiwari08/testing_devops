namespace Utils
{
    public static class Utilities
    {
        public static string GetEnvironmentVariable(string variableName)
        {
            string? variableValue = Environment.GetEnvironmentVariable(variableName);

            if (string.IsNullOrEmpty(variableValue))
            {
                throw new ArgumentNullException($"The environment variable '{variableName}' does not have a value.");
            }

            return variableValue;
        }
    }
}
