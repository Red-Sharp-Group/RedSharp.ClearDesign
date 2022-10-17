using System;
using System.Globalization;

namespace RedSharp.ClearDesign.Shared.Interfaces
{
    public interface IConvertor
    {
        Type TargetType { get; }

        Type ResultType { get; }

        object ForwardConversion(object value);

        object BackwardConversion(object value);
    }
}
