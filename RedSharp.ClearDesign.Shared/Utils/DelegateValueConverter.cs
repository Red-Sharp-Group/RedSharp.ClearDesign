using System;
using RedSharp.ClearDesign.Shared.Abstracts;

namespace RedSharp.ClearDesign.Shared.Utils
{
    public class DelegateConverter<TInput, TResult> : ConvertorBase<TInput, TResult>
    {
        private Func<TInput, TResult> _forwardConversion;
        private Func<TResult, TInput> _backwardConversion;

        public DelegateConverter(Func<TInput, TResult> forwardConversion, Func<TResult, TInput> backwardConversion = null)
        {
            _forwardConversion = forwardConversion;
            _backwardConversion = backwardConversion;
        }

        public override TResult ForwardConversion(TInput input)
        {
            return _forwardConversion.Invoke(input);
        }

        public override TInput BackwardConversion(TResult input)
        {
            if (_backwardConversion == null)
                throw new NotSupportedException();

            return _backwardConversion.Invoke(input);
        }
    }
}
