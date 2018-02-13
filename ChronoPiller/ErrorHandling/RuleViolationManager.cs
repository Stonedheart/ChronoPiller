using System.Collections.Generic;

namespace ChronoPiller.ErrorHandling
{
    public static class RuleViolationManager
    {
        public static List<RuleViolation> GetIntRuleViolations(string[] parameters)
        {
            var ruleViolations = new List<RuleViolation>();
            int outInt;

            foreach (var element in parameters)
            {
                if (!int.TryParse(element, out outInt))
                {
                    ruleViolations.Add(new RuleViolation() {Message = ""});
                }
            }

            return ruleViolations;
        }

        public static List<RuleViolation> GetStringTuleViolations(string[] parameters)
        {
            var ruleViolations = new List<RuleViolation>();



            return ruleViolations;
        }

        public static List<RuleViolation> GetDateRuleViolations(string[] parameters)
        {
            var ruleViolations = new List<RuleViolation>();



            return ruleViolations;
        }

    }
}