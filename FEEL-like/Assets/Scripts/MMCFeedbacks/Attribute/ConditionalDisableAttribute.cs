using System;
using UnityEngine;

namespace MMCFeedbacks.Attribute
{
    public class ConditionalDisableAttribute : PropertyAttribute
    {
        public readonly float ComparedFloat;
        public readonly int ComparedInt;

        public readonly string ComparedStr;
        public readonly bool ConditionalInvisible;
        public readonly bool TrueThenDisable;
        public readonly string VariableName;
        public readonly Type VariableType;

        private ConditionalDisableAttribute(string variableName, Type variableType, bool trueThenDisable = false,
            bool conditionalInvisible = false)
        {
            VariableName = variableName;
            VariableType = variableType;
            TrueThenDisable = trueThenDisable;
            ConditionalInvisible = conditionalInvisible;
        }

        public ConditionalDisableAttribute(string boolVariableName, bool trueThenDisable = false,
            bool conditionalInvisible = false)
            : this(boolVariableName, typeof(bool), trueThenDisable, conditionalInvisible)
        {
        }

        public ConditionalDisableAttribute(string strVariableName, string comparedStr, bool notEqualThenEnable = false,
            bool conditionalInvisible = false)
            : this(strVariableName, comparedStr.GetType(), notEqualThenEnable, conditionalInvisible)
        {
            ComparedStr = comparedStr;
        }

        public ConditionalDisableAttribute(string intVariableName, int comparedInt, bool notEqualThenEnable = false,
            bool conditionalInvisible = false)
            : this(intVariableName, comparedInt.GetType(), notEqualThenEnable, conditionalInvisible)
        {
            ComparedInt = comparedInt;
        }

        public ConditionalDisableAttribute(string floatVariableName, float comparedFloat,
            bool greaterThanComparedThenEnable = true, bool conditionalInvisible = false)
            : this(floatVariableName, comparedFloat.GetType(), greaterThanComparedThenEnable, conditionalInvisible)
        {
            ComparedFloat = comparedFloat;
        }
    }
}