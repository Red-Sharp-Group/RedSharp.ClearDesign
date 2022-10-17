using System;
using System.Globalization;
using RedSharp.ClearDesign.Shared.Interfaces;

namespace RedSharp.ClearDesign.Shared.Abstracts
{
    public abstract class ConvertorBase<TInput, TResult> : IConvertor
    {
        Type IConvertor.TargetType => typeof(TInput);

        Type IConvertor.ResultType => typeof(TResult);

        object IConvertor.ForwardConversion(object value)
        {
            if (!(value is TInput))
                return default(TResult);

            return ForwardConversion((TInput)value);
        }

        object IConvertor.BackwardConversion(object value)
        {
            if (!(value is TResult))
                return default(TInput);

            return BackwardConversion((TResult)value);
        }

        public virtual TResult ForwardConversion(TInput input)
        {
            throw new NotImplementedException();
        }

        public virtual TInput BackwardConversion(TResult input)
        {
            throw new NotImplementedException();
        }
    }
}
